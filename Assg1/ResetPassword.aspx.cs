using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Assignment
{
    public partial class WebForm10 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMessage.Visible = false;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Verification successful! Your identity has been successfully verified. You can now proceed to reset your password.');", true);
            }
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            // Validate password  
            if (string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                ShowErrorMessage("Password cannot be empty.");
                return;
            }

            // Check password strength  
            if (!IsPasswordStrong(txtNewPassword.Text))
            {
                ShowErrorMessage("Password does not meet requirements.");
                return;
            }

            // Check password confirmation  
            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                ShowErrorMessage("Passwords do not match.");
                return;
            }

            string hashedInputPassword = WebForm4.PasswordHasher.HashPassword(txtConfirmPassword.Text);

            SqlConnection con;
            string strCon = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

            con = new SqlConnection(strCon);
            con.Open();

            string email = Session["Email"]?.ToString();
            Session.Clear();

            string strUpdate = "";
            strUpdate = "UPDATE CUSTOMER SET PasswordHash = @PasswordHash WHERE Email = @Email";

            SqlCommand cmdUpdate;
            cmdUpdate = new SqlCommand(strUpdate, con);

            cmdUpdate.Parameters.AddWithValue("@PasswordHash", hashedInputPassword);
            cmdUpdate.Parameters.AddWithValue("@Email", email);

            int intInsertStatus = cmdUpdate.ExecuteNonQuery();

            if (intInsertStatus > 0)
            {
                Session["ResetPassword"] = "Success";
                Response.Redirect("~/CustomerLogin.aspx");
            }
            else
            {
                Console.WriteLine("Update Failed.");
            }

            con.Close();
        }

        private bool IsPasswordStrong(string password)
        {
            // Check length  
            if (password.Length < 8)
                return false;

            // Check for at least one uppercase letter  
            if (!Regex.IsMatch(password, @"[A-Z]"))
                return false;

            // Check for at least one lowercase letter  
            if (!Regex.IsMatch(password, @"[a-z]"))
                return false;

            // Check for at least one number  
            if (!Regex.IsMatch(password, @"\d"))
                return false;

            // Check for at least one special character  
            if (!Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]"))
                return false;

            return true;
        }

        private void ShowErrorMessage(string message)
        {
            lblMessage.Text = message;
            lblMessage.Visible = true;
        }
    }
}