﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MailingGroupNet.Features.MailingGroup.Create;
using Microsoft.AspNetCore.Identity;
using Model.Database;
using Model.Dto;
using Model.Entity;
using Xunit;

namespace MailingGroupNetTest.Features.MailingGroup
{

    public class CreateTest
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
        public async Task ExecuteShouldReturnFailedNameIsAlreadyInUse()
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
            mailingGroupContext.Add(oldGroup);
            mailingGroupContext.SaveChanges();

            Command cmd = new Command
            {
                UserId = user.Id,
                Name = oldGroup.Name
            };
            //Act
            ApiResult<MailingGroupDto> result = await new Handler(mailingGroupContext).Handle(cmd, CancellationToken.None);
            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("This mailing group is already exists");
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
            mailingGroupContext.Add(user);
            mailingGroupContext.SaveChanges();

            Command cmd = new Command
            {
                UserId = user.Id,
                Name = "New group"
            };
            //Act
            ApiResult<MailingGroupDto> result = await new Handler(mailingGroupContext).Handle(cmd, CancellationToken.None);
            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().BeNullOrEmpty();

            mailingGroupContext.MailingGroups.Should().HaveCount(1);
            Model.Entity.MailingGroup mailingGroup = mailingGroupContext.MailingGroups.First();
            mailingGroup.Name.Should().Be(cmd.Name);
            mailingGroup.UserId.Should().Be(cmd.UserId);
            mailingGroup.Id.Should().Be(1);

            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(mailingGroup.Id);

        }
    }
}
