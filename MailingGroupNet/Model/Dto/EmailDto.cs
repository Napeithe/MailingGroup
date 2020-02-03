using System;
using System.Collections.Generic;
using System.Text;
using Model.Entity;

namespace Model.Dto
{
    public class EmailDto
    {
        public EmailDto()
        {

        }

        public EmailDto(Email email)
        {
            Id = email.Id;
            Name = email.Name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
