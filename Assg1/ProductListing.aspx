<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductListing.aspx.cs" Inherits="Assg1.ProductListing" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <title>Flower Shop - Products</title>
    <style>
        body 
        {
            font-family: Arial, sans-serif;
            margin: 20px;
            background-repeat: no-repeat; 
            background-attachment: fixed; 
            background-position: center;
            background-size: cover;
        }
        h1 
        {
            text-align: center;
        }
        .filters-panel 
        {
            display: flex;
            justify-content: center; /* Centers the entire panel horizontally */
            align-items: center; /* Centers the content vertically */
            background-color: #009973;
            padding: 10px 20px;
            border-radius: 5px;
            margin: 20px auto;
            width: 100%; /* Ensures it spans the full width of the container */
            max-width: 1050px; /* Optional: Limits the maximum width */
            text-align: center; /* Ensures content inside is aligned */
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1); /* Adds a slight shadow for aesthetics */
        }

        .quick-filters 
        {
            display: flex;
            justify-content: center; /* Centers the dropdowns horizontally */
            align-items: center; /* Centers the dropdowns vertically */
            flex-wrap: wrap; /* Ensures responsiveness by wrapping items */
            gap: 15px; /* Adds spacing between elements */
            width: 100%; /* Ensures full-width alignment */
            max-width: 1000px; /* Optional: Controls the maximum width */
            color: white;
            font-family: Arial, sans-serif;
            font-size: 1.2em;
        }

        .filter-title 
        {
            display: flex;
            align-items: center;
            font-weight: bold;
            gap: 10px;
        }

        .filter-title i 
        {
            font-size: 1.5em;
        }

        .filter-selects 
        {
            display: flex;
            flex-wrap: wrap; /* Allows dropdowns to wrap on smaller screens */
            justify-content: center; /* Centers dropdowns */
            gap: 10px;
        }

        .styled-dropdown 
        {
            flex: 1;
            padding: 10px;
            font-size: 1em;
            border: 1px solid #ddd;
            border-radius: 5px;
            width: 200px;
            background-color: white;
            color: #333;
            appearance: none; 
            outline: none;
            box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.1);
            background-image: url('icons/drop arrow.jpg'); 
            background-repeat: no-repeat;
            background-position: right 10px center;
            background-size: 12px;
        }
        .styled-dropdown:first-child 
        {
            margin-right: 10px; 
        }

        .styled-dropdown:last-child 
        {
            margin-left: 10px;
        }

        .styled-dropdown:focus 
        {
            border-color: #4CAF50;
            box-shadow: 0 0 5px rgba(76, 175, 80, 0.5);
        }

        .styled-dropdown option 
        {
            padding: 10px;
        }
        .product-list 
        {
            display: grid;
            grid-template-columns: repeat(4, 1fr); /* Four columns for products */
            gap: 20px;
            margin: 20px auto;
            max-width: 1200px;
            padding: 10px;
        }
        .product-card 
        {
            border: 1px solid #ddd;
            border-radius: 10px;
            padding: 15px;
            background-color: #ffffff;
            text-align: center;
            box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.1);
            display: flex;
            flex-direction: column;
            align-items: center; /* Center content horizontally */
            justify-content: space-between; /* Evenly space content */
        }
        .product-image 
        {
            max-width: 100%;
            height: auto;
            border-radius: 5px;
        }
        .product-card img 
        {
            width: 100%; /* Ensure the image takes up the full width of its container */
            height: 200px; /* Set a fixed height for all images */
            object-fit: cover; /* Crop the image to fit the dimensions without distortion */
            border-radius: 5px; /* Optional: Add a slight border-radius for aesthetic */
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1); /* Optional: Add shadow for better visibility */
            transition: transform 0.2s ease-in-out; /* Optional: Add a hover effect */
        }

        .original-price 
        {
            text-decoration: line-through;
            color: #888;
        }
        .back-button 
        {
            margin: 20px 0;
            padding: 10px 20px;
            background-color: #00cc66;
            color: #fff;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 16px;
        }
        .back-button:hover 
        {
            background-color: #009973;
        }
        .product-card a.btn-primary {
            background-color: #5DB996; /* Set the background color */
            border-color: #5DB996; /* Set the border color */
            color: #fff; /* Set the text color */
            transition: background-color 0.3s ease-in-out; /* Smooth transition for hover effect */
        }

        .product-card a.btn-primary:hover {
            background-color: #4A956C; /* Slightly darker shade on hover */
            border-color: #4A956C; /* Update border on hover */
        }
        .filter-icon 
        {
            margin-right: 10px; 
        }

        .filter-icon img 
        {
            width: 40px; 
            height: 40px;
            border-radius: 50%; 
        }
        .product-price {
            font-size: 1.3em; /* Adjust this size as needed */
            font-weight: bold;
            color: #333; /* Choose a suitable color */
            margin: 10px 0; /* Optional: Add spacing around the price */
        }

        .original-price {
            font-size: 1em; /* Keep the original price smaller */
            font-weight: normal;
            color: #888;
            text-decoration: line-through;
            margin-left: 10px; /* Add spacing between DiscountPrice and OriginalPrice */
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
        <!-- Back Button -->
        <asp:Button ID="BackButton" runat="server" Text="Back" CssClass="back-button" OnClick="BackButton_Click" Visible="false" />

        <!-- Filters Section -->
        <asp:Panel ID="filtersPanel" runat="server" Visible="true">
            <div class="filters-panel">
                <div class="quick-filters">
                    <div class="filter-icon">
                        <asp:Image ID="imgBS" runat="server" ImageUrl="~/icons/green-flower logo.jpg" />
                    </div>
                    <div class="filter-title">
                        <i class="fa fa-gift"></i> QUICK FLOWER FINDER
                    </div>
                    <div class="filter-selects">
                        <asp:DropDownList ID="CategoryDropDown" runat="server" AutoPostBack="True" OnSelectedIndexChanged="FilterProducts" CssClass="styled-dropdown">
                            <asp:ListItem Text="Select An Occasion" Value="0"></asp:ListItem>
                        </asp:DropDownList>

                        <asp:DropDownList ID="PriceDropDown" runat="server" AutoPostBack="True" OnSelectedIndexChanged="FilterProducts" CssClass="styled-dropdown">
                            <asp:ListItem Text="Select A Budget" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Below RM100" Value="1"></asp:ListItem>
                            <asp:ListItem Text="RM100 - RM199" Value="2"></asp:ListItem>
                            <asp:ListItem Text="RM200 - RM299" Value="3"></asp:ListItem>
                        </asp:DropDownList>

                        <asp:DropDownList ID="ColorDropDown" runat="server" AutoPostBack="True" OnSelectedIndexChanged="FilterProducts" CssClass="styled-dropdown">
                            <asp:ListItem Text="All Colors" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Red" Value="Red"></asp:ListItem>
                            <asp:ListItem Text="Pink" Value="Pink"></asp:ListItem>
                            <asp:ListItem Text="White" Value="White"></asp:ListItem>
                            <asp:ListItem Text="Yellow" Value="Yellow"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <!-- Product List -->
        <div class="product-list">
            <asp:Repeater ID="ProductRepeater" runat="server">
                <ItemTemplate>
                    <div class="product-card">
                        <img src='<%# Eval("ImageURL") %>' alt='<%# Eval("ProductName") %>' />
                        <h4><%# Eval("ProductName") %></h4>
                        <p class="product-price">
                            Price: RM <%# Eval("DiscountPrice", "{0:0.00}") %> </br>
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