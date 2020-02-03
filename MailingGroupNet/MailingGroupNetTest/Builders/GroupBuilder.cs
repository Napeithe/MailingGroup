using System;
using System.Collections.Generic;
using System.Text;
using Model.Database;
using Model.Entity;

namespace MailingGroupNetTest.Builders
{
    public class GroupBuilder : Builder<GroupBuilder, MailingGroup>
    {
        public GroupBuilder(MailingGroupContext context) : base(context)
        {

        }

        public GroupBuilder WithName(string name)
        {
            State.Name = name;
            return this;
        }

        public GroupBuilder WithEmail(string email)
        {
            Email emailInstance = new EmailBuilder(Context)
                .WithName(email)
                .WithMailingGroup(State).Build();
            State.Emails.Add(emailInstance);
            return this;
        }

        public GroupBuilder WithUser(Action<AppUserBuilder> builder)
        {
            AppUserBuilder userBuilder = new AppUserBuilder(Context);
            builder.Invoke(userBuilder);
            AppUser appUser = userBuilder.Build();
            State.User = appUser;
            return this;
        }
    }
}
