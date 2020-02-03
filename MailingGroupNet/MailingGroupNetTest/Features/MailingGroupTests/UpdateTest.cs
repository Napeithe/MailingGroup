using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MailingGroupNet.Features.MailingGroup.Update;
using Model.Database;
using Model.Dto;
using Model.Entity;
using Xunit;

namespace MailingGroupNetTest.Features.MailingGroupTests
{

    public class UpdateTest
    {
        [Fact]
        public async Task ExecuteShouldReturnFailedResultUserNotFound()
        {
            //Arrange
            MailingGroupContext mailingGroupContext = ContextBuilder.BuildClean();
            Command cmd = new Command
            {
                UserId = Guid.NewGuid().ToString(),
                Name = "test"
            };
            //Act
            ApiResult<MailingGroupDto> result = await new Handler(mailingGroupContext).Handle(cmd, CancellationToken.None);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("User not found");
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task ExecuteShouldReturnFailedResultNameIsRequired()
        {
            //Arrange
            MailingGroupContext mailingGroupContext = ContextBuilder.BuildClean();
            AppUser user = new AppUser
            {
                UserName = "test",
                Email = "user@user.pl"
            };
            mailingGroupContext.Add(user);
            mailingGroupContext.SaveChanges();

            Command cmd = new Command
            {
                UserId = user.Id,
                Name = string.Empty
            };
            //Act
            ApiResult<MailingGroupDto> result = await new Handler(mailingGroupContext).Handle(cmd, CancellationToken.None);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Name is required");
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task ExecuteShouldReturnNotFound()
        {
            //Arrange
            MailingGroupContext mailingGroupContext = ContextBuilder.BuildClean();
            AppUser user = new AppUser
            {
                UserName = "test",
                Email = "user@user.pl"
            };
            AppUser otherUser = new AppUser
            {
                UserName = "test",
                Email = "user@user.pl"
            };
            mailingGroupContext.Add(user);
            mailingGroupContext.Add(otherUser);
            Model.Entity.MailingGroup oldGroup = new Model.Entity.MailingGroup
            {
                Name = "Old group",
                User = user
            };
            mailingGroupContext.Add(oldGroup);
            mailingGroupContext.SaveChanges();

            Command cmd = new Command
            {
                UserId = otherUser.Id,
                Id = oldGroup.Id,
                Name = "New group"
            };
            //Act
            ApiResult<MailingGroupDto> result = await new Handler(mailingGroupContext).Handle(cmd, CancellationToken.None);
            //Assert
            result.IsSuccess.Should().BeFalse("You cannot edit group other user");
            result.Message.Should().Be("This mailing group is not exist");
            result.StatusCode.Should().Be(404);
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task ExecuteShouldReturnNameIsTaken()
        {
            //Arrange
            MailingGroupContext mailingGroupContext = ContextBuilder.BuildClean();
            AppUser user = new AppUser
            {
                UserName = "test",
                Email = "user@user.pl"
            };
            mailingGroupContext.Add(user);
            Model.Entity.MailingGroup oldGroup = new Model.Entity.MailingGroup
            {
                Name = "Old group",
                User = user
            };
            Model.Entity.MailingGroup otherGroup = new Model.Entity.MailingGroup
            {
                Name = "New group",
                User = user
            };
            mailingGroupContext.Add(oldGroup);
            mailingGroupContext.SaveChanges();
            mailingGroupContext.Add(otherGroup);
            mailingGroupContext.SaveChanges();

            Command cmd = new Command
            {
                UserId = user.Id,
                Id = oldGroup.Id,
                Name = "New group"
            };
            //Act
            ApiResult<MailingGroupDto> result = await new Handler(mailingGroupContext).Handle(cmd, CancellationToken.None);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("This mailing group name is already exists");
            result.StatusCode.Should().Be(401);
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task ExecuteShouldReturnSuccess()
        {
            //Arrange
            MailingGroupContext mailingGroupContext = ContextBuilder.BuildClean();
            AppUser user = new AppUser
            {
                UserName = "test",
                Email = "user@user.pl"
            };

            AppUser otherUser = new AppUser
            {
                UserName = "test",
                Email = "user@user.pl"
            };
            mailingGroupContext.Add(user);
            mailingGroupContext.Add(otherUser);
            Model.Entity.MailingGroup oldGroup = new Model.Entity.MailingGroup
            {
                Name = "Old group",
                User = user
            };
            Model.Entity.MailingGroup groupOtherUser = new Model.Entity.MailingGroup
            {
                Name = "New group",
                User = otherUser
            };
            mailingGroupContext.Add(oldGroup);
            mailingGroupContext.Add(groupOtherUser);
            mailingGroupContext.SaveChanges();

            Command cmd = new Command
            {
                UserId = user.Id,
                Id = oldGroup.Id,
                Name = "New group"
            };
            //Act
            ApiResult<MailingGroupDto> result = await new Handler(mailingGroupContext).Handle(cmd, CancellationToken.None);
            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().BeNullOrEmpty();

            mailingGroupContext.MailingGroups.Should().HaveCount(2);
            Model.Entity.MailingGroup mailingGroup = mailingGroupContext.MailingGroups.First();
            mailingGroup.Name.Should().Be(cmd.Name);
            mailingGroup.UserId.Should().Be(cmd.UserId);
            mailingGroup.Id.Should().Be(1);

            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(mailingGroup.Id);

        }
    }
}
