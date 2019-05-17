using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Cashier_App.Models;

namespace Cashier_App.Controllers
{
    public class ProductController : Controller
    {
        string connectionString = @"Data Source = VITTO; Initial Catalog = CashierDB; Integrated Security = True";
        [HttpGet]
        public ActionResult Index()
        {
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM ProductTable", sqlCon);
                sqlDa.Fill(dtblProduct);
            }
            return View(dtblProduct);
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            DataTable dtblDetailProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "SELECT * FROM ProductTable WHERE ProductTable.ProductID= @ProductID";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@ProductID", id);
                sqlDa.Fill(dtblDetailProduct);
            }
            return View(dtblDetailProduct);
        }

        [HttpGet]
        // GET: Product/Create
        public ActionResult Create()
        {
            return View(new ProductModel());
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(ProductModel productModel)
        {
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "INSERT INTO ProductTable VALUES(@ProductName, @Price, @Stock)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@ProductName", productModel.ProductName);
                sqlCmd.Parameters.AddWithValue("@Price", productModel.Price);
                sqlCmd.Parameters.AddWithValue("@Stock", productModel.Stock);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            ProductModel productModel = new ProductModel();
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "SELECT * FROM ProductTable WHERE ProductID = @ProductID";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@ProductID", id);
                sqlDa.Fill(dtblProduct);
            }
            if (dtblProduct.Rows.Count == 1)
            {
                productModel.ProductID = Convert.ToInt32(dtblProduct.Rows[0][0].ToString());
                productModel.ProductName = dtblProduct.Rows[0][1].ToString();
                productModel.Price = Convert.ToInt32(dtblProduct.Rows[0][2].ToString());
                productModel.Stock = Convert.ToInt32(dtblProduct.Rows[0][3].ToString());
                return View(productModel);
            }
            else
                return RedirectToAction("Index");
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(ProductModel productModel)
        {
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "UPDATE ProductTable SET  ProductName = @ProductName, Price = @Price, Stock = @Stock WHERE ProductID = @ProductID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@ProductID", productModel.ProductID);
                sqlCmd.Parameters.AddWithValue("@ProductName", productModel.ProductName);
                sqlCmd.Parameters.AddWithValue("@Price", productModel.Price);
                sqlCmd.Parameters.AddWithValue("@Stock", productModel.Stock);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "DELETE FROM ProductTable WHERE ProductID = @ProductID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@ProductID", id);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

    }
}
