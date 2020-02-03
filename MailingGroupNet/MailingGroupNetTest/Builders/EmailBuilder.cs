using System;
using System.Collections.Generic;
using System.Text;
using Model.Database;
using Model.Entity;

namespace MailingGroupNetTest.Builders
{
    public class EmailBuilder : Builder<EmailBuilder, Email>
    {
        public EmailBuilder(MailingGroupContext context) : base(context)
        {

        }

        public EmailBuilder WithName(string emailName)
        {
            State.Name = $"{emailName}@email.com";
            return this;
        }

        public EmailBuilder WithMailingGroup(MailingGroup mailingGroup)
        {
            State.MailingGroup = mailingGroup;
            return this;
        }

        public override Email Build()
        {
            if (State.MailingGroup is null)
            {
                throw new BuilderSaveException("MailingGroup has to be set");
            }
            return base.Build();
        }
    }
}
