using Application.Guest.DTO;
using Application.Guest.Ports;
using Application.Guest.Requests;
using Application.Guest.Responses;
using Application.Responses;
using Domain.Guest.DomainExceptions;
using Domain.Guest.Ports;

namespace Application.Guest
{
    public class GuestManager : IGuestManager
    {
        private IGuestRepository _guestRepository;
        public GuestManager(IGuestRepository guestRepository)
        {
            _guestRepository = guestRepository;
        }
        public async Task<GuestResponse> CreateGuest(CreateGuestRequest request)
        {
            try
            {
                var guest = GuestDto.MapToEntity(request.Data);

                await guest.Save(_guestRepository);

                request.Data.Id = guest.Id;

                return new GuestResponse
                {
                    Data = request.Data,
                    Success = true,
                };
            }
            catch (InvalidPersonDocumentIdException e)
            {
                return new GuestResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.INVALID_DOCUMENT_ID,
                    Message = "This ID passed is not valid"
                };
            }
            catch (MissingRequiredInformation e)
            {
                return new GuestResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.MISSING_REQUIRED_INFORMATION,
                    Message = "Missing required information passed"
                };
            }
            catch (InvalidEmailException e)
            {
                return new GuestResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.INVALID_EMAIL,
                    Message = "The given email is not valid"
                };
            }
            catch (Exception)
            {
                return new GuestResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.COULDNT_STORE_DATA,
                    Message = "There was an error when saving to DB"
                };
            }
        }

        public async Task<GuestResponse> GetGuest(int guestId)
        {
            var guest = await _guestRepository.Get(guestId);

            if (guest == null)
            {
                return new GuestResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.GUEST_NOT_FOUND,
                    Message = "No Guest record has found with the given ID"
                };
            }

            return new GuestResponse
            {
                Data = GuestDto.MapToDto(guest),
                Success = true,
            };
        }
    }
}
