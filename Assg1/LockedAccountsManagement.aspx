<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LockedAccountsManagement.aspx.cs" Inherits="Assg1.LockedAccountsManagement" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Locked Accounts Management</title>
    <link href="style/AdminDashboard.css" rel="stylesheet" type="text/css" />
    <style>
        .content-container {
            margin: 20px;
            font-family: Arial, sans-serif;
        }

        .table-header {
            background-color: #5DB996;
            color: white;
            font-weight: bold;
            text-align: left;
        }

        .gridview {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }

        .gridview th, .gridview td {
            border: 1px solid #ddd;
            padding: 8px;
            text-align: left;
        }

        .gridview th {
            background-color: #5DB996;
            color: white;
        }

        .gridview tr:nth-child(even) {
            background-color: #f2f2f2;
        }

        .gridview tr:hover {
            background-color: #ddd;
        }

        .unlock-btn {
            background-color: #4CAF50;
            color: white;
            border: none;
            padding: 5px 10px;
            cursor: pointer;
            border-radius: 4px;
        }

        .unlock-btn:hover {
            background-color: #45a049;
        }

        .title {
            font-size: 24px;
            font-weight: bold;
            color: #5DB996;
            margin-bottom: 20px;
        }
    </style>
</head>
<body>
    <form id="frmLockedAccounts" runat="server">
        <div class="content-container">
            <h1 class="title">Locked Customer Accounts</h1>

            <asp:GridView 
                ID="gvLockedAccounts" 
                runat="server" 
                AutoGenerateColumns="false" 
                CssClass="gridview" 
                OnRowCommand="gvLockedAccounts_RowCommand">
                <Columns>
                    <asp:BoundField DataField="CustomerID" HeaderText="Customer ID" />
                    <asp:BoundField DataField="Name" HeaderText="Name" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />

                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button 
                                ID="btnUnlock" 
                                runat="server" 
                                Text="Unlock Account" 
                                CommandName="UnlockAccount" 
                                CommandArgument='<%# Eval("CustomerID") %>' 
                                CssClass="unlock-btn" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:Label ID="lblMessage" runat="server" ForeColor="Green"></asp:Label>
        </div>
    </form>
</body>
</html>
