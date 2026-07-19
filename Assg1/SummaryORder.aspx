<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="SummaryOrder.aspx.cs" Inherits="Assg1.SummaryOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .thank-you-page {
            padding: 40px 0;
            background: #f8f9fa;
        }
        .thank-you-container {
            max-width: 800px;
            margin: 0 auto;
            padding: 30px;
            background: white;
            border-radius: 10px;
            box-shadow: 0 2px 5px rgba(0,0,0,0.1);
        }
        .thank-you-container h1 {
            color: #55BC88;
            margin-bottom: 20px;
        }
        .order-summary {
            margin: 30px 0;
            padding: 20px;
            background: #f8f9fa;
            border-radius: 5px;
        }
        .thank-you-actions {
            margin-top: 30px;
            text-align: center;
        }
        .button-row {
            display: flex;
            gap: 15px;
            justify-content: center;
            margin-top: 20px;
        }
        .btn {
            padding: 10px 25px;
            border-radius: 5px;
            text-decoration: none;
            font-weight: bold;
        }
        .btn-view-details {
            background-color: #55BC88;
            color: white;
        }
        .btn:hover {
            opacity: 0.9;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="thank-you-page">
        <div class="thank-you-container">
            <h1>Thank you for your order!</h1>
            <p class="email-confirmation">
                An email confirmation has been sent to <strong><asp:Label ID="lblEmail" runat="server" /></strong>
            </p>

            <div class="order-summary">
                <p><strong>Order number:</strong> <asp:Label ID="lblOrderID" runat="server" /></p>
                <p><strong>Order total:</strong> <asp:Label ID="lblTotalAmount" runat="server" /></p>
                <p><strong>Delivered by:</strong> <asp:Label ID="lblDeliveryDate" runat="server" />, <asp:Label ID="lblTimeSlot" runat="server" /></p>
                <p><strong>Recipient:</strong> <asp:Label ID="lblRecipient" runat="server" /></p>
                <p><strong>Delivered to:</strong> <asp:Label ID="lblAddress" runat="server" /></p>
            </div>

            <div class="thank-you-actions">
                <p class="need-help">
                    Need Help? <a href="#">Contact Us</a>
                </p>
                <div class="button-row">
                    <a href="ProductListing.aspx" class="btn btn-view-details">Continue Shopping</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>