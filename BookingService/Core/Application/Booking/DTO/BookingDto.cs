using Domain.Booking.Enums;
using EntitiesBooking = Domain.Booking.Entities;
using EntitiesGuest = Domain.Guest.Entities;
using EntitiesRoom = Domain.Room.Entities;

namespace Application.Booking.DTO
{
    public class BookingDto
    {
        public BookingDto()
        {
            this.PlacedAt = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        //public Status Status { get; set; }
        public int RoomId { get; set; }
        public int GuestId { get; set; }

        public static EntitiesBooking.Booking MapToEntity(BookingDto bookingDto)
        {
            return new EntitiesBooking.Booking
            {
                Id = bookingDto.Id,
                PlacedAt = bookingDto.PlacedAt,
                Start = bookingDto.Start,
                End = bookingDto.End,
                Guest = new EntitiesGuest.Guest
                {
                    Id = bookingDto.GuestId
                },
                Room = new EntitiesRoom.Room { 
                    Id = bookingDto.RoomId 
                },
            };
        }

        public static BookingDto MapToDto(EntitiesBooking.Booking booking)
        {
            return new BookingDto
            {
                Id = booking.Id,
                PlacedAt = booking.PlacedAt,
                Start = booking.Start,
                End = booking.End,
                GuestId = booking.Guest.Id,
                RoomId = booking.Room.Id,
                //Status = booking.Status

            };
        }
    }
}
