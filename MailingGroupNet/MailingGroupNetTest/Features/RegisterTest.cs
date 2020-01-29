using System.Threading.Tasks;
using FluentAssertions;
using MailingGroupNet.Features.Authentication;
using MailingGroupNetTest.Moq;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace MailingGroupNetTest.Features
{
    public class RegisterTest
    {
        [Fact]
        public async Task ExecuteShouldReturnFailurePasswordMismatch()
        {
            //Arrange
            Mock<UserManager<IdentityUser>> userManagerMoq = UserManagerMoq.Get();
            RegisterModel registerModel = new RegisterModel
            {
                Email = "dsa",
                UserName = "das",
                Password = "123",
                RePassword = "1234"
            };
            //Act
            var result = await new RegisterHandler(userManagerMoq.Object).Handle(registerModel, default);
            //Assert
            result.IsSuccess.Should().BeFalse("Passwords are invalid");
            result.Message.Should().NotBeNullOrEmpty();
            userManagerMoq.Verify(x=>x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteShouldReturnFailureUserManagerFailure()
        {
            //Arrange
            Mock<UserManager<IdentityUser>> userManagerMoq = UserManagerMoq.Get();
            var message = "Username is taken";
            userManagerMoq.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(new[]
            {
                new IdentityError()
                {
                    Description = message
                }
            }));
            RegisterModel registerModel = new RegisterModel
            {
                Email = "dsa",
                UserName = "das",
                Password = "123",
                RePassword = "123"
            };
            //Act
            var result = await new RegisterHandler(userManagerMoq.Object).Handle(registerModel, default);
            //Assert
            result.IsSuccess.Should().BeFalse("User is taken");
            result.Message.Should().NotBeNullOrEmpty().And.Be(message);

            userManagerMoq.Verify(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteShouldReturnSuccess()
        {
            //Arrange
            Mock<UserManager<IdentityUser>> userManagerMoq = UserManagerMoq.Get();
            userManagerMoq.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            RegisterModel registerModel = new RegisterModel
            {
                Email = "dsa",
                UserName = "das",
                Password = "123",
                RePassword = "123"
            };
            //Act
            var result = await new RegisterHandler(userManagerMoq.Object).Handle(registerModel, default);
            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().BeNullOrEmpty();

            userManagerMoq.Verify(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);
        }
    }
}
