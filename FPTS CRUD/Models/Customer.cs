using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPTS_CRUD.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Fullname { get; set; }
        public string Gender { get; set; }
        public string CustomerAddress { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
    }
}