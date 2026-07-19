<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Account.aspx.cs" Inherits="Assignment.MyAccount" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.7.1/css/all.min.css" integrity="sha512-5Hs3dF2AEPkpNAR7UiOHba+lRSJNeM2ECkwxUIxC1Q/FLycGTbNapWXB4tP889k5T5Ju8fs4b1P5z/iB4nMfSQ==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    <link href="Styles/Account.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">

        <div class="container">

            <div class="menu">
                <a href="Account.aspx" class="menu-link"><i class="fa-solid fa-circle-user menu-icon"></i>Account</a>
                <a href="MyOrders.aspx" class="menu-link"><i class="fa-solid fa-box menu-icon"></i>My Orders</a>
                <a href="Security.aspx" class="menu-link"><i class="fa-solid fa-lock menu-icon"></i>Security</a>
            </div>

            <div class="account">
                <div class="account-header">
                    <h1 class="account-title">Account Setting</h1>
                    <div class="btn-container">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn-cancel" />
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn-save" OnClick="btnSave_Click" />
                    </div>
                </div>
            
                <div class="profile">
                    <div class="profile-header">
                        <asp:Image ID="profilePicture" runat="server" CssClass="profile-img" />
                        <div class="browse-upload-btn-container">
                            <asp:FileUpload ID="profilePictureUpload" runat="server" />
                            <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                            <asp:Label ID="lblMsg" runat="server" Visible="false" ></asp:Label>
                        </div>
                    </div>
                </div>

                <div class="account-edit">
                    <div class="input-container">
                        <p>First Name</p>
                        <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
                    </div>
                    <div class="input-container">
                        <p>Last Name</p>
                        <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
                    </div>
                </div>

                <div class="account-edit">
                    <div class="input-container">
                        <p>Email</p>
                        <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                    </div>
                    <div class="input-container">
                        <p>Date of Birth</p>
                        <table>
                            <tr>
                                <td>Day</td>
                                <td>Month</td>
                                <td>Year</td>
                            </tr>
                            <tr>
                                <td><asp:TextBox ID="txtDay" runat="server" Placeholder="DD"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtMonth" runat="server" Placeholder="MM"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtYear" runat="server" Placeholder="YYYY"></asp:TextBox></td>
                            </tr>
                        </table>
                    </div>
                </div>

                <div class="account-edit">
                    <div class="input-container">
                        <p>Address</p>
                        <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox>
                    </div>
                    <div class="input-container">
                        <p>City</p>
                        <asp:TextBox ID="txtCity" runat="server"></asp:TextBox>
                    </div>
                </div>

                <div class="account-edit">
                    <div class="input-container">
                        <p>State</p>
                        <asp:DropDownList ID="ddlState" runat="server" CssClass="dll">
                            <asp:ListItem>-- Select --</asp:ListItem>
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
                    <div class="input-container">
                        <p>Postal Code</p>
                        <asp:TextBox ID="txtPostalCode" runat="server"></asp:TextBox>
                    </div>
                </div>

                <div class="account-edit">
                    <div class="input-container">
                        <p>Country</p>
                        <asp:TextBox ID="txtCountry" runat="server" Enabled="false">Malaysia</asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
