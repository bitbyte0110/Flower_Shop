using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Assg1
{
    public class CheckoutHandler
    {
        private readonly string connectionString;

        public CheckoutHandler(string connString)
        {
            connectionString = connString;
        }

        public class BillingInfo
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Address { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string PostalCode { get; set; }
            public string Country { get; set; }
            public string PhoneNumber { get; set; }
        }

        public class DeliveryInfo : BillingInfo
        {
            public string DeliveryDate { get; set; }
            public string TimeSlot { get; set; }
            public string SenderName { get; set; }
            public string PersonalMessage { get; set; }
        }

        public int CreateOrder(int customerId, decimal totalAmount, BillingInfo billingInfo, DeliveryInfo deliveryInfo)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Create Order
                        int orderId = InsertOrder(conn, transaction, customerId, totalAmount);

                        // Create Billing Address
                        int billingId = InsertBillingAddress(conn, transaction, orderId, billingInfo);

                        // Create Delivery Details
                        int shippingId = InsertDeliveryDetails(conn, transaction, orderId, deliveryInfo);

                        // Update Order with Billing and Shipping IDs
                        UpdateOrder(conn, transaction, orderId, billingId, shippingId);

                        // Transfer Cart Items to Order Items
                        TransferCartItems(conn, transaction, orderId, customerId);

                        // Clear Cart
                        ClearCart(conn, transaction, customerId);

                        transaction.Commit();
                        return orderId;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private int InsertOrder(SqlConnection conn, SqlTransaction transaction, int customerId, decimal totalAmount)
        {
            string query = @"
            INSERT INTO Orders (CustomerID, OrderDate, TotalAmount, Status)
            VALUES (@CustomerId, @OrderDate, @TotalAmount, 'Pending');
            SELECT SCOPE_IDENTITY();";

            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                cmd.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private int InsertBillingAddress(SqlConnection conn, SqlTransaction transaction, int orderId, BillingInfo billing)
        {
            string query = @"
            INSERT INTO BillingAddress (OrderID, FirstName, LastName, Address, Address2, 
                                      City, State, PostalCode, Country, PhoneNumber)
            VALUES (@OrderId, @FirstName, @LastName, @Address, @Address2,
                    @City, @State, @PostalCode, @Country, @PhoneNumber);
            SELECT SCOPE_IDENTITY();";

            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                cmd.Parameters.AddWithValue("@FirstName", billing.FirstName);
                cmd.Parameters.AddWithValue("@LastName", billing.LastName);
                cmd.Parameters.AddWithValue("@Address", billing.Address);
                cmd.Parameters.AddWithValue("@Address2", billing.Address2 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@City", billing.City);
                cmd.Parameters.AddWithValue("@State", billing.State);
                cmd.Parameters.AddWithValue("@PostalCode", billing.PostalCode);
                cmd.Parameters.AddWithValue("@Country", billing.Country);
                cmd.Parameters.AddWithValue("@PhoneNumber", billing.PhoneNumber);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private int InsertDeliveryDetails(SqlConnection conn, SqlTransaction transaction, int orderId, DeliveryInfo delivery)
        {
            string query = @"
            INSERT INTO DeliveryDetails (OrderID, FirstName, LastName, Address, Address2, 
                                       City, State, PostalCode, Country, PhoneNumber,
                                       Status, DeliveryDate, TimeSlot, SenderName, PersonalMessage)
            VALUES (@OrderId, @FirstName, @LastName, @Address, @Address2,
                    @City, @State, @PostalCode, @Country, @PhoneNumber,
                    'Pending', @DeliveryDate, @TimeSlot, @SenderName, @PersonalMessage);
            SELECT SCOPE_IDENTITY();";

            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                cmd.Parameters.AddWithValue("@FirstName", delivery.FirstName);
                cmd.Parameters.AddWithValue("@LastName", delivery.LastName);
                cmd.Parameters.AddWithValue("@Address", delivery.Address);
                cmd.Parameters.AddWithValue("@Address2", delivery.Address2 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@City", delivery.City);
                cmd.Parameters.AddWithValue("@State", delivery.State);
                cmd.Parameters.AddWithValue("@PostalCode", delivery.PostalCode);
                cmd.Parameters.AddWithValue("@Country", delivery.Country);
                cmd.Parameters.AddWithValue("@PhoneNumber", delivery.PhoneNumber);
                cmd.Parameters.AddWithValue("@DeliveryDate", DateTime.Parse(delivery.DeliveryDate));
                cmd.Parameters.AddWithValue("@TimeSlot", delivery.TimeSlot);
                cmd.Parameters.AddWithValue("@SenderName", delivery.SenderName);
                cmd.Parameters.AddWithValue("@PersonalMessage", delivery.PersonalMessage ?? (object)DBNull.Value);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void UpdateOrder(SqlConnection conn, SqlTransaction transaction, int orderId, int billingId, int shippingId)
        {
            string query = "UPDATE Orders SET BillingID = @BillingId, ShippingID = @ShippingId WHERE OrderID = @OrderId";

            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                cmd.Parameters.AddWithValue("@BillingId", billingId);
                cmd.Parameters.AddWithValue("@ShippingId", shippingId);
                cmd.ExecuteNonQuery();
            }
        }

        private void TransferCartItems(SqlConnection conn, SqlTransaction transaction, int orderId, int customerId)
        {
            string query = @"
            INSERT INTO OrderItems (OrderID, ProductID, ProductSizeID, Quantity, Price, Color)
            SELECT @OrderId, P.ProductID, PS.ProductSizeID, C.Quantity, PS.Price, C.Color
            FROM Cart C
            JOIN Products P ON P.ProductName = C.ProductName
            JOIN ProductSizes PS ON PS.ProductID = P.ProductID AND PS.Size = C.Size
            WHERE C.CustomerID = @CustomerId";

            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                cmd.ExecuteNonQuery();
            }
        }

        private void ClearCart(SqlConnection conn, SqlTransaction transaction, int customerId)
        {
            string query = "DELETE FROM Cart WHERE CustomerID = @CustomerId";

            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                cmd.ExecuteNonQuery();
            }
        }

        public bool ValidateInput(BillingInfo billing, DeliveryInfo delivery, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(billing.FirstName) || string.IsNullOrWhiteSpace(billing.LastName))
            {
                errorMessage = "Billing name is required";
                return false;
            }

            if (string.IsNullOrWhiteSpace(billing.Address))
            {
                errorMessage = "Billing address is required";
                return false;
            }

            if (string.IsNullOrWhiteSpace(billing.City) || string.IsNullOrWhiteSpace(billing.State))
            {
                errorMessage = "City and state are required";
                return false;
            }

            if (string.IsNullOrWhiteSpace(billing.PostalCode))
            {
                errorMessage = "Postal code is required";
                return false;
            }

            if (string.IsNullOrWhiteSpace(billing.PhoneNumber))
            {
                errorMessage = "Phone number is required";
                return false;
            }

            if (!DateTime.TryParse(delivery.DeliveryDate, out _))
            {
                errorMessage = "Invalid delivery date";
                return false;
            }

            if (string.IsNullOrWhiteSpace(delivery.TimeSlot))
            {
                errorMessage = "Delivery time slot is required";
                return false;
            }

            return true;
        }
    }
}