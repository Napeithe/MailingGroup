using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MailingGroupNet.Features.MailingGroup.GetAll;
using Model.Dto;
using Model.Entity;
using Xunit;

namespace MailingGroupNetTest.Features.MailingGroupTests
{
    public class GetAllTest
    {
        [Fact]
        public async Task ExecuteShouldReturnAllUsersMailingGroupWithEmails()
        {
            //Arrange
            var context = ContextBuilder.BuildClean();
            AppUser firstUser = new AppUser
            {
                UserName = "firstUser",
                Email = "email@first.pl"
            };
            List<MailingGroup> firstUsersGroup = GenerateGroups(5, firstUser).ToList();
            context.AddRange(firstUsersGroup);
            AppUser secondUser = new AppUser
            {
                UserName = "secondUser",
                Email = "email@second.pl"
            };
            List<MailingGroup> secondUsersGroup = GenerateGroups(4, secondUser).ToList();
            context.AddRange(secondUsersGroup);
            context.SaveChanges();
            context.MailingGroups.Should().HaveCount(9);
            context.Emails.Should().HaveCount(16);
            Query query = new Query
            {
                UserId = firstUser.Id
            };
            //Act
            ApiResult<List<MailingGroupItemListDto>> apiResult = await new Handler(context).Handle(query, CancellationToken.None);
            // Assert
            apiResult.IsSuccess.Should().BeTrue();
            apiResult.Data.Should().NotBeNull()
                .And.NotBeEmpty()
                .And.HaveCount(5);
            MailingGroupItemListDto lastElement = apiResult.Data.Last();
            lastElement.Id.Should().Be(5);
            lastElement.Name.Should().NotBeNullOrEmpty();
            lastElement.NumberOfEmails.Should().Be(4);
        }


        private IEnumerable<Model.Entity.MailingGroup> GenerateGroups(int numberOfGroup, AppUser user)
        {
            for (int i = 0; i < numberOfGroup; i++)
            {
                MailingGroup mailingGroup = new Model.Entity.MailingGroup()
                {
                    Name = $"Lista {i}",
                    User = user
                };
                mailingGroup.Emails = GenerateEmails(i, mailingGroup).ToList();
                yield return mailingGroup;
            }
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
