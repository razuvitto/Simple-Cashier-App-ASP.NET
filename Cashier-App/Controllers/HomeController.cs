using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Cashier_App.Models;
using OrderNumberGenerator;

namespace Cashier_App.Controllers
{
    public class HomeController : Controller
    {
        string connectionString = @"Data Source = VITTO; Initial Catalog = CashierDB; Integrated Security = True";
        [HttpGet]
        public ActionResult Index()
        {
            DataTable tblProduct = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM ProductTable", sqlCon);
                sqlDa.Fill(tblProduct);
            }
            if (Session["id"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                return View(tblProduct);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        // GET: Product/Create
        public ActionResult Create()
        {
            return View(new ProductModel());
        }


        [HttpGet]
        // GET: Home/Details/{id}
        public ActionResult Details()
        {
            return View(new TransactionModel());
        }

        // GET: Home/Details/5
        public ActionResult Details(int id, TransactionModel transactionModel)
        {
            DataTable dtblDetailProd= new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "SELECT * FROM ProductTable WHERE ProductTable.ProductID= @ProductID";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@ProductID", id);
                sqlDa.Fill(dtblDetailProd);
            }
            return View(dtblDetailProd);
        }

    }
}