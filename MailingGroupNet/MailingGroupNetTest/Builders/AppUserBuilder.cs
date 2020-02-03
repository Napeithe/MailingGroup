using System;
using System.Collections.Generic;
using System.Text;
using Model.Database;
using Model.Entity;

namespace MailingGroupNetTest.Builders
{
    public class AppUserBuilder : Builder<AppUserBuilder, AppUser>
    {
        public AppUserBuilder(MailingGroupContext context) : base(context)
        {

        }

        public AppUserBuilder WithName(string name)
        {
            State.UserName = name;
            State.Email = $"{name}@email.com";
            return this;
        }

        public override AppUser Build()
        {
            if (string.IsNullOrEmpty(State.UserName))
            {
                throw new BuilderSaveException("Username cannot be empty");
            }

            if (string.IsNullOrEmpty(State.Email))
            {
                throw new BuilderSaveException("Email cannot be empty");
            }

            return base.Build();
        }
    }
}
