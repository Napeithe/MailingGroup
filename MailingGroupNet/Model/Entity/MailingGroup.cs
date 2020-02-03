using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Model.Entity
{
    public class MailingGroup : Entity
    {
        public MailingGroup()
        {
            Emails = new List<Email>();
        }
        public string Name { get; set; }
        public List<Email> Emails { get; set; }

        public AppUser User { get; set; }
        public string UserId { get; set; }
    }
}
