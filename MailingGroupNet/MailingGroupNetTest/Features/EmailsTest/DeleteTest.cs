using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MailingGroupNet.Features.Emails.Delete;
using MailingGroupNetTest.Builders;
using Model.Database;
using Model.Dto;
using Model.Entity;
using Xunit;

namespace MailingGroupNetTest.Features.EmailsTest
{
    public class DeleteTest 
    {
        [Fact]
        public async Task ExecuteShouldReturnFailedMailingGroupIsNotExist()
        {
            //Arrange 
            MailingGroupContext context = ContextBuilder.BuildClean();
            Command cmd = new Command
            {
                UserId = Guid.NewGuid().ToString(),
                GroupId = 23
            };
            //Act
            ApiResult result = await new Handler(context).Handle(cmd, CancellationToken.None);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Email is not exists");
            result.StatusCode.Should().Be(404);
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
                .BuildAndSave();

            Command cmd = new Command
            {
                UserId = mailingGroup.User.Id,
                GroupId = mailingGroup.Id,
                EmailId = mailingGroup.Emails.First().Id
            };
            //Act
            ApiResult result = await new Handler(context).Handle(cmd, CancellationToken.None);
            //Assert
            result.IsSuccess.Should().BeTrue();
            result.StatusCode.Should().Be(200);
        }
    }
}
