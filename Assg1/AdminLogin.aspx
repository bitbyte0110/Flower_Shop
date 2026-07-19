<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminLogin.aspx.cs" Inherits="Assignment.WebForm5" %>  

<!DOCTYPE html>  

<html xmlns="http://www.w3.org/1999/xhtml">  
<head runat="server">  
    <title>Admin Login</title>  
    <link href="Styles/AdminLogin.css" rel="stylesheet" type="text/css" />
</head>  
<body>  
    <form id="form1" runat="server" class="admin-login-container">  
        <div class="login-wrapper">  
            <div class="login-header">  
                <h2>Admin Login</h2>  
                <p class="login-subtitle">Secure Access Panel</p>  
            </div>  

            <div class="form-group">  
                <label for="<%=txtAdminID.ClientID%>">Admin ID</label>  
                <asp:TextBox ID="txtAdminID" runat="server" CssClass="form-input" Required="true"></asp:TextBox>  
            </div>  

            <div class="form-group">  
                <label for="<%=txtPassword.ClientID%>">Password</label>  
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-input" Required="true"></asp:TextBox>  
            </div>  

            <div class="login-message">  
                <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="error-message"></asp:Label>  
            </div>  

            <div class="login-actions">  
                <asp:HyperLink ID="linkForgotPassword" runat="server" CssClass="forgot-password" NavigateUrl="#">Forgot password?</asp:HyperLink>  
                
                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn-login" />  
            </div>  

            <div class="login-footer">  
                <p class="copyright">&copy; <%= DateTime.Now.Year %> Admin Portal. All Rights Reserved.</p>  
                <div class="customer-link">  
                    Customer?   
                    <asp:HyperLink ID="linkCustomer" runat="server" CssClass="link-to-customer" NavigateUrl="~/CustomerLogin.aspx">Click here</asp:HyperLink>  
                </div>  
            </div>  
        </div>  
    </form>  
</body>  
</html>