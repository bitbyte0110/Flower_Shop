using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Web.Services;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Assg1
{
    public partial class Cart : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Remove soon
                Session["custID"] = "1";
                //BackButton.Visible = true;
                // Get customer ID from session
                string strCustId = Session["custID"].ToString();

                // Add try catch to check if able to parse customer ID
                int custId = Int32.Parse(strCustId);

                string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

                // Use DISTINCT to avoid duplicates
                string query = $@"
               SELECT C.CartID, C.Quantity, C.Size, C.Color, C.ProductName ,
           PS.Price as UnitPrice, 
           (PS.Price * C.Quantity) as TotalPrice,
           P.ImageURL
    FROM dbo.Cart C
    JOIN dbo.Products P ON P.ProductName = C.ProductName
    JOIN dbo.ProductSizes PS ON PS.ProductID = P.ProductID AND PS.Size = C.Size
    WHERE C.CustomerId = {custId};
            ";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            CartRepeater.DataSource = reader;
                            CartRepeater.DataBind();
                        }
                        else
                        {
                            CartRepeater.DataSource = null;
                            CartRepeater.DataBind();
                            Response.Write("<script>alert('No cart item found.');</script>");
                        }
                    }
                }
            }

            if (Request.HttpMethod == "POST")
            {
                string cartId = Request.Form["cartId"];
                if (!string.IsNullOrEmpty(cartId))
                {
                    // Remove the item from the cart based on CartID
                    string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;
                    string query = "DELETE FROM dbo.Cart WHERE CartID = @CartID";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CartID", cartId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }

                    // Return a success response (optional, can be customized)
                    Response.StatusCode = 200;
                    Response.End();
                }
            }
        }
        [WebMethod]
        public static void UpdateItemCount(int cartID, int count, float price)
        {
            string query = @"
        UPDATE [dbo].[Cart]
        SET Quantity = @count,
            TotalPrice = @price
        WHERE CartID = @Id;";

            // Use the correct connection string name (e.g., "ProductsDB")
            string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("@Id", cartID);
                        cmd.Parameters.AddWithValue("@count", count);
                        cmd.Parameters.AddWithValue("@price", price);

                        // Open the connection and execute the query
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw new Exception("Error updating cart item count", ex);
            }
        }

        [WebMethod]
        public static void UpdateCheckoutItem(int cartID)
        {
            string query = @"INSERT INTO [dbo].[CheckoutCart] ([CustomerID], [ProductName], [Size], [Color], [Quantity], [TotalPrice])
                 SELECT [CustomerID], [ProductName],  [Size], [Color], [Quantity], [TotalPrice]
                 FROM  [dbo].[Cart]
                 WHERE  [CartID] = @Id;";
            string queryd = @"DELETE FROM dbo.Cart 
                            WHERE CartID = @Id";
            // Use the correct connection string name (e.g., "ProductsDB")
            string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("@Id", cartID);

                        // Open the connection and execute the query
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw new Exception("Error updating checkout cart item count", ex);
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(queryd, conn))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("@Id", cartID);

                        // Open the connection and execute the query
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw new Exception("Error delete cart item", ex);
            }

        }
        protected void BackButton_Click(object sender, EventArgs e)
        {
            //Reset search state
            Session["IsSearch"] = false;
            // Navigate back to the product listing page
            Response.Redirect("ProductListing.aspx");
        }

        protected void CheckoutButton_Click(object sender, EventArgs e)
        {

            try
            {
                // Get the selected cart items
                string deliveryDate = Request.Form["delivery-date"];
                string timeSlot = Request.Form["time-slot"];
                string personalMessage = TxtboxDA.Text;
                string senderName = TxtboxSender.Text;

                // Store in session
                Session["DeliveryDate"] = deliveryDate;
                Session["TimeSlot"] = timeSlot;
                Session["PersonalMessage"] = personalMessage;
                Session["SenderName"] = senderName;

                // Get customer ID from session
                //string strCustId = Session["custID"]?.ToString() ?? "8";

                //using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString))
                //{
                //    conn.Open();

                //    // Verify cart items exist and belong to the customer
                //    string verifyQuery = @"
                //SELECT COUNT(*) 
                //FROM Cart 
                //WHERE CustomerID = @CustomerId";

                //    using (SqlCommand cmd = new SqlCommand(verifyQuery, conn))
                //    {
                //        cmd.Parameters.AddWithValue("@CustomerId", strCustId);
                //        int itemCount = (int)cmd.ExecuteScalar();

                //        if (itemCount == 0)
                //        {
                //            // If no items found, redirect back to cart
                //            Response.Redirect("Cart.aspx");
                //            return;
                //        }
                //    }
                //}

                // Redirect to checkout
                Response.Redirect("CheckOutA.aspx");
            }
            catch (Exception ex)
            {
                // Log error and show user-friendly message
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    $"alert('Error during checkout: {ex.Message}');", true);
            }
        }

    }
}