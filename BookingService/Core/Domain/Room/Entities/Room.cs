using Domain.Room.Ports;
using Domain.Guest.ValueObjects;
using Domain.Guest.DomainExceptions;
using Domain.Room.DomainExceptions;
using Domain.Booking.Enums;
using EntitiesBooking = Domain.Booking.Entities;

namespace Domain.Room.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public bool InMaintenance { get; set; }
        public Price Price { get; set; }
        public ICollection<EntitiesBooking.Booking> Bookings { get; set; }
        public bool IsAvailable
        {
            get
            {
                if (InMaintenance || HasGuest)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public bool HasGuest
        {
            get 
            {
                var notAvailableStatus = new List<Status>()
                {
                    Status.Created,
                    Status.Paid
                };

                return Bookings.Where(x => x.Room.Id == Id && notAvailableStatus.Contains(x.Status)).Count() > 0;
            }
        }

        private void ValidateState()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new InvalidRoomDataException();
            }

            if(Price == null || Price.Value < 10)
            {
                throw new InvalidRoomPriceException();
            }
        }

        public bool CanBeBooked()
        {
            try
            {
                ValidateState();
            }
            catch (Exception)
            {
                return false;
            }

            if (IsAvailable)
            {
                return false;
            }

            return true;
        }

        public async Task Save(IRoomRepository roomRepository)
        {
            ValidateState();
            if (Id == 0)
            {
                Id = await roomRepository.Create(this);
            }
            else
            {
                //await guestRepository.Update(this);
            }
        }
    }
}
