using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

namespace Assg1
{
    public partial class OrderListA : System.Web.UI.Page
    {
        protected string currentFilter = "All";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string customerId = Session["CustomerId"]?.ToString() ?? "8";
                BindOrders(currentFilter, customerId);
                UpdateFilterStyles();
            }
        }

        protected bool IsOrderReceivableStatus(string status)
        {
            return status == "Order Placed" || status == "To Ship" || status == "Shipped Out";
        }

        protected void UpdateFilterStyles()
        {
            lnkAll.CssClass = currentFilter == "All" ? "filter-active" : "";
            lnkToShip.CssClass = currentFilter == "ToShip" ? "filter-active" : "";
            lnkToReceive.CssClass = currentFilter == "ToReceive" ? "filter-active" : "";
            lnkCompleted.CssClass = currentFilter == "Completed" ? "filter-active" : "";
            lnkToRate.CssClass = currentFilter == "ToRate" ? "filter-active" : "";
        }

        protected void BindOrders(string filterType, string customerId)
        {
            currentFilter = filterType;
            string strCon = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(strCon))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;

                    string baseQuery = @"
                SELECT DISTINCT 
                    o.OrderID,
                    o.Status,
                    o.TotalAmount,
                    p.ProductName,
                    p.ImageURL,  -- Make sure this matches your database exactly
                    oi.Quantity,
                    oi.Color,
                    ps.Size
                FROM Orders o
                INNER JOIN OrderItems oi ON o.OrderID = oi.OrderID
                INNER JOIN Products p ON oi.ProductID = p.ProductID
                INNER JOIN ProductSizes ps ON oi.ProductSizeID = ps.ProductSizeID
                WHERE o.CustomerID = @CustomerID";

                    switch (filterType)
                    {
                        case "ToShip":
                            baseQuery += " AND o.Status = 'To Ship'";
                            break;
                        case "ToReceive":
                            baseQuery += " AND o.Status = 'Shipped Out'";
                            break;
                        case "Completed":
                            baseQuery += " AND o.Status = 'Completed'";
                            break;
                        case "ToRate":
                            baseQuery += " AND (o.Status = 'Completed' OR o.Status = 'Rated')";
                            break;
                    }

                    baseQuery += " ORDER BY o.OrderID DESC";

                    cmd.CommandText = baseQuery;
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);

                    DataTable dt = new DataTable();
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    var groupedOrders = dt.AsEnumerable()
                        .GroupBy(row => row.Field<int>("OrderID"))
                        .Select(g => new
                        {
                            OrderID = g.Key,
                            Status = g.First().Field<string>("Status"),
                            TotalAmount = g.First().Field<decimal>("TotalAmount"),
                            Items = g.Select(row => new
                            {
                                ProductName = row.Field<string>("ProductName"),
                                ImageURL = row.Field<string>("ImageURL"),
                                Size = row.Field<string>("Size"),
                                Color = row.Field<string>("Color"),
                                Quantity = row.Field<int>("Quantity")
                            }).ToList()
                        })
                        .ToList();

                    rptOrderGroups.DataSource = groupedOrders;
                    rptOrderGroups.DataBind();
                }
            }

            UpdateFilterStyles();
        }
        protected void rptOrderGroups_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var orderGroup = e.Item.DataItem as dynamic;
                Repeater rptOrderItems = (Repeater)e.Item.FindControl("rptOrderItems");

                if (rptOrderItems != null && orderGroup != null)
                {
                    rptOrderItems.DataSource = orderGroup.Items;
                    rptOrderItems.DataBind();
                }
            }
        }

        protected void UpdateOrderStatus(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Received")
            {
                string orderId = e.CommandArgument.ToString();
                string strCon = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

                using (SqlConnection con = new SqlConnection(strCon))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE Orders SET Status = 'Completed' WHERE OrderID = @OrderID", con))
                    {
                        cmd.Parameters.AddWithValue("@OrderID", orderId);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                // Refresh the orders list
                string customerId = Session["CustomerId"]?.ToString() ?? "8";
                BindOrders(currentFilter, customerId);

                ScriptManager.RegisterStartupScript(this, GetType(), "UpdateSuccess",
                    "alert('Order status updated successfully!');", true);
            }
        }

        protected void RateOrder(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Rate")
            {
                string orderId = e.CommandArgument.ToString();
                Response.Redirect($"RateOrder.aspx?OrderID={orderId}");
            }
        }

        protected void lnkFilter_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string filterType = btn.CommandArgument;
            string customerId = Session["CustomerId"]?.ToString() ?? "8";
            BindOrders(filterType, customerId);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();
            string customerId = Session["CustomerId"]?.ToString() ?? "8";

            if (string.IsNullOrEmpty(searchText))
            {
                BindOrders("All", customerId);
                return;
            }

            string strCon = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(strCon))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;

                    string searchQuery = @"
                SELECT DISTINCT 
                    o.OrderID,
                    o.Status,
                    o.TotalAmount,
                    p.ProductName,
                    p.ImageURL,
                    oi.Quantity,
                    oi.Color,
                    ps.Size
                FROM Orders o
                INNER JOIN OrderItems oi ON o.OrderID = oi.OrderID
                INNER JOIN Products p ON oi.ProductID = p.ProductID
                INNER JOIN ProductSizes ps ON oi.ProductSizeID = ps.ProductSizeID
                WHERE o.CustomerID = @CustomerID 
                AND (CAST(o.OrderID AS NVARCHAR(50)) LIKE @SearchText 
                    OR p.ProductName LIKE @SearchText)
                ORDER BY o.OrderID DESC";

                    cmd.CommandText = searchQuery;
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);
                    cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");

                    DataTable dt = new DataTable();
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    var groupedOrders = dt.AsEnumerable()
                        .GroupBy(row => row.Field<int>("OrderID"))
                        .Select(g => new
                        {
                            OrderID = g.Key,
                            Status = g.First().Field<string>("Status"),
                            TotalAmount = g.First().Field<decimal>("TotalAmount"),
                            Items = g.Select(row => new
                            {
                                ProductName = row.Field<string>("ProductName"),
                                ImageURL = row.Field<string>("ImageURL"),
                                Size = row.Field<string>("Size"),
                                Color = row.Field<string>("Color"),
                                Quantity = row.Field<int>("Quantity")
                            }).ToList()
                        })
                        .ToList();

                    rptOrderGroups.DataSource = groupedOrders;
                    rptOrderGroups.DataBind();
                }
            }

            UpdateFilterStyles();
        }
    }
}