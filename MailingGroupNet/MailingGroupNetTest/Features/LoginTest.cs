using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using MailingGroupNet.Features.Authentication;
using MailingGroupNetTest.Moq;
using Microsoft.AspNetCore.Identity;
using MockQueryable.Moq;
using Model.Entity;
using Moq;
using Xunit;

namespace MailingGroupNetTest.Features
{
    public class LoginTest
    {
        [Fact]
        public async Task ExecuteShouldReturnFailureUserNotExist()
        {
            //Arrange
            Mock<UserManager<AppUser>> userManagerMoq = UserManagerMoq.Get();
            var AppUsers = new List<AppUser>().AsQueryable().BuildMock();
            userManagerMoq.SetupGet(x => x.Users).Returns(AppUsers.Object);
            AuthenticationModel registerModel = new AuthenticationModel
            {
                UserName = "das",
                Password = "12323",
            };
            //Act
            var result = await new LoginHandler(userManagerMoq.Object).Handle(registerModel, default);
            //Assert
            result.IsSuccess.Should().BeFalse("User not exist");
            result.Message.Should().NotBeNullOrEmpty().And.Be("Username or password is invalid");
            userManagerMoq.Verify(x=>x.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>()), Times.Never);
            userManagerMoq.Verify(x => x.GetClaimsAsync(It.IsAny<AppUser>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteShouldReturnFailurePasswordMismatch()
        {
            //Arrange
            Mock<UserManager<AppUser>> userManagerMoq = UserManagerMoq.Get();
            AppUser user = new AppUser("das");
            var AppUsers = new List<AppUser>()
            {
                user
            }.AsQueryable().BuildMock();
            userManagerMoq.SetupGet(x => x.Users).Returns(AppUsers.Object);
            userManagerMoq.Setup(x => x.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(false);
            AuthenticationModel registerModel = new AuthenticationModel
            {
                UserName = "das",
                Password = "12323",
            };
            //Act
            var result = await new LoginHandler(userManagerMoq.Object).Handle(registerModel, default);
            //Assert
            result.IsSuccess.Should().BeFalse("User not exist");
            result.Message.Should().NotBeNullOrEmpty().And.Be("Username or password is invalid");
            userManagerMoq.Verify(x=>x.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>()), Times.Once);
            userManagerMoq.Verify(x => x.GetClaimsAsync(user), Times.Never);
        }

        [Fact]
        public async Task ExecuteShouldReturnSuccessAndClaims()
        {
            //Arrange
            Mock<UserManager<AppUser>> userManagerMoq = UserManagerMoq.Get();
            AppUser user = new AppUser("das");
            var AppUsers = new List<AppUser>()
            {
                user
            }.AsQueryable().BuildMock();
            userManagerMoq.SetupGet(x => x.Users).Returns(AppUsers.Object);
            userManagerMoq.Setup(x => x.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            userManagerMoq.Setup(x => x.GetClaimsAsync(user)).ReturnsAsync(new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Name")
            });
            AuthenticationModel registerModel = new AuthenticationModel
            {
                UserName = "das",
                Password = "12323",
            };
            //Act
            var result = await new LoginHandler(userManagerMoq.Object).Handle(registerModel, default);
            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().BeNullOrEmpty();
            userManagerMoq.Verify(x => x.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>()), Times.Once);
            userManagerMoq.Verify(x => x.GetClaimsAsync(user), Times.Once);
        }

    }
}
