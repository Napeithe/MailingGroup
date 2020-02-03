using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Model.Entity
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {

        }
        public AppUser(string userName) : base(userName)
        {

        }

        public ICollection<MailingGroup> MailingGroups { get; set; }
    }
}
