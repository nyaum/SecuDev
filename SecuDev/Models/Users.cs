using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecuDev.Models
{
    public class Users
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AuthorityLevel { get; set; }

    }
}