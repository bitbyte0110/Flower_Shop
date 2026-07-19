using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace Assg1
{
    public partial class ProductDetails : Page
    {
        // Class representing a cart item
        public class CartItem
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public string Size { get; set; }
            public int Quantity { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    // Validate ProductID from query string
                    if (int.TryParse(Request.QueryString["ProductID"], out int productId))
                    {
                        LoadProductDetails(productId);
                        LoadProductSizes(productId);
                        LoadStandardSizePrice(productId);
                        BackButton.Visible = true;
                        LoadRelatedItems(); // Load related items
                    }
                    else
                    {
                        RedirectToListing("Invalid Product ID.");
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex);
                    ShowError("An error occurred while loading the product details.");
                }
            }
        }

        private void LoadProductDetails(int productId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                SELECT ProductName, Description, ImageURL, Price
                FROM dbo.Products 
                WHERE ProductID = @ProductID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ProductNameLabel.Text = reader["ProductName"].ToString();
                                ProductGeneralDescription.InnerText = reader["Description"].ToString();
                                ProductImage.Src = ResolveUrl(reader["ImageURL"].ToString());
                                ProductImage.Alt = reader["ProductName"].ToString();

                                // Update the Price Label
                                ProductPriceLabel.Text = Convert.ToDecimal(reader["Price"]).ToString("0.00");

                                // Set ViewState variables
                                ViewState["ProductID"] = productId;
                                ViewState["ProductName"] = reader["ProductName"].ToString();
                            }
                            else
                            {
                                RedirectToListing("Product not found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                ShowError("Error loading product details.");
            }
        }

        protected void AddToCart_Click(object sender, EventArgs e)
        {
            try
            {
                int productId = int.Parse(Request.QueryString["ProductID"]);
                string selectedSize = Request.Form["SelectedSize"];
                string selectedColor = Request.Form["SelectedColor"]; // Retrieve the selected color
                string productName = ProductNameLabel.Text;
                decimal selectedPrice = decimal.TryParse(Request.Form["SelectedPrice"], out decimal parsedPrice) ? parsedPrice : 1;
                int quantity = int.TryParse(Request.Form["QuantityInput"], out int parsedQuantity) ? parsedQuantity : 1;
                Response.Write("<script>console.log('Selected Price: " + selectedPrice + "');</script>");

                // Validate input
                if (string.IsNullOrEmpty(selectedSize))
                {
                    Response.Write("<script>alert('Please select a size.');</script>");
                    return;
                }
                if (string.IsNullOrEmpty(selectedColor))
                {
                    Response.Write("<script>alert('Please select a color.');</script>");
                    return;
                }

                // Check stock
                int stock = GetStockForSize(productId, selectedSize);
                if (quantity > stock)
                {
                    Response.Write("<script>alert('Insufficient stock.');</script>");
                    return;
                }

                // Reduce stock and add to cart
                UpdateStockForSize(productId, selectedSize, stock - quantity);

                var masterPage = this.Master as Site1;
                if (masterPage != null)
                {
                    masterPage.AddToCart(productId, productName, selectedPrice, selectedSize, selectedColor, quantity); // Include color
                }

                Response.Write($"<script> window.location='ProductListing.aspx';</script>");
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error adding to cart: {ex.Message}');</script>");
            }
        }



        private void LoadRelatedItems()
        {
            if (ViewState["ProductID"] == null)
            {
                ShowError("Product ID is missing. Unable to load related items.");
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                SELECT TOP 5 ProductID, ProductName, ImageURL, 
                       Price, DiscountPrice
                FROM dbo.Products
                WHERE ProductID <> @ProductID
                ORDER BY NEWID()";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", ViewState["ProductID"]);

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                RelatedItemsRepeater.DataSource = reader;
                                RelatedItemsRepeater.DataBind();
                            }
                            else
                            {
                                RelatedItemsRepeater.Visible = false; // Hide if no items
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                ShowError("Error loading related items. Please try again later.");
            }
        }

        private void LoadProductSizes(int productId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT Size, Price, Stock
                        FROM dbo.ProductSizes 
                        WHERE ProductID = @ProductID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Clear existing size options
                            SizeOptions.InnerHtml = "";

                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string size = reader["Size"].ToString();
                                    decimal price = Convert.ToDecimal(reader["Price"]);
                                    int stock = Convert.ToInt32(reader["Stock"]);

                                    // Render size options dynamically
                                    SizeOptions.InnerHtml += $@"
                                        <div class='size-option' onclick=""selectSizeOption('{size}', {price}, {stock})"">
                                            {size}
                                        </div>";
                                }
                            }
                            else
                            {
                                ShowError("No sizes found for this product.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                ShowError("Error loading product sizes. Please try again later.");
            }
        }

        private void LoadStandardSizePrice(int productId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                SELECT Price
                FROM dbo.ProductSizes
                WHERE ProductID = @ProductID AND Size = 'Standard'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);

                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            // Update the Product Price Label
                            ProductPriceLabel.Text = Convert.ToDecimal(result).ToString("0.00");
                        }
                        else
                        {
                            ProductPriceLabel.Text = "N/A"; // Fallback in case no price is found
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                ShowError("Error loading product price. Please try again later.");
            }
        }

        private int GetStockForSize(int productId, string size)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Stock FROM dbo.ProductSizes WHERE ProductID = @ProductID AND Size = @Size";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        cmd.Parameters.AddWithValue("@Size", size);

                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error retrieving stock: {ex.Message}');</script>");
                return 0;
            }
        }

        private void UpdateStockForSize(int productId, string size, int newStock)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        UPDATE dbo.ProductSizes 
                        SET Stock = @Stock 
                        WHERE ProductID = @ProductID AND Size = @Size";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Stock", newStock);
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        cmd.Parameters.AddWithValue("@Size", size);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                ShowError("Error updating stock information. Please try again later.");
            }
        }

        protected void BackButton_Click(object sender, EventArgs e)
        {
            // Navigate back to the product listing page
            Response.Redirect("ProductListing.aspx");
        }

        private void RedirectToListing(string message)
        {
            Response.Write($"<script>alert('{message}'); window.location='ProductListing.aspx';</script>");
        }

        private void ShowError(string message)
        {
            Response.Write($"<script>alert('{message}');</script>");
        }

        private void LogError(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
        }
    }
}
