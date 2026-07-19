using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Configuration;

namespace Assg1
{
    public partial class ProductListing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    // Determine if a search is active based on session state
                    bool isSearch = Session["IsSearch"] != null && (bool)Session["IsSearch"];
                    ToggleFilters(!isSearch); // Hide filters if searching, show otherwise

                    BackButton.Visible = isSearch; // Show the back button only during a search

                    // Load categories only if not searching
                    if (!isSearch)
                    {
                        LoadCategories();
                    }

                    // Load products (always execute)
                    LoadProducts();
                }
                catch (Exception ex)
                {
                    LogError(ex);
                    Response.Write($"<script>alert('An error occurred: {ex.Message}');</script>");
                }
            }
        }

        private void LoadCategories()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;
                string query = "SELECT CategoryID, CategoryName FROM dbo.Categories";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    CategoryDropDown.DataSource = reader;
                    CategoryDropDown.DataTextField = "CategoryName";
                    CategoryDropDown.DataValueField = "CategoryID";
                    CategoryDropDown.DataBind();
                }

                // Add default option
                CategoryDropDown.Items.Insert(0, new ListItem("All Categories", "0"));
            }
            catch (Exception ex)
            {
                LogError(ex);
                Response.Write($"<script>alert('Error loading categories: {ex.Message}');</script>");
            }
        }

        private void LoadProducts(string filterQuery = "", List<SqlParameter> parameters = null)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

                // Use DISTINCT to avoid duplicates
                string query = @"
                SELECT DISTINCT ProductID, ProductName, Description, Price, DiscountPrice, ImageURL, Rating, Color 
                FROM dbo.Products 
                WHERE 1 = 1 " + filterQuery;

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameters if provided
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            ProductRepeater.DataSource = reader;
                            ProductRepeater.DataBind();
                        }
                        else
                        {
                            ProductRepeater.DataSource = null;
                            ProductRepeater.DataBind();
                            Response.Write("<script>alert('No products found.');</script>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                Response.Write($"<script>alert('Error loading products: {ex.Message}');</script>");
            }
        }

        private void ToggleFilters(bool isVisible)
        {
            filtersPanel.Visible = isVisible; // Toggle the filter panel visibility
        }

        protected void FilterProducts(object sender, EventArgs e)
        {
            try
            {
                string filterQuery = "";
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (CategoryDropDown.SelectedValue != "0")
                {
                    filterQuery += " AND CategoryID = @CategoryID";
                    parameters.Add(new SqlParameter("@CategoryID", CategoryDropDown.SelectedValue));
                }

                if (PriceDropDown.SelectedValue == "1")
                {
                    filterQuery += " AND DiscountPrice < 100";
                }
                else if (PriceDropDown.SelectedValue == "2")
                {
                    filterQuery += " AND DiscountPrice BETWEEN 100 AND 199";
                }
                else if (PriceDropDown.SelectedValue == "3")
                {
                    filterQuery += " AND DiscountPrice BETWEEN 200 AND 299";
                }

                if (ColorDropDown.SelectedValue != "0")
                {
                    filterQuery += " AND Color = @Color";
                    parameters.Add(new SqlParameter("@Color", ColorDropDown.SelectedValue));
                }

                Session["IsSearch"] = false; // Reset search state
                ToggleFilters(true); // Show filters
                BackButton.Visible = false; // Hide back button
                LoadProducts(filterQuery, parameters);
            }
            catch (Exception ex)
            {
                LogError(ex);
                Response.Write($"<script>alert('Error filtering products: {ex.Message}');</script>");
            }
        }

        protected void BackButton_Click(object sender, EventArgs e)
        {
            // Reset search state
            Session["IsSearch"] = false;

            // Redirect back to the product listing page
            Response.Redirect("ProductListing.aspx");
        }

        private void LogError(Exception ex)
        {
            // Log errors to a file or monitoring system
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
        }
    }
}