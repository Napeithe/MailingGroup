using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Dto
{
    public class AuthenticateDto
    {
        public string AccessToken { get; set; }
        public int ExpireInSeconds { get; set; }

    }
}
