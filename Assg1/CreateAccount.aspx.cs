using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Assignment
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public class EmailValidator
        {
            private const string EmailRegexPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            public static bool IsValidEmailFormat(string email)
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return false;
                }

                return Regex.IsMatch(email, EmailRegexPattern, RegexOptions.IgnoreCase);
            }

            //public static bool IsEmailAlreadyExists(string email)

            public static EmailValidationResult ValidateEmail(string email)
            {
                var result = new EmailValidationResult
                {
                    Email = email ?? string.Empty,
                };

                result.IsValid = true;

                if (!IsValidEmailFormat(email))
                {
                    result.IsValid = false;
                    result.ErrorMessage = "Please enter a valid email address.";
                    return result;
                }

                return result;
            }

            public class EmailValidationResult
            {
                public string Email { get; set; }
                public bool IsValid { get; set; }
                public bool IsRegistered { get; set; }
                public string ErrorMessage { get; set; }
            }
        }

        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            Session["FirstName"] = txtFirstName.Text;
            Session["LastName"] = txtLastName.Text;
            Session["Email"] = txtEmail.Text;
            Session["Password"] = txtPassword.Text;
            Session["SecurityQuestion"] = ddlSecurityQuestion.SelectedValue.ToString();
            Session["YourAnswer"] = txtYourAnswer.Text;

            Response.Redirect("~/AdditionalDetails.aspx");
        }

        protected void txtEmail_TextChanged(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();

            lblEmailStatus.Text = "";
             
            var validationResult = EmailValidator.ValidateEmail(email);

            if (validationResult.IsValid)
            {
                btnSignUp.Enabled = true;
            }
            else
            {
                lblEmailStatus.Text = validationResult.ErrorMessage;
                btnSignUp.Enabled = false;
            }

        }
    }
}