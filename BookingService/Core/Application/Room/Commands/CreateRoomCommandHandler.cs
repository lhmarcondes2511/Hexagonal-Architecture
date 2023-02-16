using Application.Responses;
using Application.Room.DTO;
using Application.Room.Responses;
using Domain.Guest.DomainExceptions;
using Domain.Room.DomainExceptions;
using Domain.Room.Ports;
using MediatR;

namespace Application.Room.Commands
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, RoomResponse>
    {
        private readonly IRoomRepository _roomRepository;

        public CreateRoomCommandHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<RoomResponse> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var room = RoomDto.MapToEntity(request.RoomDto);

                await room.Save(_roomRepository);

                request.RoomDto.Id = room.Id;

                return new RoomResponse
                {
                    Data = request.RoomDto,
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
            catch (InvalidRoomPriceException e)
            {
                return new RoomResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ROOM_MISSING_REQUIRED_INFORMATION,
                    Message = "Room price is invalid"
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
    }
}
