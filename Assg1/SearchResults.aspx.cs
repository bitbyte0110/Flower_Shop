using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Assg1
{
    public partial class SearchResults : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string query = Request.QueryString["q"];
                if (!string.IsNullOrEmpty(query))
                {
                    ViewState["SearchQuery"] = query; // Save query for filtering
                    LoadSearchResults(query);
                    LoadCategories();
                }
                else
                {
                    Response.Write("<script>alert('No search term provided.');</script>");
                }
            }
        }

        private void LoadSearchResults(string query, string category = null, string priceRange = null, string color = null)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

            // Base query
            string sqlQuery = @"
        SELECT ProductID, ProductName, ImageURL, Price, DiscountPrice, Rating 
        FROM dbo.Products 
        WHERE ProductName LIKE @SearchQuery";

            List<SqlParameter> parameters = new List<SqlParameter>
    {
        new SqlParameter("@SearchQuery", "%" + query + "%")
    };

            // Add filters
            if (!string.IsNullOrEmpty(category) && category != "0")
            {
                sqlQuery += " AND CategoryID = @CategoryID";
                parameters.Add(new SqlParameter("@CategoryID", category));
            }
            if (!string.IsNullOrEmpty(priceRange) && priceRange != "0")
            {
                switch (priceRange)
                {
                    case "1":
                        sqlQuery += " AND DiscountPrice < 100";
                        break;
                    case "2":
                        sqlQuery += " AND DiscountPrice BETWEEN 100 AND 199";
                        break;
                    case "3":
                        sqlQuery += " AND DiscountPrice BETWEEN 200 AND 299";
                        break;
                }
            }
            if (!string.IsNullOrEmpty(color) && color != "0")
            {
                sqlQuery += " AND Color = @Color";
                parameters.Add(new SqlParameter("@Color", color));
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        SearchResultsRepeater.DataSource = reader;
                        SearchResultsRepeater.DataBind();
                    }
                    else
                    {
                        SearchResultsRepeater.DataSource = null;
                        SearchResultsRepeater.DataBind();
                        ShowNoResultsMessage("No results found for the applied filters.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors gracefully
                Response.Write($"<script>alert('Error loading search results: {ex.Message}');</script>");
            }
        }

        protected void FilterProducts(object sender, EventArgs e)
        {
            string query = ViewState["SearchQuery"]?.ToString() ?? string.Empty;
            string category = CategoryDropDown.SelectedValue;
            string priceRange = PriceDropDown.SelectedValue;
            string color = ColorDropDown.SelectedValue;

            LoadSearchResults(query, category, priceRange, color);
        }

        private void ShowNoResultsMessage(string message)
        {
            SearchResultsRepeater.DataSource = null;
            SearchResultsRepeater.DataBind();

            // Add a placeholder or visible message to the UI
            Response.Write($"<script>alert('{message}');</script>");
        }

        private void LoadCategories()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;
            string query = "SELECT CategoryID, CategoryName FROM dbo.Categories";

            try
            {
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

                CategoryDropDown.Items.Insert(0, new ListItem("All Categories", "0"));
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error loading categories: {ex.Message}');</script>");
            }
        }


        protected void BackButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProductListing.aspx");
        }
    }
}
