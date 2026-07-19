using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Assg1
{
    public partial class LockedAccountsManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadLockedAccounts();
            }
        }

        private void LoadLockedAccounts()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT " +
                                "CustomerID AS [CustomerID], " +
                                "FirstName AS [FirstName], " +
                                "LastName AS [LastName], " +
                                "Email AS [Email] " +
                                "FROM Customer WHERE IsAccountLocked = 1";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dtLockedUsers = new DataTable();

                conn.Open();
                adapter.Fill(dtLockedUsers);

                dtLockedUsers.Columns.Add("Name", typeof(string), "FirstName + ' ' + LastName");

                gvLockedAccounts.DataSource = dtLockedUsers;
                gvLockedAccounts.DataBind();
            }
        }

        protected void gvLockedAccounts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UnlockAccount")
            {
                int CustomerID = Convert.ToInt32(e.CommandArgument);
                UnlockUserAccount(CustomerID);
            }
        }

        private void UnlockUserAccount(int CustomerID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE CUSTOMER SET IsAccountLocked = 0, FailedLoginAttempts = 0 WHERE CustomerID = @CustomerID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        lblMessage.Text = "Account successfully unlocked!";
                        LoadLockedAccounts(); // Refresh grid  
                    }
                    else
                    {
                        lblMessage.Text = "Failed to unlock account.";
                    }
                }
            }
        }
    }
}