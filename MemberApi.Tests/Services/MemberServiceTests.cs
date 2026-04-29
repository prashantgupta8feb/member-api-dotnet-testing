using MemberApi.Interfaces;
using MemberApi.Models;
using MemberApi.Services;
using Moq;
using Xunit;

namespace MemberApi.Tests.Services
{
    public class MemberServiceTests
    {
        [Fact]
        public async Task GetAllMembersAsync_ReturnsMemberList()
        {
            // Arrange
            var mockRepo = new Mock<IMembers>();

            var testData = new List<Member>
            {
                new Member { MemberId = 1, FirstName = "Test" }
            };

            mockRepo.Setup(x => x.GetAllMembers())
                    .Returns(testData);

            var service = new MemberService(mockRepo.Object);

            // Act
            var result = await service.GetAllMembersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1, result[0].MemberId);
        }

        //member NOT found
        [Fact]
        public async Task GetMemberByIdAsync_WhenNotFound_ThrowsException()
        {
            // Arrange
            var mockRepo = new Mock<IMembers>();

            mockRepo.Setup(x => x.GetMember(It.IsAny<int>()))
                    .Returns((Member)null);

            var service = new MemberService(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.GetMemberByIdAsync(10));
        }

        //Empty list
        [Fact]
        public async Task GetAllMembersAsync_ReturnsEmptyList()
        {
            var mockRepo = new Mock<IMembers>();
            mockRepo.Setup(x => x.GetAllMembers())
                    .Returns(new List<Member>());

            var service = new MemberService(mockRepo.Object);

            var result = await service.GetAllMembersAsync();

            Assert.Empty(result);
        }

        //Verify repository call
        [Fact]
        public async Task GetAllMembersAsync_CallsRepositoryOnce()
        {
            var mockRepo = new Mock<IMembers>();

            mockRepo.Setup(x => x.GetAllMembers())
                    .Returns(new List<Member>());

            var service = new MemberService(mockRepo.Object);

            await service.GetAllMembersAsync();

            mockRepo.Verify(x => x.GetAllMembers(), Times.Once);
        }

        //Valid member fetch
        [Fact]
        public async Task GetMemberByIdAsync_ReturnsMember_WhenExists()
        {
            var mockRepo = new Mock<IMembers>();

            mockRepo.Setup(x => x.GetMember(1))
                    .Returns(new Member { MemberId = 1 });

            var service = new MemberService(mockRepo.Object);

            var result = await service.GetMemberByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.MemberId);
        }
    }
}