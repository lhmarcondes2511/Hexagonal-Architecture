namespace Application.Responses
{
    public enum ErrorCodes
    {
        // Guests Related codes 1 - 99
        NOT_FOUND = 1,
        COULDNT_STORE_DATA = 2,
        INVALID_DOCUMENT_ID = 3,
        MISSING_REQUIRED_INFORMATION = 4,
        INVALID_EMAIL = 5,
        GUEST_NOT_FOUND = 6,

        // Room Related codes 100 - 199
        ROOM_NOT_FOUND = 100,
        ROOM_COULDNT_STORE_DATA = 101,
        ROOM_INVALID_PERSON_ID = 102,
        ROOM_MISSING_REQUIRED_INFORMATION = 103,
        ROOM_INVALID_EMAIL = 104,
        RROM_CANNOT_BOOKING = 105,

        // Booking Related codes 200 - 299
        BOOKING_NOT_FOUND = 200,
        BOOKING_COULDNT_STORE_DATA = 201,
        BOOKING_MISSING_REQUIRED_INFORMATION = 202,
        BOOKING_ROOM_CANNOT_BE_BOOKED = 203,



        // Payment ErrorCodes 500 - 1000
        PAYMENT_INVALID_PAYMENT_INTENTION = 500,
        PAYMENT_PROVIDER_NOT_IMPLEMENTED = 501
    }

    public abstract class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ErrorCodes ErrorCode { get; set; }
    }
}
