using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Web.Helpers;
using static System.Data.Entity.Infrastructure.Design.Executor;
using FPTS_CRUD.Models;

namespace FPTS_CRUD.Controllers
{
    public class SystemController : Controller
    {
        string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
        // GET: System
        public ActionResult User()
        {
            DataTable table = refresh("tb_user");
            return View(table);
        }
        [HttpPost]
        public ActionResult User(string create, string search, string refresh,string delete, string update, string username, string password, string createby, string createon, string modifiedby, string modifiedon, string fullname, string userstatus, string email)
        {
            DataTable table = this.refresh("tb_user");
            if (create == "Create")
            {
                handleCreateUser(username, password, createby, createon, modifiedby, modifiedon, fullname, userstatus, email);
            }
            if (search == "Search")
            {
                return View(handleSearchUser(username));
            }
            if (refresh == "Refresh")
            {
                return View(this.refresh("tb_user"));
            }
            if(update == "Update")
            {
                handleUpdateUser(username, password, createby, createon, modifiedby, modifiedon, fullname, userstatus, email);
            }
            if(delete == "Delete")
            {
                handleDeleteUser(username);
            }
            return View(table);
        }
        public ActionResult Customer()
        {
            DataTable table = refresh("tb_customer");
            return View(table);
        }

        [HttpPost]
        public ActionResult Customer(string create, string search, string refresh, string update, string delete, string customerid, string fullname, string gender, string birthdate, string customeraddress, string email)
        {
            DataTable table = this.refresh("tb_customer");
            if (create == "Create")
            {
                handleCreateCustomer(customerid, fullname, gender, birthdate, customeraddress, email);
            }
            if (search == "Search")
            {
                return View(handleSearchCustomer(customerid));
            }
            if (refresh == "Refresh")
            {
                return View(this.refresh("tb_customer"));
            }
            if (update == "Update")
            {
                handleUpdateCustomer(customerid, fullname, gender, birthdate, customeraddress, email);
            }
            if (delete == "Delete")
            {
                handleDeleteCustomer(customerid);
            }
            return View(table);
        }
        public ActionResult Supplier()
        {
            DataTable table = refresh("tb_supplier");
            return View(table);
        }

        [HttpPost]
        public ActionResult Supplier(string create, string search, string refresh, string delete, string update, string supplierid, string suppliername, string supplieraddress)
        {
            DataTable table = this.refresh("tb_supplier");
            if (create == "Create")
            {
                handleCreateSupplier(supplierid, suppliername, supplieraddress);
            }
            if (search == "Search")
            {
                return View(handleSearchSupplier(supplierid));
            }
            if (refresh == "Refresh")
            {
                return View(this.refresh("tb_supplier"));
            }
            if (update == "Update")
            {
                handleUpdateSupplier(supplierid, suppliername, supplieraddress);
            }
            if (delete == "Delete")
            {
                handleDeleteSupplier(supplierid);
            }
            return View(table);
        }
        public ActionResult UserGroup()
        {
            DataTable table = refresh("tb_user_group");
            return View(table);
        }

        [HttpPost]
        public ActionResult UserGroup(string create, string search, string refresh,string update, string delete, string username, string groupcode, string createby, string createdate)
        {
            DataTable table = this.refresh("tb_user_group");
            if (create == "Create")
            {
                handleCreateUserGroup(username, groupcode, createby, createdate);
            }
            if (search == "Search")
            {
                return View(handleSearchUserGroup(username, groupcode));
            }
            if (refresh == "Refresh")
            {
                return View(this.refresh("tb_user_group"));
            }
            if (update == "Update")
            {
                handleUpdateUserGroup(username, groupcode, createby, createdate);
            }
            if (delete == "Delete")
            {
                handleDeleteUserGroup(username, groupcode);
            }
            return View(table);
        }

        public DataTable refresh(string name)
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
        private void handleCreateUser(string username, string password, string createby, string createon, string modifiedby, string modifiedon, string fullname, string userstatus, string email)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_insert_user";
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@createby", createby);
                    cmd.Parameters.AddWithValue("@createon", Convert.ToDateTime(createon));
                    cmd.Parameters.AddWithValue("@modifiedby", modifiedby);
                    cmd.Parameters.AddWithValue("@userstatus", userstatus);
                    cmd.Parameters.AddWithValue("@fullname", fullname);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@modifiedon", Convert.ToDateTime(modifiedon));
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }

        private void handleUpdateUser(string username, string password, string createby, string createon, string modifiedby, string modifiedon, string fullname, string userstatus, string email)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_update_user";
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@createby", createby);
                    cmd.Parameters.AddWithValue("@createon", Convert.ToDateTime(createon));
                    cmd.Parameters.AddWithValue("@modifiedby", modifiedby);
                    cmd.Parameters.AddWithValue("@userstatus", userstatus);
                    cmd.Parameters.AddWithValue("@fullname", fullname);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@modifiedon", Convert.ToDateTime(modifiedon));
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }
        private DataTable handleSearchUser(string username)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(String.Format("exec sp_search_user @username = '{0}'", username), cnn))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    dataAdapter.Fill(table);
                    return table;
                }
            }
        }

        private void handleDeleteUser(string username)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_delete_user";
                    cmd.Parameters.AddWithValue("@id", username);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }
        private void handleCreateCustomer(string customerid, string fullname, string gender, string birthdate, string customeraddress, string email)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_insert_customer";
                    cmd.Parameters.AddWithValue("@id", customerid);
                    cmd.Parameters.AddWithValue("@fullname", fullname);
                    cmd.Parameters.AddWithValue("@gender", gender);
                    cmd.Parameters.AddWithValue("@birthdate", Convert.ToDateTime(birthdate));
                    cmd.Parameters.AddWithValue("@customeraddress", customeraddress);
                    cmd.Parameters.AddWithValue("@email", email);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }

        private void handleUpdateCustomer(string customerid, string fullname, string gender, string birthdate, string customeraddress, string email)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_update_customer";
                    cmd.Parameters.AddWithValue("@id", customerid);
                    cmd.Parameters.AddWithValue("@fullname", fullname);
                    cmd.Parameters.AddWithValue("@gender", gender);
                    cmd.Parameters.AddWithValue("@birthdate", Convert.ToDateTime(birthdate));
                    cmd.Parameters.AddWithValue("@customeraddress", customeraddress);
                    cmd.Parameters.AddWithValue("@email", email);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }
        private DataTable handleSearchCustomer(string customerid)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(String.Format("exec sp_search_customer @id = '{0}'", customerid), cnn))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    dataAdapter.Fill(table);
                    return table;
                }
            }
        }

        private void handleDeleteCustomer(string customerid)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_delete_customer";
                    cmd.Parameters.AddWithValue("@id", customerid);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }
        private void handleCreateSupplier(string supplierid, string suppliername, string supplieraddress)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_insert_supplier";
                    cmd.Parameters.AddWithValue("@id", supplierid);
                    cmd.Parameters.AddWithValue("@suppliername", suppliername);
                    cmd.Parameters.AddWithValue("@supplieraddress", supplieraddress);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }
        private void handleUpdateSupplier(string supplierid, string suppliername, string supplieraddress)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_update_supplier";
                    cmd.Parameters.AddWithValue("@id", supplierid);
                    cmd.Parameters.AddWithValue("@suppliername", suppliername);
                    cmd.Parameters.AddWithValue("@supplieraddress", supplieraddress);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }
        private DataTable handleSearchSupplier(string supplierid)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(String.Format("exec sp_search_supplier @id = '{0}'", supplierid), cnn))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    dataAdapter.Fill(table);
                    return table;
                }
            }
        }

        private void handleDeleteSupplier(string supplierid)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_delete_supplier";
                    cmd.Parameters.AddWithValue("@id", supplierid);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }
        private void handleCreateUserGroup(string username, string groupcode, string createby, string createdate)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_insert_usergroup";
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@groupcode", groupcode);
                    cmd.Parameters.AddWithValue("@createby", createby);
                    cmd.Parameters.AddWithValue("@createdate", Convert.ToDateTime(createdate));
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }

        private void handleUpdateUserGroup(string username, string groupcode, string createby, string createdate)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_update_usergroup";
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@groupcode", groupcode);
                    cmd.Parameters.AddWithValue("@createby", createby);
                    cmd.Parameters.AddWithValue("@createdate", Convert.ToDateTime(createdate));
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }
        private DataTable handleSearchUserGroup(string username, string groupcode)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(String.Format("exec sp_search_usergroup @username = '{0}', @groupcode = '{1}'", username, groupcode), cnn))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    dataAdapter.Fill(table);
                    return table;
                }
            }
        }

        private void handleDeleteUserGroup(string username, string groupcode)
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_delete_usergroup";
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@groupcode", groupcode);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }
    }
}