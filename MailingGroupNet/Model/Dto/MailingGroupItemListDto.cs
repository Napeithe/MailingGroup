using System;
using System.Collections.Generic;
using System.Text;
using Model.Entity;

namespace Model.Dto
{
    public class MailingGroupItemListDto
    {
        public MailingGroupItemListDto()
        {
            
        }

        public MailingGroupItemListDto(MailingGroup mailingGroup)
        {
            Id = mailingGroup.Id;
            Name = mailingGroup.Name;
            NumberOfEmails = mailingGroup.Emails.Count;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfEmails { get; set; }
    }
}
