using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Model.Database;
using Model.Entity;
using MailingGroupNet.Features.MailingGroup.Delete;
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
            AppUser firstUser = new AppUser()
            {
                UserName = "first",
                Email = "first@email.com"
            };
            AppUser secondUser = new AppUser()
            {
                UserName = "second",
                Email = "second@email.com"
            };
            context.Add(firstUser);
            context.Add(secondUser);

            List<MailingGroup> mailingGroups = new List<MailingGroup>
            {
                new MailingGroup()
                {
                    Name = "first",
                    User = firstUser
                },
                new MailingGroup()
                {
                    Name = "second",
                    User = secondUser
                }
            };
            context.AddRange(mailingGroups);
            context.SaveChanges();
            Command query = new Command()
            {
                Id = 2,
                UserId = firstUser.Id
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
            AppUser firstUser = new AppUser()
            {
                UserName = "first",
                Email = "first@email.com"
            };
            AppUser secondUser = new AppUser()
            {
                UserName = "second",
                Email = "second@email.com"
            };
            context.Add(firstUser);
            context.Add(secondUser);

            List<MailingGroup> mailingGroups = new List<MailingGroup>
            {
                new MailingGroup()
                {
                    Name = "first",
                    User = firstUser
                },
                new MailingGroup()
                {
                    Name = "second",
                    User = secondUser
                }
            };
            context.AddRange(mailingGroups);
            context.SaveChanges();
            Command query = new Command()
            {
                Id = 22,
                UserId = firstUser.Id
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
            AppUser firstUser = new AppUser()
            {
                UserName = "first",
                Email = "first@email.com"
            };
            context.Add(firstUser);

            MailingGroup group = new MailingGroup()
            {
                Name = "first",
                User = firstUser,
            };
            group.Emails = GenerateEmails(20, group).ToList();
            context.Add(group);
            context.SaveChanges();
            Command query = new Command()
            {
                Id = 1,
                UserId = firstUser.Id
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

        private IEnumerable<Email> GenerateEmails(int numberOfEmails, MailingGroup mailingGroup)
        {
            for (int i = 0; i < numberOfEmails; i++)
            {
                yield return new Email()
                {
                    Name = $"Email@{i}.pl",
                    MailingGroup = mailingGroup
                };
            }
        }
    }
}
