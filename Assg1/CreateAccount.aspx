<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateAccount.aspx.cs" Inherits="Assignment.WebForm2" %>  

<!DOCTYPE html>  

<html xmlns="http://www.w3.org/1999/xhtml">  
<head runat="server">  
    <title>Create Account</title>  
    <link href="Styles/CreateAccount.css" rel="stylesheet" type="text/css" />  
</head>  
<body>  
    <form id="form1" runat="server" class="create-account-form">  
        <div class="form-title">  
            <h2>Create Account</h2>  
        </div>  
        
        <div class="form-group">  
            <label for="txtFirstName">First Name</label>  
            <asp:TextBox ID="txtFirstName" runat="server" Required="true" Placeholder="e.g., John" CssClass="form-input"></asp:TextBox>  
        </div>  
        
        <div class="form-group">  
            <label for="txtLastName">Last Name</label>  
            <asp:TextBox ID="txtLastName" runat="server" Required="true" Placeholder="e.g., Doe" CssClass="form-input"></asp:TextBox>  
        </div>  
        
        <div class="form-group">  
            <label for="txtEmail">Email</label>  
            <asp:TextBox ID="txtEmail" runat="server" Required="true" AutoPostBack="true" Placeholder="e.g., johndoe@example.com" OnTextChanged="txtEmail_TextChanged" CssClass="form-input"></asp:TextBox>  
            <asp:Label ID="lblEmailStatus" runat="server" ForeColor="Red" CssClass="email-status"></asp:Label>  
        </div>  
        
        <div class="form-group">  
            <label for="txtPassword">Password</label>  
            <asp:TextBox ID="txtPassword" runat="server" Required="true" TextMode="Password" CssClass="form-input"></asp:TextBox>  
        </div>  
        
        <div class="form-group-security">  
            <label for="ddlSecurityQuestion">Security Question</label>  
            <asp:DropDownList ID="ddlSecurityQuestion" runat="server" CssClass="form-input dropdown-input">  
                <asp:ListItem>-- Select One --</asp:ListItem>  
                <asp:ListItem>What was your childhood best friend's nickname?</asp:ListItem>  
                <asp:ListItem>What was the first thing you learned to cook?</asp:ListItem>  
                <asp:ListItem>What was your dream job as a child?</asp:ListItem>  
                <asp:ListItem>In what city did your parents meet?</asp:ListItem>  
                <asp:ListItem>Who is your all-time favourite movie character?</asp:ListItem>  
            </asp:DropDownList>  
        </div>  
        
        <div class="form-group">  
            <label for="txtYourAnswer">Your Answer</label>  
            <asp:TextBox ID="txtYourAnswer" runat="server" CssClass="form-input"></asp:TextBox>  
        </div>  
        
        <div class="form-group-button">  
            <asp:Button ID="btnSignUp" runat="server" Text="Sign Up" OnClick="btnSignUp_Click" CssClass="btn-submit" />  
        </div>  
        
        <div class="form-footer">  
            <p>Returning Customer? <asp:HyperLink ID="linkLogin" runat="server" ForeColor="Black" NavigateUrl="~/CustomerLogin.aspx"><u>Login &#x2192</u></asp:HyperLink></p>  
        </div>  
    </form>  
</body>  
</html>