using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace MailingGroupNetTest.Moq
{
    public class UserManagerMoq : UserManager<IdentityUser>
    {
        public UserManagerMoq()
            : base(new Mock<IUserStore<IdentityUser>>().Object,
        new Mock<IOptions<IdentityOptions>>().Object,
        new Mock<IPasswordHasher<IdentityUser>>().Object,
        new IUserValidator<IdentityUser>[0],
        new IPasswordValidator<IdentityUser>[0],
        new Mock<ILookupNormalizer>().Object,
        new Mock<IdentityErrorDescriber>().Object,
        new Mock<IServiceProvider>().Object,
        new Mock<ILogger<UserManager<IdentityUser>>>().Object)
        {
        }

        public static Mock<UserManager<IdentityUser>> Get()
        {
            var userStoreMock = new Mock<IUserStore<IdentityUser>>();
            return new Mock<UserManager<IdentityUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
        }
    }
}
