using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Assg1
{
    public partial class SummaryOrder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string orderIdStr = Request.QueryString["orderId"];
                if (!string.IsNullOrEmpty(orderIdStr) && int.TryParse(orderIdStr, out int orderId))
                {
                    LoadOrderSummary(orderId);
                }
                else
                {
                    Response.Redirect("Cart.aspx");
                }
            }
        }

        private void LoadOrderSummary(int orderId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT O.OrderID, O.TotalAmount, O.OrderDate,
                           C.Email, 
                           DD.FirstName + ' ' + DD.LastName as RecipientName,
                           DD.Address + ', ' + DD.State + ' ' + DD.PostalCode as DeliveryAddress,
                           DD.DeliveryDate, DD.TimeSlot
                    FROM Orders O
                    JOIN Customer C ON O.CustomerID = C.CustomerId
                    JOIN DeliveryDetails DD ON O.OrderID = DD.OrderID
                    WHERE O.OrderID = @OrderId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderId", orderId);

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblOrderID.Text = reader["OrderID"].ToString();
                                lblEmail.Text = reader["Email"].ToString();
                                lblTotalAmount.Text = $"RM {reader["TotalAmount"]:0.00}";
                                lblDeliveryDate.Text = Convert.ToDateTime(reader["DeliveryDate"]).ToString("dd/MM/yyyy");
                                lblTimeSlot.Text = reader["TimeSlot"].ToString();
                                lblRecipient.Text = reader["RecipientName"].ToString();
                                lblAddress.Text = reader["DeliveryAddress"].ToString();
                            }
                            else
                            {
                                Response.Redirect("Cart.aspx");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Response.Write($"<script>alert('Error loading order summary: {ex.Message}');</script>");
                    }
                }
            }
        }
    }
}