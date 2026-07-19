using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace Assg1
{
    public partial class AdminOrderList1 : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadOrderList();
            }
        }

        protected void LoadOrderList(string orderId = "", string status = "All")
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                SELECT 
                    o.OrderID,
                    o.CustomerID,
                    CONVERT(VARCHAR, o.OrderDate, 103) AS OrderDate,
                    o.Status,
                    ISNULL(oi.TotalAmount, 0) AS TotalAmount, -- Updated to calculate price from OrderItems
                    ISNULL(oi.TotalItems, 0) AS TotalItems,
                    o.ShippingID AS ShippingAddress
                FROM Orders o
                LEFT JOIN (
                    SELECT 
                        OrderID,
                        SUM(Quantity) AS TotalItems,
                        SUM(Price * Quantity) AS TotalAmount -- Correct price calculation
                    FROM OrderItems
                    GROUP BY OrderID
                ) oi ON o.OrderID = oi.OrderID
                WHERE 
                    (@OrderID = '' OR o.OrderID LIKE '%' + @OrderID + '%') AND
                    (@Status = 'All' OR o.Status = @Status)
                ORDER BY o.OrderDate DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@OrderID", orderId);
                        cmd.Parameters.AddWithValue("@Status", status);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        OrderGridView.DataSource = dt;
                        OrderGridView.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error: {ex.Message}');</script>");
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            string orderId = txtOrderID.Text.Trim();
            string status = ddlStatus.SelectedValue;
            LoadOrderList(orderId, status);
        }

        protected void OrderGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewOrder")
            {
                // Get the OrderID from the CommandArgument
                string orderId = e.CommandArgument.ToString();

                // Redirect to AdminOrderDetails.aspx with OrderID as a query string
                Response.Redirect($"AdminOrderDetails.aspx?OrderID={orderId}");
            }
        }
    }
}
