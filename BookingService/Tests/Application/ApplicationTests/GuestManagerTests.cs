using Application.Guest;
using Application.Guest.DTO;
using Application.Guest.Requests;
using Application.Responses;
using Domain.Guest.Entities;
using Domain.Guest.Enums;
using Domain.Guest.Ports;
using Moq;

namespace ApplicationTests
{
    // Um método de Mocar a injeção de dependencia
    //class FakeRepos : IGuestRepository
    //{
    //    public Task<int> Create(Guest guest)
    //    {
    //        return Task.FromResult(111);
    //    }

    //    public Task<Guest> Get(int id)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public class GuestManagertests
    {
        GuestManager guestManager;
        [SetUp]
        public void Setup()
        {
            // 1 metodo para mocar
            //var fakeRepos = new FakeRepos();
            //guestManager = new GuestManager(fakeRepos);
        }

        [Test]
        public async Task HappyPath()
        {
            var guestDto = new GuestDto
            {
                Name = "Test",
                Subname = "Test",
                Email = "Test",
                IdNumber = "1234",
                IdTypeCode = 1
            };

            var request = new CreateGuestRequest
            {
                Data = guestDto
            };

            // 2 metodo para mocar
            var fakeRepos = new Mock<IGuestRepository>();
            fakeRepos.Setup(x => x.Create(It.IsAny<Guest>())).Returns(Task.FromResult(222));
            guestManager = new GuestManager(fakeRepos.Object);

            var res = await guestManager.CreateGuest(request);

            Assert.IsNotNull(res);
            Assert.IsTrue(res.Success);
            Assert.Equals(res.Data.Id, 222);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("a")]
        [TestCase("ab")]
        [TestCase("abc")]
        public async Task ShouldReturnInvalidPersonDocumentIdException(string docNumber)
        {
            var guestDto = new GuestDto
            {
                Name = "Test",
                Subname = "Test",
                Email = "Test",
                IdNumber = docNumber,
                IdTypeCode = 1
            };

            var request = new CreateGuestRequest
            {
                Data = guestDto
            };

            // 2 metodo para mocar
            var fakeRepos = new Mock<IGuestRepository>();
            fakeRepos.Setup(x => x.Create(It.IsAny<Guest>())).Returns(Task.FromResult(222));
            guestManager = new GuestManager(fakeRepos.Object);

            var res = await guestManager.CreateGuest(request);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.INVALID_DOCUMENT_ID);
            Assert.AreEqual(res.Message, "This ID passed is not valid");
        }

        [TestCase("", "", "")]
        [TestCase("", "subname", "emailcorreto@gmail.com")]
        [TestCase(null, "subname", "emailcorreto@gmail.com")]
        [TestCase("name", "", "emailcorreto@gmail.com")]
        [TestCase("name", null, "emailcorreto@gmail.com")]
        [TestCase("name", "subname", "")]
        [TestCase("name", "subname", null)]
        public async Task ShouldReturnMissingRequiredInformation(string name, string subname, string email)
        {
            var guestDto = new GuestDto
            {
                Name = name,
                Subname = subname,
                Email = email,
                IdNumber = "1234",
                IdTypeCode = 1
            };

            var request = new CreateGuestRequest
            {
                Data = guestDto
            };

            // 2 metodo para mocar
            var fakeRepos = new Mock<IGuestRepository>();
            fakeRepos.Setup(x => x.Create(It.IsAny<Guest>())).Returns(Task.FromResult(222));
            guestManager = new GuestManager(fakeRepos.Object);

            var res = await guestManager.CreateGuest(request);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.MISSING_REQUIRED_INFORMATION);
            Assert.AreEqual(res.Message, "Missing required information passed");
        }

        [TestCase("b8b.com")]
        public async Task ShouldReturnInvalidEmailException(string email)
        {
            var guestDto = new GuestDto
            {
                Name = "Name",
                Subname = "Subname",
                Email = email,
                IdNumber = "1234",
                IdTypeCode = 1
            };

            var request = new CreateGuestRequest
            {
                Data = guestDto
            };

            // 2 metodo para mocar
            var fakeRepos = new Mock<IGuestRepository>();
            fakeRepos.Setup(x => x.Create(It.IsAny<Guest>())).Returns(Task.FromResult(222));
            guestManager = new GuestManager(fakeRepos.Object);

            var res = await guestManager.CreateGuest(request);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.INVALID_EMAIL);
            Assert.AreEqual(res.Message, "The given email is not valid");
        }

        [Test]
        public async Task ShouldReturnGuestNotFound()
        {
            var fakeRepos = new Mock<IGuestRepository>();

            fakeRepos.Setup(x => x.Get(333)).Returns(Task.FromResult<Guest?>(null));

            guestManager = new GuestManager(fakeRepos.Object);

            var res = await guestManager.GetGuest(333);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.GUEST_NOT_FOUND);
            Assert.AreEqual(res.Message, "No Guest record has found with the given ID");
        }

        [Test]
        public async Task ShouldReturnGuestSuccess()
        {
            var fakeRepos = new Mock<IGuestRepository>();

            var fakeGuest = new Guest
            {
                Id = 333,
                Name = "Name",
                Subname = "SubName",
                Email = "teste@gmail.com",
                DocumentId = new Domain.Guest.ValueObjects.PersonId
                {
                    IdNumber = "1234",
                    DocumentType = DocumentType.DriveLicense
                }
            };

            fakeRepos.Setup(x => x.Get(333)).Returns(Task.FromResult((Guest?)fakeGuest));

            guestManager = new GuestManager(fakeRepos.Object);

            var res = await guestManager.GetGuest(333);

            Assert.IsNotNull(res);
            Assert.True(res.Success);

        }
    }
}