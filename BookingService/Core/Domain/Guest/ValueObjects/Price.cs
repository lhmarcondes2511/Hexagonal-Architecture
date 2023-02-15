using Domain.Guest.Enums;

namespace Domain.Guest.ValueObjects
{
    public class Price
    {
        public decimal Value { get; set; }
        public AcceptedCurrencies Currency { get; set; }
    }
}
