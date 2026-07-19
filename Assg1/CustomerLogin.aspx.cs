using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Assignment
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                string check = Session["ResetPassword"]?.ToString();
                if (check == "Success")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Success! Your password has been reset. You can now log in with your new password.');", true);
                    Session.Clear();
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            SqlConnection con;
            string strCon = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

            con = new SqlConnection(strCon);
            con.Open();

            string strEmailSearch = txtEmail.Text;

            string strRetrieve = "SELECT * FROM CUSTOMER WHERE Email = @Email";

            SqlCommand cmdR;
            cmdR = new SqlCommand(strRetrieve, con);
            cmdR.Parameters.AddWithValue("@Email", strEmailSearch);

            SqlDataReader dtrCustomer = cmdR.ExecuteReader();

            if (dtrCustomer.Read())
            {
                string inputPassword = txtPassword.Text; ;
                string hashedInputPassword = WebForm4.PasswordHasher.HashPassword(inputPassword);

                string passwordHash = dtrCustomer["PasswordHash"].ToString();
                int failedLoginAttempts = Convert.ToInt32(dtrCustomer["FailedLoginAttempts"]);
                int isAccountLocked = Convert.ToInt32(dtrCustomer["IsAccountLocked"]);
                const int MAX_FAILED_ATTEMPTS = 3;
                int intUpdateStatus;

                SqlConnection con1;
                string strCon1 = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

                con1 = new SqlConnection(strCon1);
                con1.Open();

                string strUpdate = "UPDATE CUSTOMER SET FailedLoginAttempts = @FailedLoginAttempts WHERE Email = @Email";

                SqlCommand cmdU;
                cmdU = new SqlCommand(strUpdate, con1);

                if (hashedInputPassword == passwordHash && isAccountLocked == 0)
                {
                    string customerId = dtrCustomer["CustomerId"].ToString();
                    Session["CustomerId"] = customerId.ToString();

                    failedLoginAttempts = 0;
                    cmdU.Parameters.AddWithValue("@FailedLoginAttempts", failedLoginAttempts);
                    cmdU.Parameters.AddWithValue("@Email", txtEmail.Text);
                    intUpdateStatus = cmdU.ExecuteNonQuery();
                    if (intUpdateStatus <= 0)
                    {
                        Console.WriteLine("Update Failed.");
                    }

                    Response.Redirect("~/HOME.aspx");
                }
                else
                {
                    if (failedLoginAttempts < MAX_FAILED_ATTEMPTS)
                    {
                        failedLoginAttempts++;
                        cmdU.Parameters.AddWithValue("@FailedLoginAttempts", failedLoginAttempts);
                        cmdU.Parameters.AddWithValue("@Email", txtEmail.Text);
                        intUpdateStatus = cmdU.ExecuteNonQuery();
                        if (intUpdateStatus <= 0)
                        {
                            Console.WriteLine("Update Failed.");
                        }
                        lblMsg.Visible = true;
                        lblMsg.Text = "Oops! It seems like something went wrong with your login. Please check your details or consider resetting your password.";
                    }
                    else
                    {
                        isAccountLocked = 1;
                        strUpdate = "UPDATE CUSTOMER SET IsAccountLocked = @IsAccountLocked WHERE Email = @Email";
                        SqlCommand cmdU1;
                        cmdU1 = new SqlCommand(strUpdate, con1);
                        cmdU1.Parameters.AddWithValue("@IsAccountLocked", isAccountLocked);
                        cmdU1.Parameters.AddWithValue("@Email", txtEmail.Text);
                        intUpdateStatus = cmdU1.ExecuteNonQuery();
                        if (intUpdateStatus <= 0)
                        {
                            Console.WriteLine("Update Failed.");
                        }
                        lblMsg.Visible = true;
                        lblMsg.Text = "Oops! Your account has been locked due to exceeding the maximum number of failed login attempts. Please contact the administrator to unlock your account.";
                    }
                }
                con1.Close();
            }
            else
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Oops! We couldn't find an account with those details. Would you like to try again or create a new account?";
            }

            con.Close();
        }
    }
}