using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;

namespace Assg1
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        public class CartItem
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public string Size { get; set; }
            public string Color { get; set; } 
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UpdateCartDetails(); // Update cart details on initial load
            }

            //string customerId = Session["CustomerId"]?.ToString();
            string customerId = "8";

            SqlConnection con;
            string strCon = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

            con = new SqlConnection(strCon);
            con.Open();

            string strRetrieve = "SELECT * FROM CUSTOMER WHERE CustomerId = @CustomerId";

            SqlCommand cmdR;
            cmdR = new SqlCommand(strRetrieve, con);
            cmdR.Parameters.AddWithValue("@CustomerId", customerId);

            SqlDataReader dtrCustomer = cmdR.ExecuteReader();

            string firstName, lastName, name;

            if (dtrCustomer.Read())
            {
                firstName = dtrCustomer["FirstName"].ToString();
                lastName = dtrCustomer["LastName"].ToString();
                name = firstName + " " + lastName;
                lblName.Text = name;
            }
        }

        public void AddToCart(int productId, string productName, decimal price, string size, string color, int quantity)
        {
            try
            {
                // Get customer ID from session, default to 8 for testing
                string strCustId = Session["custID"]?.ToString() ?? "8";
                int customerId = Int32.Parse(strCustId);

                string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if item already exists in cart
                    string checkQuery = @"
                SELECT CartID, Quantity 
                FROM Cart 
                WHERE CustomerID = @CustomerId 
                AND ProductName = @ProductName 
                AND Size = @Size 
                AND Color = @Color";

                    using (var checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@CustomerId", customerId);
                        checkCmd.Parameters.AddWithValue("@ProductName", productName);
                        checkCmd.Parameters.AddWithValue("@Size", size);
                        checkCmd.Parameters.AddWithValue("@Color", color);

                        using (var reader = checkCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Update existing cart item
                                reader.Close();
                                string updateQuery = @"
                            UPDATE Cart 
                            SET Quantity = Quantity + @Quantity,
                                TotalPrice = (Quantity + @Quantity) * @Price
                            WHERE CustomerID = @CustomerId 
                            AND ProductName = @ProductName 
                            AND Size = @Size 
                            AND Color = @Color";

                                using (var updateCmd = new SqlCommand(updateQuery, conn))
                                {
                                    updateCmd.Parameters.AddWithValue("@CustomerId", customerId);
                                    updateCmd.Parameters.AddWithValue("@ProductName", productName);
                                    updateCmd.Parameters.AddWithValue("@Size", size);
                                    updateCmd.Parameters.AddWithValue("@Color", color);
                                    updateCmd.Parameters.AddWithValue("@Quantity", quantity);
                                    updateCmd.Parameters.AddWithValue("@Price", price);
                                    updateCmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                // Insert new cart item
                                reader.Close();
                                string insertQuery = @"
                            INSERT INTO Cart 
                            (CustomerID, ProductName, Size, Color, Quantity, TotalPrice, DateAdded)
                            VALUES 
                            (@CustomerId, @ProductName, @Size, @Color, @Quantity, @TotalPrice, @DateAdded)";

                                using (var insertCmd = new SqlCommand(insertQuery, conn))
                                {
                                    insertCmd.Parameters.AddWithValue("@CustomerId", customerId);
                                    insertCmd.Parameters.AddWithValue("@ProductName", productName);
                                    insertCmd.Parameters.AddWithValue("@Size", size);
                                    insertCmd.Parameters.AddWithValue("@Color", color);
                                    insertCmd.Parameters.AddWithValue("@Quantity", quantity);
                                    insertCmd.Parameters.AddWithValue("@TotalPrice", price * quantity);
                                    insertCmd.Parameters.AddWithValue("@DateAdded", DateTime.Now);
                                    insertCmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
                UpdateCartDetails();
                Response.Write("<script>alert('Item added to cart successfully!');</script>");
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error adding to cart: {ex.Message}');</script>");
            }
        }
        private void InsertCartItemIntoDatabase(int customerId, string productName, int quantity, string size, string color, decimal price)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;
                Response.Write($"<script>console.log('Connection string: {connectionString}');</script>");

                using (var conn = new SqlConnection(connectionString))
                {
                    // Get the correct unit price from ProductSizes table
                    string priceQuery = @"
               SELECT PS.Price 
               FROM Products P 
               JOIN ProductSizes PS ON P.ProductID = PS.ProductID 
               WHERE P.ProductName = @ProductName AND PS.Size = @Size";

                    decimal unitPrice;
                    using (var priceCmd = new SqlCommand(priceQuery, conn))
                    {
                        priceCmd.Parameters.AddWithValue("@ProductName", productName);
                        priceCmd.Parameters.AddWithValue("@Size", size);

                        conn.Open();
                        var result = priceCmd.ExecuteScalar();
                        if (result == null)
                        {
                            throw new Exception("Product price not found");
                        }
                        unitPrice = (decimal)result;
                    }

                    decimal totalPrice = unitPrice * quantity;

                    string insertQuery = @"
               INSERT INTO Cart 
               (CustomerID, ProductName, Size, Color, Quantity, TotalPrice, DateAdded)  
               VALUES (@CustomerId, @ProductName, @Size, @Color, @Quantity, @TotalPrice, @DateAdded)";

                    using (var cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", customerId);
                        cmd.Parameters.AddWithValue("@ProductName", productName);
                        cmd.Parameters.AddWithValue("@Size", size);
                        cmd.Parameters.AddWithValue("@Color", color);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.Parameters.AddWithValue("@TotalPrice", totalPrice);
                        cmd.Parameters.AddWithValue("@DateAdded", DateTime.Now);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            Response.Write("<script>alert('No rows were affected. Check constraints or data duplication.');</script>");
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Response.Write($"<script>alert('SQL Error: {sqlEx.Message}. Code: {sqlEx.ErrorCode}');</script>");
                Response.Write($"<script>console.log('StackTrace: {sqlEx.StackTrace}');</script>");
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('General Error: {ex.Message}');</script>");
                Response.Write($"<script>console.log('StackTrace: {ex.StackTrace}');</script>");
            }
        }
        public void UpdateCartDetails()
        {
            try
            {
                string strCustId = Session["custID"]?.ToString() ?? "8";
                int customerId = Int32.Parse(strCustId);

                string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;
                using (var conn = new SqlConnection(connectionString))
                {
                    string query = @"
                SELECT SUM(Quantity) as TotalItems, 
                       SUM(TotalPrice) as TotalAmount
                FROM Cart 
                WHERE CustomerID = @CustomerId";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", customerId);
                        conn.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int totalItems = reader["TotalItems"] != DBNull.Value ? Convert.ToInt32(reader["TotalItems"]) : 0;
                                decimal totalAmount = reader["TotalAmount"] != DBNull.Value ? Convert.ToDecimal(reader["TotalAmount"]) : 0;

                                CartCountLabel.InnerHtml = totalItems.ToString();
                                CartTotalLabel.InnerHtml = $"RM {totalAmount:F2}";
                            }
                            else
                            {
                                CartCountLabel.InnerHtml = "0";
                                CartTotalLabel.InnerHtml = "RM 0.00";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write($"<script>console.log('Error updating cart: {ex.Message}');</script>");
                CartCountLabel.InnerHtml = "0";
                CartTotalLabel.InnerHtml = "RM 0.00";
            }
        }

    }
}