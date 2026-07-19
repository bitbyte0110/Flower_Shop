using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Assignment
{
    public partial class MyAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string check = Session["UpdateAccountSettings"]?.ToString();
                if (check == "Success")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Success! Your account settings have been updated successfully.');", true);
                    Session["UpdateAccountSettings"] = "";
                }
            }

            SqlConnection con;
            string strCon = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

            con = new SqlConnection(strCon);
            con.Open();

            //string strCustomerIdSearch = "8";
            string strCustomerIdSearch = Session["CustomerId"]?.ToString();

            string strRetrieve = "SELECT * FROM CUSTOMER WHERE CustomerId = @CustomerId";

            SqlCommand cmdR;
            cmdR = new SqlCommand(strRetrieve, con);
            cmdR.Parameters.AddWithValue("@CustomerId", strCustomerIdSearch);

            SqlDataReader dtrCustomer = cmdR.ExecuteReader();

            while (dtrCustomer.Read())
            {
                txtFirstName.Text = dtrCustomer["FirstName"].ToString();
                txtLastName.Text = dtrCustomer["LastName"].ToString();
                txtEmail.Text = dtrCustomer["Email"].ToString();
                
                string birthday = dtrCustomer["Birthday"].ToString();
                // 19/4/2004
                string datePart = birthday.Split(' ')[0];
                string[] parts = datePart.Split('/');
                string day = parts[0];
                string month = parts[1];
                string year = parts[2];
                txtDay.Text = day;
                txtMonth.Text = month;
                txtYear.Text = year;

                txtAddress.Text = dtrCustomer["Address"].ToString();
                txtCity.Text = dtrCustomer["City"].ToString();

                string state = dtrCustomer["State"].ToString();
                ListItem matchingItem = ddlState.Items.FindByText(state);
                if (matchingItem != null)
                {
                    ddlState.SelectedValue = matchingItem.Value; 
                }

                txtPostalCode.Text = dtrCustomer["PostalCode"].ToString();

                string profilePictureURL = dtrCustomer["ProfilePictureURL"].ToString();
                profilePicture.ImageUrl = profilePictureURL;
            }

            con.Close();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (profilePictureUpload.HasFile)
            {
                if (profilePictureUpload.PostedFile.ContentType == "image/jpg" || profilePictureUpload.PostedFile.ContentType == "image/png" || profilePictureUpload.PostedFile.ContentType == "image/jpeg")
                {
                    string pathOfFolder = Server.MapPath("~/Images/");

                    string getFileName = Path.GetFileName(profilePictureUpload.FileName);
                    profilePictureUpload.SaveAs(pathOfFolder + getFileName);
                    lblMsg.Visible = true;
                    lblMsg.Text = getFileName + "has been successfully uploaded.";

                    Session["ProfilePictureURL"] =  "~/Images/" + getFileName;
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Only file formats of JPG, PNG, and JPEG are accepted.";
                }
            }
            else
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Error while uploading the file.";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DateTime birthdayCombine = new DateTime
            (
                int.Parse(txtYear.Text),
                int.Parse(txtMonth.Text),
                int.Parse(txtDay.Text)
            );

            string birthday = birthdayCombine.ToString("yyyy-MM-dd");

            string profilePictureURL = Session["ProfilePictureURL"]?.ToString();

            SqlConnection con;
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            con = new SqlConnection(strCon);
            con.Open();

            string strUpdate = "";
            strUpdate = "UPDATE CUSTOMER SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Birthday = @Birthday, Address = @Address, City = @City, State = @State, PostalCode = @PostalCode, ProfilePictureURL = @ProfilePictureURL WHERE CustomerId = @CustomerId";

            SqlCommand cmdUpdate;
            cmdUpdate = new SqlCommand(strUpdate, con);

            cmdUpdate.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
            cmdUpdate.Parameters.AddWithValue("@LastName", txtLastName.Text);
            cmdUpdate.Parameters.AddWithValue("@Email", txtEmail.Text);
            cmdUpdate.Parameters.AddWithValue("@Birthday", birthday);
            cmdUpdate.Parameters.AddWithValue("@Address", txtAddress.Text);
            cmdUpdate.Parameters.AddWithValue("@City", txtCity.Text);
            cmdUpdate.Parameters.AddWithValue("@State", ddlState.SelectedValue.ToString());
            cmdUpdate.Parameters.AddWithValue("@PostalCode", txtPostalCode.Text);
            cmdUpdate.Parameters.AddWithValue("@ProfilePictureURL", profilePictureURL);
            cmdUpdate.Parameters.AddWithValue("@CustomerId", Session["CustomerId"]?.ToString());

            Session["ProfilePictureURL"] = "";

            int intInsertStatus = cmdUpdate.ExecuteNonQuery();

            if (intInsertStatus > 0)
            {
                Session["UpdateAccountSettings"] = "Success";
            }
            else
            {
                Console.WriteLine("Update Failed.");
            }

            con.Close();
            Response.Redirect("~/Account.aspx");
        }
    }
}