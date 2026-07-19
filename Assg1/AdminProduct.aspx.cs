using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Assg1
{
    public partial class AdminProduct : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCategoryDropdown();
                LoadProducts();
            }
        }

        private void LoadProducts()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                    SELECT 
                        p.ProductID, 
                        p.ProductName, 
                        p.Price, 
                        p.DiscountPrice, 
                        p.ImageURL, 
                        ps.Stock, 
                        p.Description, 
                        p.Rating, 
                        p.Color, 
                        ps.Size, 
                        c.CategoryName,
                        c.CategoryID
                    FROM 
                        Products p
                    INNER JOIN 
                        Categories c ON p.CategoryID = c.CategoryID
                    INNER JOIN 
                        ProductSizes ps ON p.ProductID = ps.ProductID";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    ProductGridView.DataSource = dt;
                    ProductGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorMessageLabel.Text = $"Error loading products: {ex.Message}";
            }
        }

        protected void AddUpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ProductNameTextBox.Text) ||
                    !decimal.TryParse(PriceTextBox.Text, out decimal price) ||
                    !decimal.TryParse(StandardPriceTextBox.Text, out decimal standardPrice) ||
                    !decimal.TryParse(PremiumPriceTextBox.Text, out decimal premiumPrice) ||
                    !decimal.TryParse(DeluxePriceTextBox.Text, out decimal deluxePrice) ||
                    !int.TryParse(StandardStockTextBox.Text, out int standardStock) ||
                    !int.TryParse(PremiumStockTextBox.Text, out int premiumStock) ||
                    !int.TryParse(DeluxeStockTextBox.Text, out int deluxeStock) ||
                    string.IsNullOrEmpty(CategoryDropdown.SelectedValue) ||
                    string.IsNullOrEmpty(ColorDropDown.SelectedValue))
                {
                    ErrorMessageLabel.Text = "Invalid input. Please check all fields.";
                    return;
                }

                // Ensure CategoryID exists in the Categories table
                int categoryId = int.Parse(CategoryDropdown.SelectedValue);
                if (!CategoryExists(categoryId))
                {
                    ErrorMessageLabel.Text = "Selected category does not exist.";
                    return;
                }

                string imageUrl = ImageFileUpload.HasFile
                    ? SaveUploadedFile(ImageFileUpload)
                    : ImageURLTextBox.Text.Trim();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string productQuery = string.IsNullOrEmpty(HiddenProductID.Value)
                        ? "INSERT INTO Products (ProductName, Price, DiscountPrice, ImageURL, Description, Rating, CategoryID, Color) OUTPUT INSERTED.ProductID VALUES (@Name, @Price, @DiscountPrice, @ImageURL, @Description, @Rating, @CategoryID, @Color)"
                        : "UPDATE Products SET ProductName = @Name, Price = @Price, DiscountPrice = @DiscountPrice, ImageURL = @ImageURL, Description = @Description, Rating = @Rating, CategoryID = @CategoryID, Color = @Color WHERE ProductID = @ID";

                    using (SqlCommand productCmd = new SqlCommand(productQuery, conn))
                    {
                        productCmd.Parameters.AddWithValue("@Name", ProductNameTextBox.Text);
                        productCmd.Parameters.AddWithValue("@Price", price);
                        productCmd.Parameters.AddWithValue("@DiscountPrice", standardPrice);
                        productCmd.Parameters.AddWithValue("@ImageURL", imageUrl);
                        productCmd.Parameters.AddWithValue("@Description", DescriptionTextBox.Text);
                        productCmd.Parameters.AddWithValue("@Rating", RatingTextBox.Text);
                        productCmd.Parameters.AddWithValue("@CategoryID", categoryId);
                        productCmd.Parameters.AddWithValue("@Color", ColorDropDown.SelectedValue);

                        if (!string.IsNullOrEmpty(HiddenProductID.Value))
                        {
                            productCmd.Parameters.AddWithValue("@ID", Convert.ToInt32(HiddenProductID.Value));
                            productCmd.ExecuteNonQuery();

                            // Update sizes
                            int productId = Convert.ToInt32(HiddenProductID.Value);
                            InsertOrUpdateProductSize(conn, productId, "Standard", standardPrice, standardStock);
                            InsertOrUpdateProductSize(conn, productId, "Premium", premiumPrice, premiumStock);
                            InsertOrUpdateProductSize(conn, productId, "Deluxe", deluxePrice, deluxeStock);
                        }
                        else
                        {
                            int productId = (int)productCmd.ExecuteScalar();
                            InsertOrUpdateProductSize(conn, productId, "Standard", standardPrice, standardStock);
                            InsertOrUpdateProductSize(conn, productId, "Premium", premiumPrice, premiumStock);
                            InsertOrUpdateProductSize(conn, productId, "Deluxe", deluxePrice, deluxeStock);
                        }
                    }
                }

                LoadProducts();
                ClearFields();
                SuccessMessageLabel.Text = "Product added/updated successfully.";
            }
            catch (Exception ex)
            {
                ErrorMessageLabel.Text = $"Error saving product: {ex.Message}";
            }
        }

        private bool CategoryExists(int categoryId)
        {
            string query = "SELECT COUNT(*) FROM Categories WHERE CategoryID = @CategoryID";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@CategoryID", categoryId);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        private string SaveUploadedFile(FileUpload fileUpload)
        {
            string fileName = fileUpload.FileName;
            string savePath = Server.MapPath("~/images/") + fileName;
            fileUpload.SaveAs(savePath);
            return "/images/" + fileName;
        }

        private void InsertOrUpdateProductSize(SqlConnection conn, int productId, string size, decimal price, int stock)
        {
            try
            {
                string sizeQuery = @"
                MERGE INTO ProductSizes AS target
                USING (SELECT @ProductID AS ProductID, @Size AS Size) AS source
                ON target.ProductID = source.ProductID AND target.Size = source.Size
                WHEN MATCHED THEN 
                    UPDATE SET Price = @Price, Stock = @Stock
                WHEN NOT MATCHED THEN
                    INSERT (ProductID, Size, Price, Stock) 
                    VALUES (@ProductID, @Size, @Price, @Stock);";

                using (SqlCommand sizeCmd = new SqlCommand(sizeQuery, conn))
                {
                    sizeCmd.Parameters.AddWithValue("@ProductID", productId);
                    sizeCmd.Parameters.AddWithValue("@Size", size);
                    sizeCmd.Parameters.AddWithValue("@Price", price);
                    sizeCmd.Parameters.AddWithValue("@Stock", stock);

                    System.Diagnostics.Debug.WriteLine("Executing Query: " + sizeQuery);
                    System.Diagnostics.Debug.WriteLine($"Parameters: ProductID={productId}, Size={size}, Price={price}, Stock={stock}");

                    sizeCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating ProductSizes for ProductID {productId}, Size {size}: {ex.Message}");
            }
        }

        private void BindCategoryDropdown()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT CategoryID, CategoryName FROM Categories";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    CategoryDropdown.DataSource = dt;
                    CategoryDropdown.DataTextField = "CategoryName";
                    CategoryDropdown.DataValueField = "CategoryID";
                    CategoryDropdown.DataBind();
                }

                // Add a default option
                CategoryDropdown.Items.Insert(0, new ListItem("--Select Category--", "0"));
            }
            catch (Exception ex)
            {
                ErrorMessageLabel.Text = $"Error loading categories: {ex.Message}";
            }
        }

        private void ClearFields()
        {
            // Clear hidden fields
            HiddenProductID.Value = string.Empty;

            // Clear text fields
            ProductNameTextBox.Text = string.Empty;
            PriceTextBox.Text = string.Empty;
            ImageURLTextBox.Text = string.Empty;
            DescriptionTextBox.Text = string.Empty;
            RatingTextBox.Text = string.Empty;

            // Reset dropdown lists to their default values
            ColorDropDown.SelectedIndex = 0;
            CategoryDropdown.SelectedIndex = 0;

            // Clear size-specific fields
            StandardPriceTextBox.Text = string.Empty;
            StandardStockTextBox.Text = string.Empty;
            PremiumPriceTextBox.Text = string.Empty;
            PremiumStockTextBox.Text = string.Empty;
            DeluxePriceTextBox.Text = string.Empty;
            DeluxeStockTextBox.Text = string.Empty;

            // Clear messages
            SuccessMessageLabel.Text = string.Empty;
            ErrorMessageLabel.Text = string.Empty;
        }

        protected void ProductGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int productId = Convert.ToInt32(ProductGridView.DataKeys[e.RowIndex].Value);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Delete related rows in ProductSizes table
                    string deleteSizesQuery = "DELETE FROM ProductSizes WHERE ProductID = @ProductID";
                    using (SqlCommand deleteSizesCmd = new SqlCommand(deleteSizesQuery, conn))
                    {
                        deleteSizesCmd.Parameters.AddWithValue("@ProductID", productId);
                        deleteSizesCmd.ExecuteNonQuery();
                    }

                    // Delete the product from Products table
                    string deleteProductQuery = "DELETE FROM Products WHERE ProductID = @ProductID";
                    using (SqlCommand deleteProductCmd = new SqlCommand(deleteProductQuery, conn))
                    {
                        deleteProductCmd.Parameters.AddWithValue("@ProductID", productId);
                        deleteProductCmd.ExecuteNonQuery();
                    }
                }

                LoadProducts();
                SuccessMessageLabel.Text = "Product and related sizes deleted successfully.";
            }
            catch (Exception ex)
            {
                ErrorMessageLabel.Text = $"Error deleting product: {ex.Message}";
            }
        }

        protected void ProductGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int productId = Convert.ToInt32(ProductGridView.SelectedRow.Cells[0].Text.Trim());

                // Get the category ID directly from database for the selected product
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT CategoryID FROM Products WHERE ProductID = @ProductID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        conn.Open();
                        object categoryId = cmd.ExecuteScalar();
                        if (categoryId != null)
                        {
                            CategoryDropdown.SelectedValue = categoryId.ToString();
                        }
                    }
                }

                GridViewRow row = ProductGridView.SelectedRow;
                HiddenProductID.Value = row.Cells[0].Text.Trim();
                ProductNameTextBox.Text = row.Cells[1].Text.Trim();
                PriceTextBox.Text = row.Cells[2].Text.Trim();
                ImageURLTextBox.Text = row.Cells[5].Text.Trim();
                DescriptionTextBox.Text = row.Cells[6].Text.Trim();
                RatingTextBox.Text = row.Cells[7].Text.Trim();
                ColorDropDown.SelectedValue = row.Cells[8].Text.Trim();
                LoadSizeSpecificDetails(productId);

                ScriptManager.RegisterStartupScript(this, GetType(), "ScrollToForm", "scrollToForm();", true);
            }
            catch (Exception ex)
            {
                ErrorMessageLabel.Text = $"Error loading product details: {ex.Message}";
            }
        }

        private void LoadSizeSpecificDetails(int productId)
        {
            string query = @"
        SELECT Size, Price, Stock 
        FROM ProductSizes 
        WHERE ProductID = @ProductID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string size = reader["Size"].ToString();
                            decimal price = Convert.ToDecimal(reader["Price"]);
                            int stock = Convert.ToInt32(reader["Stock"]);

                            switch (size.ToLower())
                            {
                                case "standard":
                                    StandardPriceTextBox.Text = price.ToString("0.00");
                                    StandardStockTextBox.Text = stock.ToString();
                                    break;
                                case "premium":
                                    PremiumPriceTextBox.Text = price.ToString("0.00");
                                    PremiumStockTextBox.Text = stock.ToString();
                                    break;
                                case "deluxe":
                                    DeluxePriceTextBox.Text = price.ToString("0.00");
                                    DeluxeStockTextBox.Text = stock.ToString();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessageLabel.Text = $"Error loading size-specific details: {ex.Message}";
            }
        }

        protected void ProductGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ProductGridView.PageIndex = e.NewPageIndex;
            LoadProducts();
        }

        protected void ClearButton_Click(object sender, EventArgs e)
        {
            ClearFields();
            SuccessMessageLabel.Text = string.Empty;
            ErrorMessageLabel.Text = string.Empty;
        }
    }
}