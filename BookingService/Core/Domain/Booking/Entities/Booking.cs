using EntitiesGuest = Domain.Guest.Entities;
using EntitiesRoom = Domain.Room.Entities;

using Action = Domain.Guest.Enums.Action;
using Domain.Booking.Enums;
using Domain.Booking.Ports;
using Domain.Booking.DomainExceptions;
using Domain.Guest.Ports;

namespace Domain.Booking.Entities
{
    public class Booking
    {
        public Booking()
        {
            Status = Status.Created;
            PlacedAt = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Status Status { get; set; }
        public EntitiesRoom.Room Room { get; set; }
        public EntitiesGuest.Guest Guest { get; set; }

        public void ChangeState(Action action)
        {
            Status = (Status, action) switch
            {
                (Status.Created, Action.Pay) => Status.Paid,
                (Status.Created, Action.Cancel) => Status.Canceled,
                (Status.Paid, Action.Finish) => Status.Finished,
                (Status.Paid, Action.Refound) => Status.Refunded,
                (Status.Canceled, Action.Reopen) => Status.Created,
                _ => Status
            };
        }

        public bool IsValid()
        {
            try
            {
                ValidateState();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void ValidateState() 
        {
            if(PlacedAt == default(DateTime) ||
               Start == default(DateTime) ||
               End == default(DateTime) ||
               Room == null ||
               Guest == null)
            {
                throw new PlacedAllsRequiredInformationException();
            }

            Guest.IsValid();

            if (Room.CanBeBooked())
            {
                throw new RoomCannotBeBookingException();
            }

        }

        public async Task Save(IBookingRepository bookingRepository)
        {
            this.IsValid();

            if (Id == 0)
            {
                Id = await bookingRepository.CreateBooking(this);
            }
            else
            {
                //await guestRepository.Update(this);
            }
        }
    }
}
