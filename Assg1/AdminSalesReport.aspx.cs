using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Linq;

namespace Assg1
{
    public partial class SalesReport : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateYearDropdown();
                ReportTypeDropdown.SelectedIndex = 0; // Default to Monthly Report
            }
        }

        private void PopulateYearDropdown()
        {
            int currentYear = DateTime.Now.Year;
            for (int year = currentYear; year >= currentYear - 5; year--)
            {
                YearDropdown.Items.Add(new ListItem(year.ToString(), year.ToString()));
            }
        }

        protected void GenerateReportButton_Click(object sender, EventArgs e)
        {
            string reportType = ReportTypeDropdown.SelectedValue;
            int selectedYear = int.Parse(YearDropdown.SelectedValue);
            LoadSalesReport(reportType, selectedYear);
        }

        private void LoadSalesReport(string reportType, int year)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query;

                    if (reportType == "Monthly")
                    {
                        query = @"
                    SELECT 
                        MONTH(o.OrderDate) AS SaleMonth, 
                        p.ProductName, 
                        SUM(oi.Quantity) AS TotalQuantity, 
                        SUM(oi.Quantity * p.Price) AS TotalRevenue
                    FROM OrderItems oi
                    INNER JOIN Products p ON oi.ProductID = p.ProductID
                    INNER JOIN Orders o ON oi.OrderID = o.OrderID
                    WHERE YEAR(o.OrderDate) = @Year
                    GROUP BY MONTH(o.OrderDate), p.ProductName
                    ORDER BY MONTH(o.OrderDate)";
                    }
                    else // Yearly
                    {
                        query = @"
                    SELECT 
                        p.ProductName, 
                        SUM(oi.Quantity) AS TotalQuantity, 
                        SUM(oi.Quantity * p.Price) AS TotalRevenue
                    FROM OrderItems oi
                    INNER JOIN Products p ON oi.ProductID = p.ProductID
                    INNER JOIN Orders o ON oi.OrderID = o.OrderID
                    WHERE YEAR(o.OrderDate) = @Year
                    GROUP BY p.ProductName";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Year", year);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (reportType == "Monthly")
                        {
                            // Group data by month
                            var groupedData = dt.AsEnumerable()
                                .GroupBy(row => row.Field<int>("SaleMonth"))
                                .OrderBy(group => group.Key);

                            SalesReportPlaceholder.Controls.Clear(); // Clear previous content
                            decimal totalRevenueYearly = 0;

                            foreach (var monthGroup in groupedData)
                            {
                                int month = monthGroup.Key;
                                DataTable monthTable = monthGroup.CopyToDataTable();

                                // Add a header for the month
                                Label monthHeader = new Label
                                {
                                    Text = $"<h4>Month: {new DateTime(1, month, 1):MMMM}</h4>",
                                    CssClass = "text-primary"
                                };
                                SalesReportPlaceholder.Controls.Add(monthHeader);

                                // Create a GridView for the month
                                GridView monthlyGridView = new GridView
                                {
                                    CssClass = "table table-striped table-bordered",
                                    AutoGenerateColumns = false
                                };

                                monthlyGridView.Columns.Add(new BoundField { HeaderText = "Product Name", DataField = "ProductName" });
                                monthlyGridView.Columns.Add(new BoundField { HeaderText = "Total Quantity Sold", DataField = "TotalQuantity" });
                                monthlyGridView.Columns.Add(new BoundField { HeaderText = "Total Revenue", DataField = "TotalRevenue", DataFormatString = "RM {0:N2}" });

                                monthlyGridView.DataSource = monthTable;
                                monthlyGridView.DataBind();

                                SalesReportPlaceholder.Controls.Add(monthlyGridView);

                                // Update total yearly revenue
                                totalRevenueYearly += monthGroup.Sum(row => row.Field<decimal>("TotalRevenue"));
                            }

                            TotalRevenueLabel.Text = $"Total Revenue (Yearly): RM {totalRevenueYearly:N2}";
                        }
                        else
                        {
                            // Yearly Report (Single table)
                            SalesReportPlaceholder.Controls.Clear(); // Clear placeholder for yearly data

                            GridView yearlyGridView = new GridView
                            {
                                CssClass = "table table-striped table-bordered",
                                AutoGenerateColumns = false
                            };

                            yearlyGridView.Columns.Add(new BoundField { HeaderText = "Product Name", DataField = "ProductName" });
                            yearlyGridView.Columns.Add(new BoundField { HeaderText = "Total Quantity Sold", DataField = "TotalQuantity" });
                            yearlyGridView.Columns.Add(new BoundField { HeaderText = "Total Revenue", DataField = "TotalRevenue", DataFormatString = "RM {0:N2}" });

                            yearlyGridView.DataSource = dt;
                            yearlyGridView.DataBind();

                            SalesReportPlaceholder.Controls.Add(yearlyGridView);

                            decimal totalRevenue = dt.AsEnumerable()
                                .Sum(row => row.Field<decimal>("TotalRevenue"));

                            TotalRevenueLabel.Text = $"Total Revenue: RM {totalRevenue:N2}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors
                TotalRevenueLabel.Text = $"Error: {ex.Message}";
            }
        }

        protected void ReportTypeDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear the placeholder where the report tables are generated dynamically
            SalesReportPlaceholder.Controls.Clear();

            // Clear the total revenue label
            TotalRevenueLabel.Text = string.Empty;

            // Ensure the dropdowns and placeholders are reset when switching report types
            if (ReportTypeDropdown.SelectedValue == "Yearly")
            {
                // Optional: Add a default message or placeholder for yearly reports
                Label placeholderLabel = new Label
                {
                    Text = "Please select a year and generate the yearly report.",
                    CssClass = "text-info"
                };
                SalesReportPlaceholder.Controls.Add(placeholderLabel);
            }
        }
    }
}
