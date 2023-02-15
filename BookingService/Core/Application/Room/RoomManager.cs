using Application.Room.DTO;
using Application.Room.Ports;
using Application.Room.Requests;
using Application.Room.Responses;
using Application.Responses;
using Application.Room.Ports;
using Domain.Guest.DomainExceptions;
using Domain.Guest.Ports;
using Domain.Room.Ports;

namespace Application.Room
{
    public class RoomManager : IRoomManager
    {
        private IRoomRepository _roomRepository;
        public RoomManager(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }
        public async Task<RoomResponse> CreateRoom(CreateRoomRequest request)
        {
            try
            {
                var room = RoomDto.MapToEntity(request.Data);

                await room.Save(_roomRepository);

                request.Data.Id = room.Id;

                return new RoomResponse
                {
                    Data = request.Data,
                    Success = true,
                };
            }
            catch (InvalidRoomDataException e)
            {
                return new RoomResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ROOM_MISSING_REQUIRED_INFORMATION,
                    Message = "Missing required information passed"
                };
            }
            catch (Exception)
            {
                return new RoomResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ROOM_COULDNT_STORE_DATA,
                    Message = "There was an error when saving to DB"
                };
            }
        }

        public async Task<RoomResponse> GetRoom(int roomId)
        {
            var room = await _roomRepository.Get(roomId);

            if (room == null)
            {
                return new RoomResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ROOM_NOT_FOUND,
                    Message = "No Guest record has found with the given ID"
                };
            }

            return new RoomResponse
            {
                Data = RoomDto.MapToDto(room),
                Success = true,
            };
        }
    }
}
