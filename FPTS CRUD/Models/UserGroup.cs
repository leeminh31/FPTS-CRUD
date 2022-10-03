using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPTS_CRUD.Models
{
    public class UserGroup
    {
        public string Username { get; set; }
        public string GroupCode { get; set; }
        public string CreateBy { get; set; }

        public DateTime CreateDate { get; set; }
    }
}