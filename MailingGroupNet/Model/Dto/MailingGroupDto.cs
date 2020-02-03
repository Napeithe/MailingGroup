using System.Collections.Generic;

namespace Model.Dto
{
    public class MailingGroupDto
    {
        #region constructors
        public MailingGroupDto() : this(default, default)
        {

        }

        public MailingGroupDto(int id, string name) : this(id, name, new List<EmailDto>())
        {

        }

        public MailingGroupDto(int id, string name, List<EmailDto> emails)
        {
            Id = id;
            Name = name;
            Emails = emails;
        }
        #endregion

        public int Id { get; set; }
        public string Name { get; set; }
        public List<EmailDto> Emails { get; set; }
    }
}
