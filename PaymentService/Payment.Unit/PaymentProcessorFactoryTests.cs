using Application.Booking.Requests;
using Application.MercadoPago.Exceptions;
using Application.Responses;
using MercadoPago.Application;
using Payments.Application;

namespace Payment.Unit
{
    public class Tests
    {
        [Test]
        public void ShouldReturn_NotImplementedPaymentProvider_WhenAskingForStripeProvider()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.Stripe);

            Assert.AreEqual(provider.GetType(), typeof(NotImplementedPaymentProvider));
        }

        [Test]
        public void ShouldReturn_MercadoPagoAdapter_Provider()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.MercadoPago);

            Assert.AreEqual(provider.GetType(), typeof(MercadoPagoAdapter));
        }

        [Test]
        public async Task ShouldReturnFalse_WhenCapturingPaymentFor_NotImplementedPaymentProvider()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.Stripe);

            var res = await provider.CapturePayment("https://myprovider.com/asdf");

            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.PAYMENT_PROVIDER_NOT_IMPLEMENTED);
            Assert.AreEqual(res.Message, "The selected payment provider is not available at the moment");
        }
    }
}