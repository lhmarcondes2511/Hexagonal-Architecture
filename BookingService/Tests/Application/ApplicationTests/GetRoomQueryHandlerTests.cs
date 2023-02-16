using Application.Responses;
using Application.Room.Queries;
using Domain.Room.Entities;
using Domain.Room.Ports;
using Moq;

namespace ApplicationTests
{
    public class GetRoomQueryHandlerTests
    {
        [Test]
        public async Task Should_Return_Room()
        {
            var query = new GetRoomQuery { RoomId = 1 };

            var repoMock = new Mock<IRoomRepository>();
            var fakeRoom = new Room() { Id = 1 };
            repoMock.Setup(x => x.Get(query.RoomId)).Returns(Task.FromResult(fakeRoom));

            var handler = new GetRoomQueryHandler(repoMock.Object);
            var res = await handler.Handle(query, CancellationToken.None);

            Assert.True(res.Success);
            Assert.NotNull(res.Data);
        }

        [Test]
        public async Task Should_Return_ProperErrorMessage_WhenRoom_NotFound()
        {
            var query = new GetRoomQuery { RoomId = 1 };
            var repoMock = new Mock<IRoomRepository>();

            var handler = new GetRoomQueryHandler(repoMock.Object);
            var res = await handler.Handle(query, CancellationToken.None);

            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.ROOM_NOT_FOUND);
            Assert.AreEqual(res.Message, "No Room record has found with the given ID");
        }
    }
}
