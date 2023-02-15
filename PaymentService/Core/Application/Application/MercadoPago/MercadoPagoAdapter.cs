using Application.MercadoPago.Exceptions;
using Application.Payment;
using Application.Payment.DTO;
using Application.Payment.Ports;
using Application.Payment.Responses;
using Application.Responses;

namespace MercadoPago.Application
{
    public class MercadoPagoAdapter : IPaymentProcessor
    {
        public Task<PaymentResponse> CapturePayment(string paymentInformation)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(paymentInformation))
                {
                    throw new InvalidPaymentIntentionException();
                }

                paymentInformation = "/success";

                var dto = new PaymentStateDto
                {
                    CreatedDate = DateTime.Now,
                    Message = $"Successfuly Paid: {paymentInformation}",
                    PaymentId = "123",
                    Status = Status.Success,
                };

                var res = new PaymentResponse
                {
                    Data = dto,
                    Success = true,
                    Message = "Payment successfully processed"
                };

                return Task.FromResult(res);
            }
            catch (InvalidPaymentIntentionException)
            {
                var res = new PaymentResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.PAYMENT_INVALID_PAYMENT_INTENTION,
                    Message = "The selected payment intention is invalid"
                };
                return Task.FromResult(res);
            }
        }
    }
}
