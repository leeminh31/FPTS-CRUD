using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using FPTS_CRUD.Models;
using System.Diagnostics;
using System.Globalization;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace FPTS_CRUD.Controllers
{
    public class InvoiceController : Controller
    {
        string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
        // GET: Invoice
        public DataTable refresh (string name)
        {
            DataTable table = new DataTable();
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                cnn.Open();
                string query = "select * from " + name;
                SqlDataAdapter adapter = new SqlDataAdapter(query, cnn);
                adapter.Fill(table);
            }
            return table;
        }
        public ActionResult Invoice()
        {
            DataTable table = refresh("tb_invoice");
            return View(table);
        }
        [HttpPost]
        public ActionResult Invoice(string create, string search, string refresh,string update, string delete, string customerid, string supplierid, string invoiceid, string invoicesetupid, string startdate)
        {
            DataTable table = this.refresh("tb_invoice");
            if (create == "Create")
            {
                handleCreateInvoice(invoiceid, customerid, supplierid, invoicesetupid, startdate);
            }
            if (search == "Search")
            {
                return View(handleSearch(invoiceid));
            }
            if(refresh == "Refresh")
            {
                return View(this.refresh("tb_invoice"));
            }
            if(update == "Update")
            {
                handleUpdateInvoice(customerid, supplierid, invoiceid, invoicesetupid, startdate);
            }
            if (delete == "Delete")
            {
                handleDeleteInvoice(invoiceid);
            }
            return View(table);
        }

        public ActionResult InvoiceLine()
        {
            DataTable table = refresh("tb_invoice_line");
            return View(table);
        }
        [HttpPost]
        public ActionResult InvoiceLine(string create, string search, string refresh, string delete, string update, string invoicelineid, string productname, string invoiceid, string quantity, string createdate, string price)
        {
            DataTable table = this.refresh("tb_invoice_line");
            if (create == "Create")
            {
                handleCreateInvoiceLine(invoicelineid, productname, invoiceid, quantity, createdate, price);
            }
            if (search == "Search")
            {
                return View(handleSearchInvoiceLine(invoicelineid));
            }
            if (refresh == "Refresh")
            {
                return View(this.refresh("tb_invoice_line"));
            }
            if (update == "Update")
            {
                handleUpdateInvoiceLine(invoicelineid, productname, invoiceid, quantity, createdate, price);
            }
            if (delete == "Delete")
            {
                handleDeleteInvoiceLine(invoicelineid);
            }
            return View(table);
        }

        public ActionResult InvoiceSetup()
        {
            DataTable table = refresh("tb_invoice_setup");
            return View(table);
        }
        [HttpPost]
        public ActionResult InvoiceSetup(string create, string search, string refresh, string update, string delete, string invoicesetupid, string typename, string symbol, string startdate, string createby, string invoicestatus, string createdon)
        {
            DataTable table = this.refresh("tb_invoice_setup");
            if (create == "Create")
            {
                handleCreateInvoiceSetup(invoicesetupid, typename, symbol, startdate, createby, invoicestatus, createdon);
            }
            if (search == "Search")
            {
                return View(handleSearchInvoiceSetup(invoicesetupid));
            }
            if (refresh == "Refresh")
            {
                return View(this.refresh("tb_invoice_setup"));
            }
            if (update == "Update")
            {
                handleUpdateInvoiceSetup(invoicesetupid, typename, symbol, startdate, createby, invoicestatus, createdon);
            }
            if (delete == "Delete")
            {
                handleDeleteInvoiceSetup(invoicesetupid);
            }
            return View(table);
        }

        private void handleCreateInvoice(string invoiceid, string customerid, string supplierid, string invoicesetupid, string startdate)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_insert_invoice";
                    cmd.Parameters.AddWithValue("@id", invoiceid);
                    cmd.Parameters.AddWithValue("@supplierid", supplierid);
                    cmd.Parameters.AddWithValue("@customerid", customerid);
                    cmd.Parameters.AddWithValue("@invoicesetupid", invoicesetupid);
                    cmd.Parameters.AddWithValue("@startdate", Convert.ToDateTime(startdate));
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }
        private void handleCreateInvoiceLine(string invoicelineid, string invoiceid, string productname, string quantity, string price, string createdate)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_insert_invoice_line";
                    cmd.Parameters.AddWithValue("@id", invoiceid);
                    cmd.Parameters.AddWithValue("@invoiceid", invoicelineid);
                    cmd.Parameters.AddWithValue("@productname", productname);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@createdate", Convert.ToDateTime(createdate));
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }

        private void handleCreateInvoiceSetup(string invoicesetupid,string typename,string symbol,string startdate,string createby,string invoicestatus,string createdon)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_insert_invoicesetup";
                    cmd.Parameters.AddWithValue("@id", invoicesetupid);
                    cmd.Parameters.AddWithValue("@typename", typename);
                    cmd.Parameters.AddWithValue("@symbol", symbol);
                    cmd.Parameters.AddWithValue("@createby", createby);
                    cmd.Parameters.AddWithValue("@invoicestatus", invoicestatus);
                    cmd.Parameters.AddWithValue("@createon", Convert.ToDateTime(createdon));
                    cmd.Parameters.AddWithValue("@startdate", Convert.ToDateTime(startdate));
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }

        private void handleUpdateInvoice(string customerid, string supplierid, string invoiceid, string invoicesetupid, string startdate)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_update_invoice";
                    cmd.Parameters.AddWithValue("@id", invoiceid);
                    cmd.Parameters.AddWithValue("@supplierid", supplierid);
                    cmd.Parameters.AddWithValue("@customerid", customerid);
                    cmd.Parameters.AddWithValue("@invoicesetupid", invoicesetupid);
                    cmd.Parameters.AddWithValue("@startdate", Convert.ToDateTime(startdate));
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }

        private void handleUpdateInvoiceLine(string invoicelineid, string productname, string invoiceid, string quantity, string createdate, string price)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_update_invoice_line";
                    cmd.Parameters.AddWithValue("@id", invoicelineid);
                    cmd.Parameters.AddWithValue("@invoiceid", invoiceid);
                    cmd.Parameters.AddWithValue("@productname", productname);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@createdate", Convert.ToDateTime(createdate));
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }

        private void handleUpdateInvoiceSetup(string invoicesetupid, string typename, string symbol, string startdate, string createby, string invoicestatus, string createdon)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_update_invoicesetup";
                    cmd.Parameters.AddWithValue("@id", invoicesetupid);
                    cmd.Parameters.AddWithValue("@typename", typename);
                    cmd.Parameters.AddWithValue("@symbol", symbol);
                    cmd.Parameters.AddWithValue("@createby", createby);
                    cmd.Parameters.AddWithValue("@invoicestatus", invoicestatus);
                    cmd.Parameters.AddWithValue("@createon", Convert.ToDateTime(createdon));
                    cmd.Parameters.AddWithValue("@startdate", Convert.ToDateTime(startdate));
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }

        private void handleDeleteInvoice(string invoiceid)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_delete_invoice";
                    cmd.Parameters.AddWithValue("@id", invoiceid);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }

        private void handleDeleteInvoiceLine(string invoicelineid)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_delete_invoiceline";
                    cmd.Parameters.AddWithValue("@id", invoicelineid);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }

        private void handleDeleteInvoiceSetup(string invoicesetupid)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_delete_invoicesetup";
                    cmd.Parameters.AddWithValue("@id", invoicesetupid);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }

        private DataTable handleSearch(string invoiceid)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(String.Format("exec sp_search_invoice @id = '{0}'", invoiceid), cnn))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    dataAdapter.Fill(table);
                    return table;
                }
            }
        }

        private DataTable handleSearchInvoiceLine(string invoicelineid)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(String.Format("exec sp_search_invoiceline @id = '{0}'", invoicelineid), cnn))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    dataAdapter.Fill(table);
                    return table ;
                }
            }
        }

        private DataTable handleSearchInvoiceSetup(string invoicesetupid)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(String.Format("exec sp_search_invoicesetup @id = '{0}'", invoicesetupid), cnn))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    dataAdapter.Fill(table);
                    return table;
                }
            }
        }
    }
}