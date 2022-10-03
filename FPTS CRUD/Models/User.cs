using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPTS_CRUD.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string UserStatus { get; set; }

    }
}