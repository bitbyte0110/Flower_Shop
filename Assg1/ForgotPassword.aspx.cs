using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace Assignment
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void txtEmail_TextChanged(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            ddlSecurityQuestion.SelectedIndex = 0;
            txtYourAnswer.Text = "";
            
            SqlConnection con;
            string strCon = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

            con = new SqlConnection(strCon);
            con.Open();

            SqlDataReader dtrCustomer = retrieve(con);

            if (dtrCustomer.Read())
            {
                string securityQuestion = dtrCustomer["SecurityQuestion"].ToString();
                ListItem matchingItem = ddlSecurityQuestion.Items.FindByText(securityQuestion);
                if (matchingItem != null)
                {
                    ddlSecurityQuestion.SelectedValue = matchingItem.Value;
                }
            }
            else
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Oops! We couldn't find an account with those details. Would you like to try again or create a new account?";
            }

            con.Close();
        }

        private SqlDataReader retrieve(SqlConnection con)
        {
            string strEmailSearch = txtEmail.Text;

            string strRetrieve = "SELECT * FROM CUSTOMER WHERE Email = @Email";

            SqlCommand cmdR;
            cmdR = new SqlCommand(strRetrieve, con);
            cmdR.Parameters.AddWithValue("@Email", strEmailSearch);

            SqlDataReader dtrCustomer = cmdR.ExecuteReader();

            return dtrCustomer;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SqlConnection con;
            string strCon = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

            con = new SqlConnection(strCon);
            con.Open();

            SqlDataReader dtrCustomer = retrieve(con);

            if (dtrCustomer.Read())
            {
                string yourAnswer = dtrCustomer["YourAnswer"].ToString();

                if (txtYourAnswer.Text == yourAnswer)
                {
                    string email = dtrCustomer["Email"].ToString();
                    Session["Email"] = email.ToString();
                    Response.Redirect("~/ResetPassword.aspx");
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Oops! It seems like the answer to your security question is incorrect. Please double-check your response.";
                }
            }
        }
    }
}