<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminOrderList.aspx.cs" Inherits="Assg1.AdminOrderList1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Order List</title>
    <style>
        .container {
            max-width: 1200px;
            margin: 50px auto;
            background-color: #fff;
            padding: 20px;
            border-radius: 5px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }

        h1 {
            text-align: center;
            color: #333;
            margin-bottom: 20px;
        }

        .filter-bar {
            display: flex;
            gap: 10px;
            margin-bottom: 20px;
        }

        .filter-bar input,
        .filter-bar select,
        .filter-bar button {
            padding: 10px;
            border-radius: 5px;
            border: 1px solid #ccc;
        }

        table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }

        th, td {
            padding: 10px;
            border: 1px solid #ddd;
            text-align: left;
        }

        th {
            background-color: #5DB996;
            color: #fff;
        }

        .btn-view {
            color: #fff;
            background-color: #28a745;
            padding: 5px 10px;
            text-decoration: none;
            border-radius: 5px;
        }

        .btn-view:hover {
            background-color: #218838;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>Order List</h1>
            <!-- Filter Bar -->
            <div class="filter-bar">
                <asp:TextBox ID="txtOrderID" runat="server" CssClass="filter-input" placeholder="Enter Order ID" />
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="filter-dropdown">
                    <asp:ListItem Text="All" Value="All" />
                    <asp:ListItem Text="Order Placed" Value="Order Placed" />
                    <asp:ListItem Text="To Ship" Value="To Ship" />
                    <asp:ListItem Text="Order Shipped Out" Value="Order Shipped Out" />
                    <asp:ListItem Text="Completed" Value="Completed" />
                    <asp:ListItem Text="Rated" Value="Rated" />
                </asp:DropDownList>
                <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="filter-button" OnClick="btnFilter_Click" />
            </div>
            <!-- Data Table -->
            <asp:GridView ID="OrderGridView" runat="server" AutoGenerateColumns="False" CssClass="table" OnRowCommand="OrderGridView_RowCommand">
                <Columns>
                    <asp:BoundField DataField="OrderID" HeaderText="Order ID" />
                    <asp:BoundField DataField="CustomerID" HeaderText="Customer ID" />
                    <asp:BoundField DataField="OrderDate" HeaderText="Order Date" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    <asp:BoundField DataField="ShippingAddress" HeaderText="Address" />
                    <asp:BoundField DataField="TotalItems" HeaderText="Total Items" />
                     <asp:BoundField DataField="TotalAmount" HeaderText="Total Price" DataFormatString="RM {0:N}" />
                    <asp:TemplateField HeaderText="Order Details">
                        <ItemTemplate>
                            <asp:Button ID="btnView" runat="server" Text="View" CommandName="ViewOrder" CommandArgument='<%# Eval("OrderID") %>' CssClass="btn-view" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
