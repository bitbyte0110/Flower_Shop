using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Assg1
{
    public partial class CheckOutA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDeliveryDetails();
                LoadCartItems();
            }
            if (Session["custID"] == null)
            {
                Session["custID"] = "8";
            }

        }

        private void LoadDeliveryDetails()
        {
            if (Session["DeliveryDate"] != null)
                lblDeliveryDate.Text = Session["DeliveryDate"].ToString();
            if (Session["TimeSlot"] != null)
                lblTimeSlot.Text = Session["TimeSlot"].ToString();
        }

        private void LoadCartItems()
        {
            try
            {
                string strCustId = Session["custID"]?.ToString() ?? "8";
                string selectedItems = Session["SelectedCartItems"]?.ToString();

                string query = @"
            SELECT C.CartID, C.ProductName, C.Size, C.Color, C.Quantity, 
                   C.TotalPrice as Price, P.ImageURL
            FROM Cart C
            JOIN Products P ON P.ProductName = C.ProductName
            WHERE C.CustomerID = @CustomerID";

                if (!string.IsNullOrEmpty(selectedItems))
                {
                    query += " AND C.CartID IN (" + selectedItems + ")";
                }

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", strCustId);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            decimal subtotal = 0;
                            List<object> cartItems = new List<object>();

                            while (reader.Read())
                            {
                                decimal price = Convert.ToDecimal(reader["Price"]);
                                subtotal += price;
                                cartItems.Add(new
                                {
                                    ProductName = reader["ProductName"].ToString(),
                                    Size = reader["Size"].ToString(),
                                    Color = reader["Color"].ToString(),
                                    Quantity = Convert.ToInt32(reader["Quantity"]),
                                    Price = price,
                                    ImageURL = reader["ImageURL"].ToString(),
                                });
                            }

                            rptCartItems.DataSource = cartItems;
                            rptCartItems.DataBind();

                            decimal shipping = 5.00m;
                            lblSubtotal.Text = $"RM {subtotal:0.00}";
                            lblTotal.Text = $"RM {(subtotal + shipping):0.00}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    $"alert('Error loading cart items: {ex.Message}');", true);
            }
        }

        protected void btnPayNow_Click(object sender, EventArgs e)
        {
            try
            {
                string custId = Session["custID"]?.ToString() ?? "8";
                decimal totalAmount = decimal.Parse(lblTotal.Text.Replace("RM ", ""));

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString))
                {
                    conn.Open();

                    // Create order record
                    string orderQuery = @"
                INSERT INTO Orders (CustomerID, OrderDate, TotalAmount, Status)
                VALUES (@CustomerId, GETDATE(), @TotalAmount, 'To Ship');
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    int orderId;
                    using (SqlCommand cmd = new SqlCommand(orderQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", custId);
                        cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                        orderId = (int)cmd.ExecuteScalar();
                    }

                    // Add delivery details
                    string deliveryQuery = @"
                INSERT INTO DeliveryDetails 
                (OrderID, FirstName, LastName, Address, Address2, State, PostalCode, Country, 
                 PhoneNumber, DeliveryDate, TimeSlot, Status, SenderName, PersonalMessage)
                VALUES 
                (@OrderId, @FirstName, @LastName, @Address, @Address2, @State, @PostalCode, @Country,
                 @Phone, @DeliveryDate, @TimeSlot, 'To Ship', @SenderName, @PersonalMessage)";

                    using (SqlCommand cmd = new SqlCommand(deliveryQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@OrderId", orderId);
                        cmd.Parameters.AddWithValue("@FirstName", ShippingFirstName.Value);
                        cmd.Parameters.AddWithValue("@LastName", ShippingLastName.Value);
                        cmd.Parameters.AddWithValue("@Address", ShippingAddress.Value);
                        cmd.Parameters.AddWithValue("@Address2", ShippingAddress2.Value ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@State", ShippingState.Value);
                        cmd.Parameters.AddWithValue("@PostalCode", ShippingCode.Value);
                        cmd.Parameters.AddWithValue("@Country", "Malaysia");
                        cmd.Parameters.AddWithValue("@Phone", phone1.Value);
                        cmd.Parameters.AddWithValue("@DeliveryDate", Convert.ToDateTime(Session["DeliveryDate"]));
                        cmd.Parameters.AddWithValue("@TimeSlot", Session["TimeSlot"]);
                        cmd.Parameters.AddWithValue("@SenderName", Session["SenderName"]);
                        cmd.Parameters.AddWithValue("@PersonalMessage", Session["PersonalMessage"]);
                        cmd.ExecuteNonQuery();
                    }

                    // Process cart items
                    string selectedItems = Session["SelectedCartItems"]?.ToString();
                    if (!string.IsNullOrEmpty(selectedItems))
                    {
                        string orderItemQuery = @"
                    INSERT INTO OrderItems (OrderID, ProductID, ProductSizeID, Quantity, Price, Color)
                    SELECT @OrderId, P.ProductID, PS.ProductSizeID, C.Quantity, PS.Price, C.Color
                    FROM Cart C
                    JOIN Products P ON P.ProductName = C.ProductName
                    JOIN ProductSizes PS ON PS.ProductID = P.ProductID AND PS.Size = C.Size
                    WHERE C.CartID = @CartId;

                    DELETE FROM Cart WHERE CartID = @CartId;";

                        foreach (string cartId in selectedItems.Split(','))
                        {
                            if (string.IsNullOrEmpty(cartId)) continue;

                            using (SqlCommand cmd = new SqlCommand(orderItemQuery, conn))
                            {
                                cmd.Parameters.AddWithValue("@OrderId", orderId);
                                cmd.Parameters.AddWithValue("@CartId", cartId);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    // Clear session
                    Session["SelectedCartItems"] = null;
                    Session["DeliveryDate"] = null;
                    Session["TimeSlot"] = null;

                    // Redirect to summary
                    Response.Redirect($"SummaryORder.aspx?orderId={orderId}", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    $"alert('Error processing order: {ex.Message}');", true);
            }
        }




    }
}