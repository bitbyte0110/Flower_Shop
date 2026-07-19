<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="Assignment.WebForm10" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reset Password</title>
    <link href="Styles/ResetPassword.css" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;600;800&display=swap" rel="stylesheet" />
</head>
<body>
    <div class="reset-password-container">
        <form id="form1" runat="server" class="reset-password-form">
            <div class="form-header">
                <h2>Reset Password</h2>
                <p class="form-subtitle">Create a new secure password</p>
            </div>

            <div class="form-content">
                <div class="form-section password-section">
                    <div class="form-group">
                        <label for="<%= txtNewPassword.ClientID %>" class="form-label">New Password</label>
                        <asp:TextBox 
                            ID="txtNewPassword" 
                            runat="server" 
                            CssClass="form-control" 
                            TextMode="Password" 
                            Placeholder="Enter your new password"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label for="<%= txtConfirmPassword.ClientID %>" class="form-label">Confirm Password</label>
                        <asp:TextBox 
                            ID="txtConfirmPassword" 
                            runat="server" 
                            CssClass="form-control" 
                            TextMode="Password" 
                            Placeholder="Confirm your new password"></asp:TextBox>
                    </div>
                </div>

                <div class="password-requirements">
                    <p>Password must:</p>
                    <ul>
                        <li>Be at least 8 characters long</li>
                        <li>Include at least one uppercase letter</li>
                        <li>Include at least one number</li>
                        <li>Include at least one special character</li>
                    </ul>
                </div>

                <div class="message-container">
                    <asp:Label 
                        ID="lblMessage" 
                        runat="server" 
                        CssClass="message" 
                        Visible="false"></asp:Label>
                </div>
            </div>

            <div class="form-actions">
                <asp:Button 
                    ID="btnResetPassword" 
                    runat="server" 
                    Text="Reset Password" 
                    CssClass="btn btn-primary" OnClick="btnResetPassword_Click" />
            </div>
        </form>
    </div>
</body>
</html>