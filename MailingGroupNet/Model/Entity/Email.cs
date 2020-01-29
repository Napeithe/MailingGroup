using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Entity
{
    public class Email : Entity
    {
        public string Name { get; set; }
        public int MailingGroupId { get; set; }
    }
}
