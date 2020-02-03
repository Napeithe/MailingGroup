using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Model.Entity;
using Moq;

namespace MailingGroupNetTest.Moq
{
    public class UserManagerMoq : UserManager<AppUser>
    {
        public UserManagerMoq()
            : base(new Mock<IUserStore<AppUser>>().Object,
        new Mock<IOptions<IdentityOptions>>().Object,
        new Mock<IPasswordHasher<AppUser>>().Object,
        new IUserValidator<AppUser>[0],
        new IPasswordValidator<AppUser>[0],
        new Mock<ILookupNormalizer>().Object,
        new Mock<IdentityErrorDescriber>().Object,
        new Mock<IServiceProvider>().Object,
        new Mock<ILogger<UserManager<AppUser>>>().Object)
        {
        }

        public static Mock<UserManager<AppUser>> Get()
        {
            var userStoreMock = new Mock<IUserStore<AppUser>>();
            return new Mock<UserManager<AppUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
        }
    }
}
