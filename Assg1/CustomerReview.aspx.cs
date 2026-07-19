using System;
using System.Configuration;
using System.Data.SqlClient;

namespace MyNamespace
{
    public partial class CustomerReview : System.Web.UI.Page
    {
        protected void SubmitReviewButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Retrieve the selected rating
                string productRatingValue = ProductRatingHiddenField.Value;
                if (string.IsNullOrEmpty(productRatingValue))
                {
                    ErrorMessageLabel.Text = "Please select a rating for product quality.";
                    return;
                }

                if (!int.TryParse(productRatingValue, out int rating) || rating < 1 || rating > 5)
                {
                    ErrorMessageLabel.Text = "Please provide a valid rating between 1 and 5.";
                    return;
                }

                string comments = CommentsTextBox.Text.Trim();

                // Replace these with dynamic OrderID and CustomerID logic
                int orderId = GetOrderId(); // Fetch the OrderID dynamically
                int customerId = GetCustomerId(); // Fetch the CustomerID dynamically

                if (orderId <= 0 || customerId <= 0)
                {
                    ErrorMessageLabel.Text = "Invalid Order ID or Customer ID.";
                    return;
                }

                // Fetch connection string
                string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if the customer already left a review for the order
                    string checkQuery = "SELECT COUNT(1) FROM Reviews WHERE OrderID = @OrderID AND CustomerID = @CustomerID";
                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@OrderID", orderId);
                        checkCommand.Parameters.AddWithValue("@CustomerID", customerId);

                        int existingReviewCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (existingReviewCount > 0)
                        {
                            // Update existing review
                            string updateQuery = @"UPDATE Reviews 
                                                   SET Rating = @Rating, 
                                                       Comments = @Comments, 
                                                       ReviewDate = @ReviewDate 
                                                   WHERE OrderID = @OrderID AND CustomerID = @CustomerID";

                            using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                            {
                                updateCommand.Parameters.AddWithValue("@OrderID", orderId);
                                updateCommand.Parameters.AddWithValue("@CustomerID", customerId);
                                updateCommand.Parameters.AddWithValue("@Rating", rating);
                                updateCommand.Parameters.AddWithValue("@Comments", comments);
                                updateCommand.Parameters.AddWithValue("@ReviewDate", DateTime.Now);

                                updateCommand.ExecuteNonQuery();
                                SuccessMessageLabel.Text = "Review updated successfully!";
                                ErrorMessageLabel.Text = string.Empty;
                                ClearForm();
                                return;
                            }
                        }
                    }

                    // Insert new review if none exists
                    string insertQuery = @"INSERT INTO Reviews 
                                           (OrderID, CustomerID, Rating, Comments, ReviewDate) 
                                           VALUES 
                                           (@OrderID, @CustomerID, @Rating, @Comments, @ReviewDate)";

                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@OrderID", orderId);
                        insertCommand.Parameters.AddWithValue("@CustomerID", customerId);
                        insertCommand.Parameters.AddWithValue("@Rating", rating);
                        insertCommand.Parameters.AddWithValue("@Comments", comments);
                        insertCommand.Parameters.AddWithValue("@ReviewDate", DateTime.Now);

                        int rowsAffected = insertCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            SuccessMessageLabel.Text = "Review submitted successfully!";
                            ErrorMessageLabel.Text = string.Empty;
                            ClearForm();
                        }
                        else
                        {
                            ErrorMessageLabel.Text = "Failed to submit the review.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessageLabel.Text = "An error occurred: " + ex.Message;
                SuccessMessageLabel.Text = string.Empty;
            }
        }

        private int GetOrderId()
        {
            // Replace this with logic to retrieve the current OrderID
            // For example, fetch from a query string or session
            return 1; // Example value
        }

        private int GetCustomerId()
        {
            // Replace this with logic to retrieve the current CustomerID
            // For example, fetch from the logged-in user session
            return 1; // Example value
        }

        private void ClearForm()
        {
            ProductRatingHiddenField.Value = string.Empty; // Clear the hidden field
            CommentsTextBox.Text = string.Empty;           // Clear the comments box
            SuccessMessageLabel.Text = string.Empty;       // Clear the success message
            ErrorMessageLabel.Text = string.Empty;
        }
    }
}
