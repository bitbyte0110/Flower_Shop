<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Graduation.aspx.cs" Inherits="Assg1.Graduation" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Graduation Products</title>
    <style>
        .product-list {
            display: grid;
            grid-template-columns: repeat(4, 1fr);
            gap: 20px;
            margin: 20px auto;
            max-width: 1200px;
            padding: 10px;
        }

        .product-card {
            border: 1px solid #ddd;
            border-radius: 10px;
            padding: 15px;
            background-color: #ffffff;
            text-align: center;
            box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.1);
        }

        .product-card img {
            width: 100%;
            height: 200px;
            object-fit: cover;
            border-radius: 5px;
            margin-bottom: 10px;
        }

        .product-card h4 {
            font-size: 1.2em;
            margin: 10px 0;
        }

        .product-price {
            font-size: 1.1em;
            font-weight: bold;
            color: #333;
            margin: 5px 0;
        }

        .original-price {
            text-decoration: line-through;
            color: #888;
            font-size: 0.9em;
        }

        .back-button {
            margin: 20px 0;
            padding: 10px 20px;
            background-color: #5DB996;
            color: #fff;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 16px;
            text-decoration: none;
        }

        .back-button:hover {
            background-color: #4A956C;
            color: #fff;
        }

        .product-card a.btn-primary {
            background-color: #5DB996;
            border-color: #5DB996;
            color: #fff;
            transition: background-color 0.3s ease-in-out;
        }

        .product-card a.btn-primary:hover {
            background-color: #4A956C;
            border-color: #4A956C;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
        <!-- Back Button -->
        <asp:Button ID="BackButton" runat="server" Text="Back" CssClass="back-button" OnClick="BackButton_Click" />

        <h1 class="text-center mt-4">Graduation Flowers</h1>
        <!-- Product List -->
        <div class="product-list">
            <asp:Repeater ID="ProductRepeater" runat="server">
                <ItemTemplate>
                    <div class="product-card">
                        <img src='<%# Eval("ImageURL") %>' alt='<%# Eval("ProductName") %>' />
                        <h4><%# Eval("ProductName") %></h4>
                        <p class="product-price">
                            Price: RM <%# Eval("DiscountPrice", "{0:0.00}") %>
                            <span class="original-price">RM <%# Eval("Price", "{0:0.00}") %></span>
                        </p>
                        <p>Rating: <%# Eval("Rating") %></p>
                        <a href="ProductDetails.aspx?ProductID=<%# Eval("ProductID") %>" class="btn btn-primary">View Details</a>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </form>
</asp:Content>
