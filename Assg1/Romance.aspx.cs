using System;
using System.Data.SqlClient;
using System.Configuration;

namespace Assg1
{
    public partial class Romance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadRomanceProducts();
            }
        }

        private void LoadRomanceProducts()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;
                string query = "SELECT ProductID, ProductName, Description, Price, DiscountPrice, ImageURL, Rating FROM dbo.Products WHERE CategoryID = (SELECT CategoryID FROM dbo.Categories WHERE CategoryName = 'Romance')";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        ProductRepeater.DataSource = reader;
                        ProductRepeater.DataBind();
                    }
                    else
                    {
                        ProductRepeater.DataSource = null;
                        ProductRepeater.DataBind();
                        Response.Write("<script>alert('No products found in the Romance category.');</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                Response.Write($"<script>alert('Error loading Romance products: {ex.Message}');</script>");
            }
        }

        protected void BackButton_Click(object sender, EventArgs e)
        {
            // Redirect back to Product Listing page
            Response.Redirect("ProductListing.aspx");
        }

        private void LogError(Exception ex)
        {
            // Log errors for debugging purposes
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
        }
    }
}
