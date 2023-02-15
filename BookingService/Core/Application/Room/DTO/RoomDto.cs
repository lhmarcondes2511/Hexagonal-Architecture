using Entitie = Domain.Room.Entities;
using Domain.Guest.ValueObjects;
using Domain.Guest.Enums;

namespace Application.Room.DTO
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public bool InMaintenance { get; set; }
        public decimal Value { get; set; }
        public int Currency { get; set; }

        public static Entitie.Room MapToEntity(RoomDto roomDTO)
        {
            return new Entitie.Room
            {
                Id = roomDTO.Id,
                Name = roomDTO.Name,
                Level = roomDTO.Level,
                InMaintenance = roomDTO.InMaintenance,
                Price = new Price
                {
                    Value = roomDTO.Value,
                    Currency = (AcceptedCurrencies) roomDTO.Currency,
                }
            };
        }

        public static RoomDto MapToDto(Entitie.Room room)
        {
            return new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
                Level = room.Level,
                InMaintenance = room.InMaintenance,
                Value = room.Price.Value,
                Currency = (int)room.Price.Currency,
            };
        }
    }
}
