<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminOrderDetails.aspx.cs" Inherits="Assg1.AdminOrderDetails" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Order Details</title>
    <style>
        body {
            font-family: Arial, sans-serif;
        }

        .container {
            max-width: 1200px;
            margin: 20px auto;
        }

        .table {
            width: 100%;
            border-collapse: collapse;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }

        .table th, .table td {
            padding: 10px;
            text-align: left;
            border: 1px solid #ddd;
        }

        .table th {
            background-color: #5DB996; 
            color: white;
        }

        .table td {
            background-color: #f9f9f9;
        }

        .header {
            text-align: center;
            font-size: 1.5em;
            font-weight: bold;
            margin: 20px 0;
        }

        .buttons {
            display: flex;
            justify-content: space-between;
            margin: 20px 0;
        }

        .buttons .btn {
            padding: 10px 15px;
            text-decoration: none;
            color: white;
            background-color: #28a745;
            border: none;
            border-radius: 5px;
            cursor: pointer;
        }

        .btn:hover {
            background-color: #218838;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="header">Order Details</div>

            <!-- Order Details -->
            <div>
                <strong>Order ID:</strong> <asp:Label ID="lblOrderID" runat="server" Text=""></asp:Label> <br />
                <strong>Customer ID:</strong> <asp:Label ID="lblCustomerID" runat="server" Text=""></asp:Label> <br />
                <strong>Order Date:</strong> <asp:Label ID="lblOrderDate" runat="server" Text=""></asp:Label> <br />
                <div>
                    <strong>Order Status:</strong>
                    <asp:DropDownList ID="ddlOrderStatus" runat="server" CssClass="dropdown">
                        <asp:ListItem Text="Order Placed" Value="Order Placed" />
                        <asp:ListItem Text="To Ship" Value="To Ship" />
                        <asp:ListItem Text="Shipped Out" Value="Shipped Out" />
                        <asp:ListItem Text="Completed" Value="Completed" />
                        <asp:ListItem Text="Rated" Value="Rated" />
                    </asp:DropDownList>
                </div>
            </div>

            <!-- Order Items Table -->
            <asp:GridView ID="gvOrderItems" runat="server" AutoGenerateColumns="False" CssClass="table">
                <Columns>
                    <asp:BoundField DataField="OrderItemID" HeaderText="ID" />
                    <asp:BoundField DataField="ProductID" HeaderText="Product ID" />
                    <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                    <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="RM {0:N}" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                    <asp:TemplateField HeaderText="Subtotal">
                        <ItemTemplate>
                            RM <%# Convert.ToDecimal(Eval("Price")) * Convert.ToInt32(Eval("Quantity")) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <!-- Buttons -->
            <div class="buttons">
                <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn" OnClick="btnUpdate_Click" />
                <asp:Button ID="btnBack" runat="server" Text="Back to Order List Page" CssClass="btn" OnClick="btnBack_Click" />
            </div>
        </div>
    </form>
</body>
</html>
