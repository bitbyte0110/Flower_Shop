using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Assg1
{
    public partial class AdminOrderDetails : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string orderId = Request.QueryString["OrderID"];
                if (!string.IsNullOrEmpty(orderId))
                {
                    LoadOrderDetails(orderId);
                }
            }
        }

        private void LoadOrderDetails(string orderId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Corrected query to fetch order information
                    string orderQuery = @"
                        SELECT 
                            o.OrderID, 
                            o.CustomerID, 
                            CONVERT(VARCHAR, o.OrderDate, 103) AS OrderDate, 
                            o.Status
                        FROM Orders o
                        WHERE o.OrderID = @OrderID";

                    using (SqlCommand cmd = new SqlCommand(orderQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@OrderID", orderId);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblOrderID.Text = reader["OrderID"].ToString();
                                lblCustomerID.Text = reader["CustomerID"].ToString();
                                lblOrderDate.Text = reader["OrderDate"].ToString();
                                
                            }
                        }
                    }

                    // Corrected query to fetch order items
                    string itemsQuery = @"
                        SELECT 
                            oi.OrderItemID, 
                            oi.ProductID, 
                            p.ProductName, 
                            oi.Price, 
                            oi.Quantity, 
                            (oi.Price * oi.Quantity) AS Subtotal
                        FROM OrderItems oi
                        INNER JOIN Products p ON oi.ProductID = p.ProductID
                        WHERE oi.OrderID = @OrderID";

                    using (SqlCommand cmd = new SqlCommand(itemsQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@OrderID", orderId);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        gvOrderItems.DataSource = dt;
                        gvOrderItems.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error: {ex.Message}');</script>");
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            // Navigate back to the order list page
            Response.Redirect("AdminOrderList.aspx");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Update the order status
                    string updateQuery = @"
                UPDATE Orders
                SET Status = @Status
                WHERE OrderID = @OrderID";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Status", ddlOrderStatus.SelectedValue);
                        cmd.Parameters.AddWithValue("@OrderID", lblOrderID.Text);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Response.Write("<script>alert('Order status updated successfully!');</script>");
                            LoadOrderDetails(lblOrderID.Text); // Reload details to reflect the changes
                        }
                        else
                        {
                            Response.Write("<script>alert('Failed to update order status.');</script>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error: {ex.Message}');</script>");
            }
        }
    }
}
