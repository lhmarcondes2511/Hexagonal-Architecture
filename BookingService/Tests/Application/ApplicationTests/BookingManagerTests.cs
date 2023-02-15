using Application.Booking;
using Application.Booking.Requests;
using Application.Payment.DTO;
using Application.Payment.Ports;
using Application.Payment.Responses;
using Domain.Booking.Ports;
using Domain.Guest.Ports;
using Domain.Room.Ports;
using Moq;

namespace ApplicationTests
{
    public class BookingManagerTests
    {
        [SetUp]
        public void Setup()
        {
            // 1 metodo para mocar
            //var fakeRepos = new FakeRepos();
            //guestManager = new GuestManager(fakeRepos);
        }

        [Test]
        public async Task ShouldPayForABooking()
        {
            var dto = new PaymentRequestDto
            {
                SelectedPaymentProvider = SupportedPaymentProviders.MercadoPago,
                SelectedPaymentMethod = SupportedPaymentMethods.CreditCard,
                PaymentIntention = "https://www.mercadopago.coim/asdf"
            };

            var bookingRepository = new Mock<IBookingRepository>();
            var roomRepository = new Mock<IRoomRepository>();
            var guestRepository = new Mock<IGuestRepository>();
            var paymentProcessorFactory = new Mock<IPaymentProcessorFactory>();
            var paymentProcessor = new Mock<IPaymentProcessor>();

            var responseDto = new PaymentStateDto
            {
                CreatedDate = DateTime.Now,
                Message = $"Successfuly paid {dto.PaymentIntention}",
                PaymentId = "123",
                Status = Status.Success
            };

            var response = new PaymentResponse
            {
                Data = responseDto,
                Success = true,
                Message = "Payment successfuly processed",
            };

            paymentProcessor.Setup(x => x.CapturePayment(dto.PaymentIntention)).Returns(Task.FromResult(response));

            paymentProcessorFactory.Setup(x => x.GetPaymentProcessor(dto.SelectedPaymentProvider)).Returns(paymentProcessor.Object);

            var bookingManager = new BookingManager(
                bookingRepository.Object, 
                guestRepository.Object, 
                roomRepository.Object,
                paymentProcessorFactory.Object);

            var res = await bookingManager.PayForABooking(dto);

            Assert.NotNull(res);
            Assert.True(res.Success);
            Assert.AreEqual(res.Message, "Payment successfuly processed");
        }
    }
}
