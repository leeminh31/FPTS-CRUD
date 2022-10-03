using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPTS_CRUD.Models
{
    public class InvoiceLine
    {
        public int ID { get; set; }
        public int InvoiceID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public DateTime CreateDate { get; set; }
    }
}