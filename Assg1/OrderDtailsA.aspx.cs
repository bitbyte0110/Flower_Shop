using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace flower
{
    public partial class OrderDtailsA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int orderId = 2; // Retrieve OrderID dynamically

                if (orderId > 0)
                {
                    // Update progress tracker
                    UpdateProgressTracker(orderId);

                    // Load order details
                    LoadOrderDetails(orderId);

                    // Load delivery details
                    LoadDeliveryDetails(orderId);
                }
            }
        }

        private int GetOrderId()
        {
            if (Session["OrderID"] != null)
            {
                return Convert.ToInt32(Session["OrderID"]);
            }
            else if (Request.QueryString["OrderID"] != null)
            {
                return Convert.ToInt32(Request.QueryString["OrderID"]);
            }
            return 0;
        }

        private void UpdateProgressTracker(int orderId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;
            string query = "SELECT Status FROM Orders WHERE OrderID = @OrderID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@OrderID", orderId);

                try
                {
                    conn.Open();
                    string orderStatus = cmd.ExecuteScalar()?.ToString();

                    ResetTracker();

                    if (!string.IsNullOrEmpty(orderStatus))
                    {
                        ActivateStepsAndLines(orderStatus);
                    }
                }
                catch (Exception ex)
                {
                    Response.Write($"Error (UpdateProgressTracker): {ex.Message}");
                }
            }
        }

        private void LoadOrderDetails(int orderId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

            // Updated query to join Product and ProductSize tables and fetch OrderID
            string query = @"
        SELECT 
            p.ProductName,
            ps.Size AS ProductSize,
            p.Color AS ProductColor,
            oi.Quantity,
            oi.Price,
            p.ImageURL,
            o.OrderID
        FROM OrderItems oi
        INNER JOIN Products p ON oi.ProductID = p.ProductID
        INNER JOIN ProductSizes ps ON oi.ProductSizeID = ps.ProductSizeID
        INNER JOIN Orders o ON oi.OrderID = o.OrderID
        WHERE oi.OrderID = @OrderID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@OrderID", orderId);

                try
                {
                    conn.Open();

                    // Load data into a DataTable
                    SqlDataReader dr = cmd.ExecuteReader();
                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt.Load(dr);

                    // Bind to the repeater
                    rptOrderItems.DataSource = dt;
                    rptOrderItems.DataBind();

                    // Display the OrderID
                    if (dt.Rows.Count > 0)
                    {
                        lblOrderID.Text = dt.Rows[0]["OrderID"].ToString();
                    }

                    // Calculate the subtotal
                    decimal subTotal = 0.0m;

                    foreach (System.Data.DataRow row in dt.Rows)
                    {
                        int quantity = Convert.ToInt32(row["Quantity"]);
                        decimal unitPrice = Convert.ToDecimal(row["Price"]);
                        subTotal += quantity * unitPrice;
                    }

                    // Fixed shipping fee
                    decimal shippingFee = 5.00m;
                    decimal grandTotal = subTotal + shippingFee;

                    // Update labels
                    lblSubTotal.Text = "RM" + subTotal.ToString("F2");
                    lblShippingFee.Text = "RM" + shippingFee.ToString("F2");
                    lblTotal.Text = "RM" + grandTotal.ToString("F2");
                }
                catch (Exception ex)
                {
                    Response.Write($"Error (LoadOrderDetails): {ex.Message}");
                }
            }
        }


        private void LoadDeliveryDetails(int orderId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

            string query = @"
            SELECT 
                dd.FirstName, 
                dd.LastName, 
                dd.PhoneNumber, 
                dd.Address, 
                dd.Address2, 
                dd.City, 
                dd.State, 
                dd.PostalCode, 
                dd.Country
            FROM DeliveryDetails dd
            WHERE dd.OrderID = @OrderID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@OrderID", orderId);

                try
                {
                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lblRecipient.Text = $"{dr["FirstName"]} {dr["LastName"]}";
                            lblPhone.Text = dr["PhoneNumber"].ToString();
                            lblAddress.Text = $"{dr["Address"]}, {dr["Address2"]}, {dr["City"]}, {dr["State"]}, {dr["PostalCode"]}, {dr["Country"]}";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write($"Error (LoadDeliveryDetails): {ex.Message}");
                }
            }
        }

        private void ResetTracker()
        {
            step1.Attributes["class"] = "tracker-step";
            step2.Attributes["class"] = "tracker-step";
            step3.Attributes["class"] = "tracker-step";
            step4.Attributes["class"] = "tracker-step";
            step5.Attributes["class"] = "tracker-step";

            line1.Attributes["class"] = "tracker-line";
            line2.Attributes["class"] = "tracker-line";
            line3.Attributes["class"] = "tracker-line";
            line4.Attributes["class"] = "tracker-line";
        }

        private void ActivateStepsAndLines(string status)
        {
            switch (status)
            {
                case "Order Placed":
                    step1.Attributes["class"] += " completed";
                    break;

                case "To Ship":
                    step1.Attributes["class"] += " completed";
                    line1.Attributes["class"] += " completed";
                    step2.Attributes["class"] += " completed";
                    break;

                case "Order Shipped Out":
                    step1.Attributes["class"] += " completed";
                    line1.Attributes["class"] += " completed";
                    step2.Attributes["class"] += " completed";
                    line2.Attributes["class"] += " completed";
                    step3.Attributes["class"] += " completed";
                    break;

                case "Completed":
                    step1.Attributes["class"] += " completed";
                    line1.Attributes["class"] += " completed";
                    step2.Attributes["class"] += " completed";
                    line2.Attributes["class"] += " completed";
                    step3.Attributes["class"] += " completed";
                    line3.Attributes["class"] += " completed";
                    step4.Attributes["class"] += " completed";
                    break;

                case "Rated":
                    step1.Attributes["class"] += " completed";
                    line1.Attributes["class"] += " completed";
                    step2.Attributes["class"] += " completed";
                    line2.Attributes["class"] += " completed";
                    step3.Attributes["class"] += " completed";
                    line3.Attributes["class"] += " completed";
                    step4.Attributes["class"] += " completed";
                    line4.Attributes["class"] += " completed";
                    step5.Attributes["class"] += " completed";
                    break;

                default:
                    break;
            }
        }
    }
}