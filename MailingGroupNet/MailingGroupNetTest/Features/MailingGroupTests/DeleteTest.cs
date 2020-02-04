using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Model.Database;
using Model.Entity;
using MailingGroupNet.Features.MailingGroup.Delete;
using MailingGroupNetTest.Builders;
using Model.Dto;
using Xunit;

namespace MailingGroupNetTest.Features.MailingGroupTests
{
    public class DeleteTest
    {
        [Fact]
        public async Task ExecuteShouldReturnFailedWhenGroupIsForOtherUser()
        {
            //Arrange
            MailingGroupContext context = ContextBuilder.BuildClean();

            MailingGroup firstMailingGroup = new GroupBuilder(context)
                .WithName("Old group")
                .WithUser(x => x.WithName("userName"))
                .BuildAndSave();
            MailingGroup secondMailingGroup = new GroupBuilder(context)
                .WithName("Old group")
                .WithUser(x => x.WithName("otherUser"))
                .BuildAndSave();

            Command query = new Command()
            {
                Id = new List<int>{secondMailingGroup.Id},
                UserId = firstMailingGroup.UserId
            };
            //Act
            ApiResult result = await new Handler(context).Handle(query, CancellationToken.None);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Mail group not exists");
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task ExecuteShouldReturnFailedWhenGroupIdIsWrong()
        {
            //Arrange
            MailingGroupContext context = ContextBuilder.BuildClean();
            MailingGroup firstMailingGroup = new GroupBuilder(context)
                .WithName("Old group")
                .WithUser(x => x.WithName("userName"))
                .BuildAndSave();
             new GroupBuilder(context)
                .WithName("Old group")
                .WithUser(x => x.WithName("otherUser"))
                .BuildAndSave();
            Command query = new Command()
            {
                Id = new List<int> { 22 },
                UserId = firstMailingGroup.UserId
            };
            //Act
            ApiResult result = await new Handler(context).Handle(query, CancellationToken.None);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Mail group not exists");
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task ExecuteShouldReturnSuccessAndRemoveMailingGroupWithEmail()
        {
            //Arrange
            MailingGroupContext context = ContextBuilder.BuildClean();
            MailingGroup firstMailingGroup = new GroupBuilder(context)
                .WithName("Old group")
                .WithUser(x => x.WithName("userName"))
                .WithEmail("first")
                .WithEmail("second")
                .WithEmail("third")
                .BuildAndSave();
            Command query = new Command()
            {
                Id = new List<int> { 1 },
                UserId = firstMailingGroup.UserId
            };
            //Act
            ApiResult result = await new Handler(context).Handle(query, CancellationToken.None);
            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().BeNullOrEmpty();
            result.StatusCode.Should().Be(200);

            context.MailingGroups.Should().BeEmpty();
            context.Emails.Should().BeEmpty();
        }
    }
}
