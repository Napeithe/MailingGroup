using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MailingGroupNet.Infrastructure
{
    public class EmailValidator
    {
        public static bool ValidateEmail(string emailAddress)
        {
            return new EmailAddressAttribute().IsValid(emailAddress);
        }
    }
}
