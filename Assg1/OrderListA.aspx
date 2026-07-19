<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="OrderListA.aspx.cs" Inherits="Assg1.OrderListA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.7.1/css/all.min.css" />
    <link href="..\Styles\OrderListA.css" rel="stylesheet" type="text/css" />
    <style>
        .filter-active {
            background-color: #5DB996 !important;
            color: white !important;
            border-radius: 5px;
            padding: 10px 15px;
        }
        
        .order-group {
            border: 1px solid #ddd;
            border-radius: 5px;
            margin-bottom: 20px;
            background-color: #fff;
        }
        
        .order-group .order-item {
            border: none;
            border-bottom: 1px solid #eee;
            margin-bottom: 0;
        }
        
        .order-group .order-item:last-child {
            border-bottom: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="my-orders-page">
            <div class="container">
                <div class="menu">
                    <a href="Account.aspx" class="menu-link"><i class="fa-solid fa-circle-user menu-icon"></i>Account</a>
                    <a href="OrderListA.aspx" class="menu-link"><i class="fa-solid fa-box menu-icon"></i>My Orders</a>
                    <a href="Security.aspx" class="menu-link"><i class="fa-solid fa-lock menu-icon"></i>Security</a>
                </div>

                <div class="security">
                    <div class="orders-top-nav">
                        <ul>
                            <li><asp:LinkButton ID="lnkAll" runat="server" OnClick="lnkFilter_Click" CommandArgument="All" CssClass='<%# currentFilter == "All" ? "filter-active" : "" %>'>All</asp:LinkButton></li>
                            <li><asp:LinkButton ID="lnkToShip" runat="server" OnClick="lnkFilter_Click" CommandArgument="ToShip" CssClass='<%# currentFilter == "ToShip" ? "filter-active" : "" %>'>To Ship</asp:LinkButton></li>
                            <li><asp:LinkButton ID="lnkToReceive" runat="server" OnClick="lnkFilter_Click" CommandArgument="ToReceive" CssClass='<%# currentFilter == "ToReceive" ? "filter-active" : "" %>'>To Receive</asp:LinkButton></li>
                            <li><asp:LinkButton ID="lnkCompleted" runat="server" OnClick="lnkFilter_Click" CommandArgument="Completed" CssClass='<%# currentFilter == "Completed" ? "filter-active" : "" %>'>Completed</asp:LinkButton></li>
                            <li><asp:LinkButton ID="lnkToRate" runat="server" OnClick="lnkFilter_Click" CommandArgument="ToRate" CssClass='<%# currentFilter == "ToRate" ? "filter-active" : "" %>'>To Rate</asp:LinkButton></li>
                        </ul>
                    </div>
                    
                    <div class="search-bar-container">
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="search-input" placeholder="You can search by Product name or Order ID"></asp:TextBox>
                        <asp:Button ID="btnSearch" runat="server" CssClass="search-button" OnClick="btnSearch_Click" Text="Search" />
                    </div>

                  <div class="orders-list">
    <asp:Repeater ID="rptOrderGroups" runat="server" OnItemDataBound="rptOrderGroups_ItemDataBound">
        <ItemTemplate>
            <div class="order-group">
                <div class="order-header">
                    <div>
                        <span class="OrderID">Order ID.<%# Eval("OrderID") %></span>
                    </div>
                    <div>
                        <span class="seller-name"><%# Eval("Status") %></span>
                    </div>
                </div>

        <asp:Repeater ID="rptOrderItems" runat="server">
    <ItemTemplate>
        <div class="order-item">
            <div class="order-body">
                <%-- Changed from ImageUrl to ImageURL to match database column name --%>
                <img src='<%# Eval("ImageURL") %>' alt="Product Image" class="product-img" />
                <div class="order-details">
                    <p class="product-name"><%# Eval("ProductName") %></p>
                    <p class="product-variation">Variation: <%# Eval("Size") %>, <%# Eval("Color") %> x<%# Eval("Quantity") %></p>
                </div>
            </div>
        </div>
    </ItemTemplate>
</asp:Repeater>
                
                            <div class="order-footer">
                                <span class="order-total">Order Total: RM<%# Eval("TotalAmount", "{0:F2}") %></span>
                                <div class="order-footer-actions">
                                    <asp:Panel runat="server" Visible='<%# IsOrderReceivableStatus(Eval("Status").ToString()) %>'>
                                        <asp:Button ID="btnReceived" runat="server" CssClass="order-action-btn received-btn" 
                                            Text="Order Received" CommandName="Received" CommandArgument='<%# Eval("OrderID") %>' 
                                            OnCommand="UpdateOrderStatus" />
                                    </asp:Panel>
                                    <asp:Panel runat="server" Visible='<%# Eval("Status").ToString() == "Completed" %>'>
                                        <asp:Button ID="btnRate" runat="server" CssClass="order-action-btn rate-btn" 
                                            Text="Rate Order" CommandName="Rate" CommandArgument='<%# Eval("OrderID") %>' 
                                            OnCommand="RateOrder" />
                                    </asp:Panel>
                                    <asp:HyperLink ID="lnkContact" runat="server" CssClass="order-action-btn contact-btn" 
                                        NavigateUrl="~/ContactUs.aspx">Contact Sellers</asp:HyperLink>
                                    <asp:HyperLink ID="lnkDetails" runat="server" CssClass="order-action-btn view-details-btn" 
                                        NavigateUrl='<%# "~/OrderDetailsA.aspx?OrderID=" + Eval("OrderID") %>'>View Details</asp:HyperLink>
                                </div>
                            </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>