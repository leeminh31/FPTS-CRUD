using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPTS_CRUD.Models
{
    public class InvoiceSetup
    {
        public string ID { get; set; }
        public string TypeName { get; set; }
        public string Symbol { get; set; }
        public DateTime StartDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateOn { get; set; }
        public string InvoiceStatus { get; set; }


    }
}