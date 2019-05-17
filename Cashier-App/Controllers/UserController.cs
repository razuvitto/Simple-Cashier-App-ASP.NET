using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Cashier_App.Models;
using EncryptDecrypt;

namespace Cashier_App.Controllers
{
    public class UserController : Controller
    {
        string connectionString = @"Data Source = VITTO; Initial Catalog = CashierDB; Integrated Security = True";
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View (new UserModel());
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(UserModel userModel)
        {
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "INSERT INTO AccountTable (Username, Password, RoleID, RegisterTime)"+
                               "VALUES(@Username, HASHBYTES('SHA2_512',@Password), @RoleID, CURRENT_TIMESTAMP)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@Username", userModel.Username);
                sqlCmd.Parameters.AddWithValue("@Password", userModel.Password);
                sqlCmd.Parameters.AddWithValue("@RoleID", 2);
                sqlCmd.ExecuteNonQuery();
            }
            return Redirect("/");
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserModel userModel)
        {
            DataTable dttblLogin = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "SELECT * FROM AccountTable WHERE username = @username AND password = HASHBYTES('SHA2_512',@Password)";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@username", userModel.Username);
                sqlDa.SelectCommand.Parameters.AddWithValue("@password", userModel.Password);
                sqlDa.Fill(dttblLogin);
            }
            if (dttblLogin.Rows.Count == 1 )
            {
                Session["id"] = Convert.ToInt32(dttblLogin.Rows[0]["AccountID"].ToString());
                Session["username"] = (dttblLogin.Rows[0]["Username"].ToString());
                Session["role"] = Convert.ToInt32(dttblLogin.Rows[0]["RoleID"].ToString());
                if (Convert.ToInt32(Session["role"]) == 1)
                {
                    return RedirectToAction("Index", "Transaction");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
                    
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove("id");
            Session.Remove("username");
            Session.Remove("role");
            return RedirectToAction("Login");
        }
    }
}
