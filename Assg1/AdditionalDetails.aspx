<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdditionalDetails.aspx.cs" Inherits="Assignment.WebForm4" %>  

<!DOCTYPE html>  

<html xmlns="http://www.w3.org/1999/xhtml">  
<head runat="server">  
    <title>Complete Your Profile</title>  
    <link href="Styles/AdditionalDetails.css" rel="stylesheet" type="text/css" />  
</head>  
<body>  
    <div class="registration-container">  
        <form id="form1" runat="server" class="additional-details-form">  
            <div class="form-header">  
                <h2>Complete Your Profile</h2>  
            </div>  

            <div class="form-section">  
                <div class="form-group">  
                    <label>Date of Birth</label>  
                    <div class="dob-input-group">  
                        <div class="dob-input">  
                            <asp:TextBox ID="txtDay" runat="server" Required="true" Placeholder="DD" TextMode="Number" min="1" max="31" CssClass="form-input dob-day"></asp:TextBox>  
                            <span class="input-label">Day</span>  
                        </div>  
                        <div class="dob-input">  
                            <asp:TextBox ID="txtMonth" runat="server" Required="true" Placeholder="MM" TextMode="Number" min="1" max="12" CssClass="form-input dob-month"></asp:TextBox>  
                            <span class="input-label">Month</span>  
                        </div>  
                        <div class="dob-input">  
                            <asp:TextBox ID="txtYear" runat="server" Required="true" Placeholder="YYYY" TextMode="Number" min="1924" max="2006" CssClass="form-input dob-year"></asp:TextBox>  
                            <span class="input-label">Year</span>  
                        </div>  
                    </div>  
                </div>  

                <div class="form-group">  
                    <label for="txtPhoneNumber">Phone Number</label>  
                    <asp:TextBox ID="txtPhoneNumber" runat="server" Required="true" Placeholder="e.g., 0123456789" CssClass="form-input"></asp:TextBox>  
                </div>  
            </div>  

            <div class="form-section">  
                <div class="form-group">  
                    <label for="txtAddressLine1">Address Line 1</label>  
                    <asp:TextBox ID="txtAddressLine1" runat="server" Required="true" Placeholder="123 Jalan Merdeka" CssClass="form-input"></asp:TextBox>  
                </div>  

                <div class="form-group">  
                    <label for="txtAddressLine2">Address Line 2</label>  
                    <asp:TextBox ID="txtAddressLine2" runat="server" Required="true" Placeholder="Taman Melawati" CssClass="form-input"></asp:TextBox>  
                </div>  

                <div class="form-group">  
                    <label for="txtCity">City</label>  
                    <asp:TextBox ID="txtCity" runat="server" Required="true" Placeholder="Kuala Lumpur" CssClass="form-input"></asp:TextBox>  
                </div>  

                <div class="form-group-ddl">  
                    <label for="ddlState">State</label>  
                    <asp:DropDownList ID="ddlState" runat="server" Required="true" CssClass="form-input dropdown-input">  
                        <asp:ListItem>-- Select State --</asp:ListItem>  
                        <asp:ListItem>Johor</asp:ListItem>  
                        <asp:ListItem>Kedah</asp:ListItem>  
                        <asp:ListItem>Kelantan</asp:ListItem>  
                        <asp:ListItem>Malacca</asp:ListItem>  
                        <asp:ListItem>Negeri Sembilan</asp:ListItem>  
                        <asp:ListItem>Pahang</asp:ListItem>  
                        <asp:ListItem>Penang</asp:ListItem>  
                        <asp:ListItem>Perak</asp:ListItem>  
                        <asp:ListItem>Perlis</asp:ListItem>  
                        <asp:ListItem>Sabah</asp:ListItem>  
                        <asp:ListItem>Sawarak</asp:ListItem>  
                        <asp:ListItem>Selangor</asp:ListItem>  
                        <asp:ListItem>Terengganu</asp:ListItem>  
                        <asp:ListItem>Wilayah Persekutuan Kuala Lumpur</asp:ListItem>  
                        <asp:ListItem>Wilayah Persekutuan Labuan</asp:ListItem>  
                        <asp:ListItem>Wilayah Persekutuan Putrajaya</asp:ListItem>  
                    </asp:DropDownList>  
                </div>  

                <div class="form-group">  
                    <label for="txtPostalCode">Postal Code</label>  
                    <asp:TextBox ID="txtPostalCode" runat="server" Required="true" Placeholder="53100" CssClass="form-input"></asp:TextBox>  
                </div>  

                <div class="form-group">  
                    <label for="txtCountry">Country</label>  
                    <asp:TextBox ID="txtCountry" runat="server" Enabled="false" CssClass="form-input disabled-input" Text="Malaysia"></asp:TextBox>  
                </div>  
            </div>  

            <div class="form-actions">  
                <asp:Button ID="btnCompleteRegistration" runat="server" Text="Complete Registration" OnClick="btnCompleteRegistration_Click" CssClass="btn-submit" />  
            </div>  
        </form>  
    </div>  
</body>  
</html>