<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="Assignment.WebForm3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forgot Password</title>
    <link href="Styles/ForgotPassword.css" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;600;800&display=swap" rel="stylesheet" />
</head>
<body>
    <div class="forgot-password-container">  
        <form id="form1" runat="server" class="forgot-password-form">  
            <div class="form-header">  
                <h2>Customer Login</h2>  
                <p class="form-subtitle">Forgot Password</p>  
            </div>  

            <div class="form-content">  
                <div class="form-section email-section">  
                    <div class="form-group">  
                        <label for="<%= txtEmail.ClientID %>" class="form-label">Email Address</label>  
                        <asp:TextBox  
                            ID="txtEmail"  
                            runat="server"  
                            CssClass="form-control"  
                            Placeholder="Enter your registered email"  
                            aria-describedby="emailHelp" OnTextChanged="txtEmail_TextChanged" AutoPostBack="True" />  
                    </div>  
                </div>  

                <div class="form-section security-section">  
                    <div class="form-group">  
                        <label for="<%= ddlSecurityQuestion.ClientID %>" class="form-label">Security Question</label>  
                        <asp:DropDownList   
                            ID="ddlSecurityQuestion"   
                            runat="server"   
                            CssClass="form-control"
                            Enabled="false">  
                            <asp:ListItem></asp:ListItem>  
                            <asp:ListItem>What was your childhood best friend's nickname?</asp:ListItem>  
                            <asp:ListItem>What was the first thing you learned to cook?</asp:ListItem>  
                            <asp:ListItem>What was your dream job as a child?</asp:ListItem>  
                            <asp:ListItem>In what city did your parents meet?</asp:ListItem>  
                            <asp:ListItem>Who is your all-time favourite movie character?</asp:ListItem>  
                        </asp:DropDownList>  
                    </div>  

                    <div class="form-group">  
                        <label for="<%= txtYourAnswer.ClientID %>" class="form-label">Your Answer</label>  
                        <asp:TextBox   
                            ID="txtYourAnswer"   
                            runat="server"   
                            CssClass="form-control"   
                            placeholder="Enter your security answer"></asp:TextBox>  
                    </div>  
                </div>  

                <div class="verification-notice">  
                    <p>Your email address and security answer are required to complete the verification process.</p>  
                </div>  

                <div class="message-container">  
                    <asp:Label  
                        ID="lblMsg"  
                        runat="server"  
                        CssClass="message"  
                        aria-live="polite"  
                        Visible="false" ForeColor="Red" />  
                </div>  
            </div>  

            <div class="form-actions">  
                <asp:Button  
                    ID="btnSubmit"  
                    runat="server"  
                    Text="Submit"  
                    CssClass="btn btn-primary" OnClick="btnSubmit_Click" />  
                <asp:Button  
                    ID="btnCancel"  
                    runat="server"  
                    Text="Cancel"  
                    CssClass="btn btn-secondary"  
                    PostBackUrl="~/CustomerLogin.aspx" />  
            </div>  
        </form>  
    </div>
</body>
</html>
