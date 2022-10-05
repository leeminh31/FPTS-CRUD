using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FPTS_CRUD.Models;
using static FPTS_CRUD.Models.SendEmail;
using System.Text;
using DocumentFormat.OpenXml;

namespace FPTS_CRUD.Controllers
{
    public class SendEmailController : Controller
    {
        // GET: SendEmail
        string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
        public ActionResult Index()
        {
            EmailList emailList = fetchData();
            return View(emailList);
        }
        [HttpPost]
        public ActionResult Index(EmailList list, string search, string invoicecode, string fullname, string customeraddress, string email, string productname, string quantity, string price)
        {
            List<string> Emails = new List<string>();
            if(search == null)
            {
                EmailList emailList = fetchData();
                for (int i=0; i< list.ListEmails.Count; i++)
                {
                    if (list.ListEmails[i].IsChecked)
                    {
                        Emails.Add(Request["emailvalue" + i]);
                    }
                }
                handleSendMail(Emails, "Test SMTP Email", "<p>There is nothing to focus here !</p>");
                return View(emailList);
            } else
            {
                DataTable table = SearchCustomer(invoicecode, fullname, customeraddress, email, productname, quantity, price);
                List<SendEmail> sendEmails = new List<SendEmail>();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    SendEmail item = new SendEmail();
                    item.Fullname = table.Rows[i]["Fullname"].ToString();
                    item.Email = table.Rows[i]["Email"].ToString();
                    item.InvoiceCode = table.Rows[i]["InvoiceCode"].ToString();
                    item.CustomerAddress = table.Rows[i]["CustomerAddress"].ToString();
                    item.Quantity = Convert.ToInt32(table.Rows[i]["Quantity"]);
                    item.ProductName = table.Rows[i]["ProductName"].ToString();
                    item.Price = Convert.ToInt32(table.Rows[i]["Price"]);
                    item.IsChecked = false;
                    sendEmails.Add(item);
                }
                EmailList emails = new EmailList();
                emails.ListEmails = sendEmails;
                return View(emails);
            }
        }

        public ActionResult Location ()
        {
            return View();
        }

        private DataTable SearchCustomer (string invoicecode, string fullname, string customeraddress, string email, string productname, string quantity, string price)
        {
            string query = String.Format("select * from send_email where InvoiceCode = '{0}'", invoicecode);
            if (!String.IsNullOrEmpty(fullname))
            {
                query += String.Format(" and Fullname LIKE N'%{0}%'", fullname);
            }
            if (!String.IsNullOrEmpty(customeraddress))
            {
                query += String.Format(" and CustomerAddress LIKE N'%{0}%'", customeraddress);
            }
            if (!String.IsNullOrEmpty(email))
            {
                query += String.Format(" and Email LIKE '%{0}%'", email);
            }
            if (!String.IsNullOrEmpty(productname))
            {
                query += String.Format(" and ProductName LIKE N'%{0}%'", productname);
            }
            if (!String.IsNullOrEmpty(quantity))
            {
                query += String.Format(" and Quantity = {0}", quantity);
            }
            if (!String.IsNullOrEmpty(price))
            {
                query += String.Format(" and Price = {0}", price);
            }
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
            dataAdapter.Fill(dt);
            return dt;
        }
        public bool handleSendMail (List<string> toEmail, string subject, string body)
        {
            try
            {
                string email = System.Configuration.ConfigurationManager.AppSettings["SenderEmail"].ToString();
                string password = System.Configuration.ConfigurationManager.AppSettings["SenderPassword"].ToString();

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.Timeout = 100000;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(email, password);

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(email);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                mail.BodyEncoding = UTF8Encoding.UTF8;

                foreach(string s in toEmail)
                {
                    mail.To.Add(new MailAddress(s));
                }
                smtpClient.Send(mail);
                ViewBag.Message = "Successful !!!";
                return true;
            } catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return false;
            }
        }

        private EmailList fetchData()
        {
            SqlConnection cnn = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("sp_select_send_mail", cnn);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable tbl = new DataTable();
            da.Fill(tbl);
            List<SendEmail> sendEmails = new List<SendEmail>();
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                SendEmail item = new SendEmail();
                item.Fullname = tbl.Rows[i]["Fullname"].ToString();
                item.Email = tbl.Rows[i]["Email"].ToString();
                item.InvoiceCode = tbl.Rows[i]["InvoiceCode"].ToString();
                item.CustomerAddress = tbl.Rows[i]["CustomerAddress"].ToString();
                item.Quantity = Convert.ToInt32(tbl.Rows[i]["Quantity"]);
                item.ProductName = tbl.Rows[i]["ProductName"].ToString();
                item.Price = Convert.ToInt32(tbl.Rows[i]["Price"]);
                item.IsChecked = false;
                sendEmails.Add(item);
            }
            EmailList emails = new EmailList();
            emails.ListEmails = sendEmails;
            return emails;
        }
    }
}