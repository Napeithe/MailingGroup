﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Model.Entity
{
    public class MailingGroup : Entity
    {
        public string Name { get; set; }
        public List<Email> Emails { get; set; }

        public IdentityUser User { get; set; }
        public string UserId { get; set; }
    }
}