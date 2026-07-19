<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerLogin.aspx.cs" Inherits="Assignment.WebForm1" %>  

<!DOCTYPE html>  

<html xmlns="http://www.w3.org/1999/xhtml">  
<head runat="server">  
    <title>Customer Login</title>  
    <link href="Styles/CustomerLogin.css" rel="stylesheet" type="text/css" />  
</head>  
<body>  
    <form id="form1" runat="server" class="login-form">  
        <div class="form-container">  
            <h2 class="form-title">Customer Login</h2>  
            <div class="form-group">  
                <label for="txtEmail">Email</label>  
                <asp:TextBox ID="txtEmail" runat="server" Required="true" CssClass="form-input"></asp:TextBox>  
            </div>  
            <div class="form-group">  
                <label for="txtPassword">Password</label>  
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Required="true" CssClass="form-input"></asp:TextBox>  
            </div>  
            <asp:Label ID="lblMsg" runat="server" Visible="false" ForeColor="Red" CssClass="error-message"></asp:Label>  
            <div class="form-links">  
                <asp:HyperLink ID="linkForgotPassword" runat="server" ForeColor="Black" NavigateUrl="~/ForgotPassword.aspx">Forgot your password?</asp:HyperLink>  
            </div>  
            <div class="form-button">  
                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn-submit" OnClick="btnLogin_Click" />  
            </div>  
            <div class="form-footer">  
                <span>New Customer? </span><asp:HyperLink ID="linkSignUp" runat="server" ForeColor="Black" NavigateUrl="~/CreateAccount.aspx">Sign up &#x2192;</asp:HyperLink>  
                <br />  
                <span>Admin? </span><asp:HyperLink ID="linkAdmin" runat="server" ForeColor="Black" NavigateUrl="~/AdminLogin.aspx">Click here</asp:HyperLink>  
            </div>  
        </div>  
    </form>  
</body>  
</html>  