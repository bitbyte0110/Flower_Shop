using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Windows.Input;
using System.Web.Services.Description;

namespace Assignment
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCompleteRegistration_Click(object sender, EventArgs e)
        {
            int customerId = CustomerIDGenerator.GenerateNextCustomerID();

            string firstName = Session["FirstName"]?.ToString();
            string lastName = Session["LastName"]?.ToString();
            string email = Session["Email"]?.ToString();
            string password = Session["Password"]?.ToString();
            string securityQuestion = Session["SecurityQuestion"]?.ToString();
            string yourAnswer = Session["YourAnswer"]?.ToString();

            string hashedPassword = PasswordHasher.HashPassword(password);

            DateTime birthdayCombine = new DateTime
            (
                int.Parse(txtYear.Text),
                int.Parse(txtMonth.Text),
                int.Parse(txtDay.Text) 
            );

            string birthday = birthdayCombine.ToString("yyyy-MM-dd");

            string address = $"{txtAddressLine1.Text.Trim()}, {txtAddressLine2.Text.Trim()}";

            string defaultProfilePictureURL = "~/Images/default-profile-picture.png";

            SqlConnection con;
            string strCon = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

            con = new SqlConnection(strCon);
            con.Open();

            string strInsert = "";
            strInsert = "INSERT INTO CUSTOMER (CustomerId, FirstName, LastName, Email, PasswordHash, SecurityQuestion, YourAnswer, Birthday, Address, City, State, PostalCode, Country, ProfilePictureURL) VALUES (@CustomerId, @FirstName, @LastName, @Email, @PasswordHash, @SecurityQuestion, @YourAnswer, @Birthday, @Address, @City, @State, @PostalCode, @Country, @ProfilePictureURL)";

            SqlCommand cmdInsert;
            cmdInsert = new SqlCommand(strInsert, con);

            cmdInsert.Parameters.AddWithValue("@CustomerId", customerId);
            cmdInsert.Parameters.AddWithValue("@FirstName", firstName);
            cmdInsert.Parameters.AddWithValue("@LastName", lastName);
            cmdInsert.Parameters.AddWithValue("@Email", email);
            cmdInsert.Parameters.AddWithValue("@PasswordHash", hashedPassword);
            cmdInsert.Parameters.AddWithValue("@SecurityQuestion", securityQuestion);
            cmdInsert.Parameters.AddWithValue("@YourAnswer", yourAnswer);
            cmdInsert.Parameters.AddWithValue("@Birthday", birthday);
            cmdInsert.Parameters.AddWithValue("@Address", address);
            cmdInsert.Parameters.AddWithValue("@City", txtCity.Text);
            cmdInsert.Parameters.AddWithValue("@State", ddlState.SelectedValue.ToString());
            cmdInsert.Parameters.AddWithValue("@PostalCode", txtPostalCode.Text);
            cmdInsert.Parameters.AddWithValue("@Country", txtCountry.Text);
            cmdInsert.Parameters.AddWithValue("@ProfilePictureURL", defaultProfilePictureURL);

            //string strIdentityInsert = "SET IDENTITY_INSERT CUSTOMER ON";
            //SqlCommand cmdIdentityInsert;
            //cmdIdentityInsert = new SqlCommand(strIdentityInsert, con);
            //cmdIdentityInsert.ExecuteNonQuery();

            int intInsertStatus = cmdInsert.ExecuteNonQuery();

            if (intInsertStatus > 0)
            {
                Session.Clear();
                Session["CustomerId"] = customerId.ToString();
                Response.Redirect("~/HOME.aspx");
            }
            else
            {
                Console.WriteLine("Insertion Failed.");
            }

            //strIdentityInsert = "SET IDENTITY_INSERT CUSTOMER OFF";
            //cmdIdentityInsert.ExecuteNonQuery();

            con.Close();
        }

        public static class PasswordHasher
        {
            public static string HashPassword(string password)
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("X2"));
                    }
                    return builder.ToString();
                }
            }
        }

        public class CustomerIDGenerator
        {
            private static string strCon = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

            public static int GenerateNextCustomerID()
            {
                SqlConnection con;

                con = new SqlConnection(strCon);
                con.Open();

                string queryLastID = "";
                queryLastID = "SELECT ISNULL(MAX(CustomerId), 0) + 1 FROM Customer";

                SqlCommand cmdSearch;
                cmdSearch = new SqlCommand(queryLastID, con);

                object result = cmdSearch.ExecuteScalar();

                return result != null ? Convert.ToInt32(result) : 1;
            }
        }
    }
}