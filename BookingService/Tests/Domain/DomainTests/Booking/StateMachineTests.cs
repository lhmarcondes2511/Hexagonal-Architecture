using Domain.Booking.Entities;
using Domain.Booking.Enums;
using Action = Domain.Guest.Enums.Action;


namespace DomainTests.Bookings
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShouldAlwaysStartWithCreatedStatus()
        {
            var booking = new Booking();

            Assert.AreEqual(booking.Status, Status.Created);
        }
        
        [Test]
        public void ShouldSetStatusToPaidWhenPayingForABookingWithCreatedStatus()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Pay);

            Assert.AreEqual(booking.Status, Status.Paid);
        }

        [Test]
        public void ShouldSetStatusToCancelWhenCancellingForABookingWithCreatedStatus()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Cancel);

            Assert.AreEqual(booking.Status, Status.Canceled);
        }

        [Test]
        public void ShouldSetStatusToRefoundWhenRefoundingForAPaidBooking()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Pay);
            booking.ChangeState(Action.Refound);

            Assert.AreEqual(booking.Status, Status.Refunded);
        }

        [Test]
        public void ShouldSetStatusToCreatedWhenReopeningACancelBooking()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Cancel);
            booking.ChangeState(Action.Reopen);

            Assert.AreEqual(booking.Status, Status.Created);
        }
    }
}