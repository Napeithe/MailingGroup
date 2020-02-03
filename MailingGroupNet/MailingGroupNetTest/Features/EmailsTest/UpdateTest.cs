using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MailingGroupNet.Features.Emails.Update;
using MailingGroupNetTest.Builders;
using Model.Database;
using Model.Dto;
using Model.Entity;
using Xunit;

namespace MailingGroupNetTest.Features.EmailsTest
{
    public class UpdateTest 
    {
        [Fact]
        public async Task ExecuteShouldReturnFailedMailingGroupIsNotExist()
        {
            //Arrange 
            MailingGroupContext context = ContextBuilder.BuildClean();
            Command cmd = new Command
            {
                UserId = Guid.NewGuid().ToString(),
                Email = "test@das.pl",
                GroupId = 23
            };
            //Act
            ApiResult<EmailDto> result = await new Handler(context).Handle(cmd, CancellationToken.None);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Mailing group is not exists");
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task ExecuteShouldReturnFailedEmailIsNotValidFormat()
        {
            //Arrange 
            MailingGroupContext context = ContextBuilder.BuildClean();

            MailingGroup mailingGroup = new GroupBuilder(context)
                .WithName("New group")
                .WithUser(x=>x.WithName("userName"))
                .BuildAndSave();

            Command cmd = new Command
            {
                UserId = mailingGroup.User.Id,
                Email = "test",
                GroupId = mailingGroup.Id
            };
            //Act
            ApiResult<EmailDto> result = await new Handler(context).Handle(cmd, CancellationToken.None);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Email is wrong format");
            result.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task ExecuteShouldReturnFailedEmailIsNotExists()
        {
            //Arrange 
            MailingGroupContext context = ContextBuilder.BuildClean();

            MailingGroup mailingGroup = new GroupBuilder(context)
                .WithName("New group")
                .WithUser(x=>x.WithName("userName"))
                .WithEmail("first")
                .BuildAndSave();

            Command cmd = new Command
            {
                UserId = mailingGroup.User.Id,
                Email = "first@email.com",
                GroupId = mailingGroup.Id,
                EmailId = 787
            };
            //Act
            ApiResult<EmailDto> result = await new Handler(context).Handle(cmd, CancellationToken.None);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Email is not exists");
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task ExecuteShouldReturnFailedEmailIsAlreadyExists()
        {
            //Arrange 
            MailingGroupContext context = ContextBuilder.BuildClean();

            MailingGroup mailingGroup = new GroupBuilder(context)
                .WithName("New group")
                .WithUser(x=>x.WithName("userName"))
                .WithEmail("first")
                .WithEmail("second")
                .BuildAndSave();

            Command cmd = new Command
            {
                UserId = mailingGroup.User.Id,
                Email = "second@email.com",
                GroupId = mailingGroup.Id,
                EmailId = mailingGroup.Emails.First().Id
            };
            //Act
            ApiResult<EmailDto> result = await new Handler(context).Handle(cmd, CancellationToken.None);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("New email is already exists in this group");
            result.StatusCode.Should().Be(409);
        }

        [Fact]
        public async Task ExecuteShouldReturnSuccess()
        {
            //Arrange 
            MailingGroupContext context = ContextBuilder.BuildClean();

            MailingGroup mailingGroup = new GroupBuilder(context)
                .WithName("New group")
                .WithUser(x=>x.WithName("userName"))
                .WithEmail("first")
                .WithEmail("second")
                .BuildAndSave();

            Command cmd = new Command
            {
                UserId = mailingGroup.User.Id,
                Email = "third@email.com",
                GroupId = mailingGroup.Id,
                EmailId = mailingGroup.Emails.First().Id
            };
            //Act
            ApiResult<EmailDto> result = await new Handler(context).Handle(cmd, CancellationToken.None);
            //Assert
            result.IsSuccess.Should().BeTrue();
            result.StatusCode.Should().Be(200);
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(1);
        }
    }
}
