using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Cashier_App.Models;
using OrderNumberGenerator;
using iTextSharp.text;
using MvcRazorToPdf;

namespace Cashier_App.Controllers
{
    public class TransactionController : Controller
    {
        string connectionString = @"Data Source = VITTO; Initial Catalog = CashierDB; Integrated Security = True";
        [HttpGet]
        // GET: Transaction
        public ActionResult Index()
        {
            DataTable dtblProduct = new DataTable();
            DataTable best_seller = new DataTable();
            DataTable warning_stock = new DataTable();
            DataTable total_transaction = new DataTable();
            DataTable total_product = new DataTable();
            DataTable todays_transaction = new DataTable();
            DataTable todays_income = new DataTable();
            DataTable all_income = new DataTable();
            DataTable total_stock = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("select TransactionTable.TransactionID, TransactionTable.TransactionNumber, ProductTable.ProductName, ProductTable.Price, TransactionTable.Quantity, TransactionTable.Total from TransactionTable INNER JOIN ProductTable ON TransactionTable.ProductID = ProductTable.ProductID", sqlCon);
                sqlDa.Fill(dtblProduct);
            }

            using (SqlConnection sqlCon2 = new SqlConnection(connectionString))
            {
                sqlCon2.Open();
                SqlDataAdapter sqlDa2 = new SqlDataAdapter("SELECT Best_Seller_Product.ProductID, Best_Seller_Product.Produk_Terlaris, ProductTable.ProductName FROM Best_Seller_Product INNER JOIN ProductTable ON ProductTable.ProductID = Best_Seller_Product.ProductID", sqlCon2);
                sqlDa2.Fill(best_seller);
            }

            using (SqlConnection sqlCon3 = new SqlConnection(connectionString))
            {
                sqlCon3.Open();
                SqlDataAdapter sqlDa3 = new SqlDataAdapter("SELECT * FROM ProductTable WHERE ProductTable.Stock <= 10", sqlCon3);
                sqlDa3.Fill(warning_stock);
            }

            using (SqlConnection sqlCon4 = new SqlConnection(connectionString))
            {
                sqlCon4.Open();
                SqlDataAdapter sqlDa4 = new SqlDataAdapter("SELECT COUNT(TransactionTable.ProductID) AS Total_Transaction FROM TransactionTable ", sqlCon4);
                sqlDa4.Fill(total_transaction);
            }

            using (SqlConnection sqlCon5 = new SqlConnection(connectionString))
            {
                sqlCon5.Open();
                SqlDataAdapter sqlDa5 = new SqlDataAdapter("SELECT COUNT(ProductTable.ProductName) AS TotalProduct FROM ProductTable", sqlCon5);
                sqlDa5.Fill(total_product);
            }

            using (SqlConnection sqlCon6 = new SqlConnection(connectionString))
            {
                sqlCon6.Open();
                SqlDataAdapter sqlDa6 = new SqlDataAdapter("SELECT COUNT(DateOfTransaction.TransactionID) AS TransactionToday FROM DateOfTransaction", sqlCon6);
                sqlDa6.Fill(todays_transaction);
            }

            using (SqlConnection sqlCon7 = new SqlConnection(connectionString))
            {
                sqlCon7.Open();
                SqlDataAdapter sqlDa7 = new SqlDataAdapter("SELECT SUM(DateOfTransaction.Total) AS todays_income FROM DateOfTransaction", sqlCon7);
                sqlDa7.Fill(todays_income);
            }

            using (SqlConnection sqlCon8 = new SqlConnection(connectionString))
            {
                sqlCon8.Open();
                SqlDataAdapter sqlDa8 = new SqlDataAdapter("SELECT SUM(TransactionTable.Total) AS all_income FROM TransactionTable", sqlCon8);
                sqlDa8.Fill(all_income);
            }

            using (SqlConnection sqlCon9 = new SqlConnection(connectionString))
            {
                sqlCon9.Open();
                SqlDataAdapter sqlDa9 = new SqlDataAdapter("SELECT SUM(Stock) AS TotalStock FROM ProductTable", sqlCon9);
                sqlDa9.Fill(total_stock);
            }

            var Table = new IndexModelView();
            Table.Table1 = dtblProduct;
            Table.Table2 = best_seller;
            Table.Table3 = warning_stock;
            Table.Table4 = total_transaction;
            Table.Table5 = total_product;
            Table.Table6 = todays_transaction;
            Table.Table7 = todays_income;
            Table.Table8 = all_income;
            Table.Table9 = total_stock;

            return View(Table);
        }

       
        // GET: Transaction/Details/5
        public ActionResult Details(int id)
        {
            DataTable dtblDetailTransaksi = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "SELECT TransactionTable.TransactionID, TransactionTable.TransactionNumber, ProductTable.ProductName, ProductTable.Price, TransactionTable.Quantity, TransactionTable.Total from TransactionTable INNER JOIN ProductTable ON TransactionTable.ProductID = ProductTable.ProductID WHERE TransactionTable.TransactionID  = @TransactionID";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@TransactionID", id);
                sqlDa.Fill(dtblDetailTransaksi);
            }
            return View(dtblDetailTransaksi);
        }

        [HttpGet]
        // GET: Transaction/Create
        public ActionResult Create()
        {
            return View(new TransactionModel());
        }


        // POST: Transaction/Create
        [HttpPost]
        public ActionResult Create(int id, TransactionModel transactionModel)
        {
            DataTable dtblDataProduct = new DataTable();
            using (SqlConnection sqlcon2 = new SqlConnection(connectionString))
            {
                sqlcon2.Open();
                string query = "SELECT * FROM ProductTable WHERE ProductTable.ProductID = @ProductID";
                SqlDataAdapter sqlDa2 = new SqlDataAdapter(query, sqlcon2);
                sqlDa2.SelectCommand.Parameters.AddWithValue("@ProductID", id);
                sqlDa2.Fill(dtblDataProduct);
            }

            NumberGenerator generator = new NumberGenerator();
            string number = generator.GenerateNumber("TSNBR"); 
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                var hasil = transactionModel.Quantity * (int)dtblDataProduct.Rows[0]["Price"];
                var stok_barang = Convert.ToInt32(dtblDataProduct.Rows[0]["Stock"]) - transactionModel.Quantity;
                sqlcon.Open();
                string query = "INSERT INTO TransactionTable VALUES(@ProductID, @Quantity, @TransactionNumber, @AccountID, @Total, CURRENT_TIMESTAMP)";
                string query2 = "UPDATE ProductTable SET Stock = @Stock WHERE ProductID = @ProductID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                SqlCommand sqlCmd2 = new SqlCommand(query2, sqlcon);
                sqlCmd.Parameters.AddWithValue("@ProductID", dtblDataProduct.Rows[0]["ProductID"]);
                sqlCmd2.Parameters.AddWithValue("@ProductID", dtblDataProduct.Rows[0]["ProductID"]);
                sqlCmd.Parameters.AddWithValue("@Quantity", transactionModel.Quantity);
                sqlCmd.Parameters.AddWithValue("@TransactionNumber", number);
                sqlCmd.Parameters.AddWithValue("@AccountID", Convert.ToInt32(Session["id"]));
                sqlCmd.Parameters.AddWithValue("@Total", hasil);
                sqlCmd2.Parameters.AddWithValue("@Stock", stok_barang);
                 
                sqlCmd.ExecuteNonQuery();
                sqlCmd2.ExecuteNonQuery();
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Transaction/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Transaction/Edit/5
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

        // GET: Transaction/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Transaction/Delete/5
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
        public ActionResult Invoice(int id)
        {
            DataTable dtblInvoice = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "select TransactionTable.TransactionNumber, ProductTable.ProductName, ProductTable.Price, TransactionTable.Quantity, TransactionTable.Total from TransactionTable INNER JOIN ProductTable ON TransactionTable.ProductID = ProductTable.ProductID WHERE TransactionTable.TransactionID  = @TransactionID";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@TransactionID", id);
                sqlDa.Fill(dtblInvoice);
            }
            return View(dtblInvoice);
        }
    }
}
