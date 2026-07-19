<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductDetails.aspx.cs" Inherits="Assg1.ProductDetails" MasterPageFile="~/Site1.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <title>Product Details</title>
    <style>
        .style body 
        {
            font-family: Arial, sans-serif;
            margin: 20px;
            background-repeat: no-repeat; 
            background-attachment: fixed; 
            background-position: center;
            background-size: cover;
        }
        .style .container 
        {
            display: flex;
            max-width: 1200px;
            margin: 0 auto;
            border: 1px solid #ddd;
            border-radius: 5px;
            background-color: #F7F9FA;
            padding: 20px;
            box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.1);
        }
        .style .image-container 
        {
            flex: 1;
            margin-right: 20px;
        }
        .style .image-container img 
        {
            max-width: 100%;
            height: auto;
            border-radius: 5px;
        }
        .style .details-container 
        {
            flex: 2;
            display: flex;
            flex-direction: column;
        }
        .style .product-name 
        {
            font-size: 2em;
            margin-bottom: 10px;
            color: #007BFF;
        }
        .style .product-price 
        {
            font-size: 1.5em;
            font-weight: bold;
            color: #28a745;
            margin-bottom: 20px;
        }
        .style .dropdown 
        {
            margin-bottom: 20px;
        }
        .style .dropdown label 
        {
            display: block;
            margin-bottom: 5px;
            font-weight: bold;
        }
        .style .dropdown select 
        {
            width: 100%;
            padding: 10px;
            font-size: 1em;
        }
        .style .description 
        {
            margin-bottom: 20px;
            line-height: 1.5em;
        }
        .style .description h3 
        {
            font-size: 1.2em;
            font-weight: bold;
            margin-top: 20px;
            margin-bottom: 10px;
            color: #333;
        }
        .style .description p 
        {
            margin: 0 0 10px;
            line-height: 1.6em;
            color: #555;
            font-size: 1em;
        }
        .style .quantity-container 
        {
            display: flex;
            align-items: center;
            gap: 10px;
        }
        .style .quantity-controls 
        {
            display: flex;
            align-items: center;
            gap: 5px;
        }
        .style .quantity-controls button 
        {
            width: 30px;
            height: 30px;
            font-size: 1.2em;
            border: 1px solid #ccc;
            background-color: #f8f9fa;
            cursor: pointer;
            border-radius: 5px;
            
        }
        .style .quantity-controls button:hover 
        {
            background-color: #e2e6ea;
        }
        .style .quantity-controls input 
        {
            width: 50px;
            text-align: center;
            border: 1px solid #ccc;
            border-radius: 5px;
            height: 30px;
            font-size: 1em;
        }
        .style .add-to-cart-btn 
        {
            padding: 15px;
            background-color: #28a745;
            color: white;
            border: none;
            border-radius: 5px;
            text-align: center;
            font-size: 1.2em;
            cursor: pointer;
            margin-top: auto;
        }
        .style .add-to-cart-btn:hover 
        {
            background-color: #218838;
        }
        .style .back-button 
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
        .style .back-button:hover 
        {
            background-color: #009973;
        }
        .style .product-stock 
        {
            font-size: 1.2em;
            color: #dc3545; /* Red to highlight low stock */
            margin-bottom: 20px;
        }
        .style .remaining-stock-container 
        {
            display: flex;
            align-items: center;
            gap: 10px;
            margin-bottom: 15px;
            font-family: Arial, sans-serif;
        }
        .style .stock-label 
        {
            font-size: 1.2em;
            font-weight: bold;
            color: #333;
        }
        .style .stock-count 
        {
            font-size: 1.2em;
            color: #000000;
            background: #ffffff;
            padding: 6px 10px;
            border-radius: 5px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
            text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.2);
            border: 1px solid #333;
            text-align: center;
            min-width: 80px; 
        }
        .style .size-options, .style .color-options 
        {
            display: flex;
            width: 100%;
            gap: 0;
            border: 1px solid #ccc;
            border-radius: 5px;
            overflow: hidden;
        }
        .style .size-option, .style .color-option 
        {
            flex: 1;
            padding: 10px;
            text-align: center;
            cursor: pointer;
            font-weight: bold;
            background-color: white;
            border: 1px solid black;
            color: black;
            transition: background-color 0.3s, color 0.3s;
        }
        .style .size-option:hover, .style .color-option:hover 
        {
            background-color: #e8f5e9;
        }
        .style .size-option.active, .style .color-option.active 
        {
            background-color: #00b386; /* Highlighted green */
            color: white;
        }
        .style .size-options, .style .color-options 
        {
            margin-bottom: 15px; /* Spacing between groups */
        }
        .style .option 
        {
            flex: 1;
            padding: 10px 20px;
            border: 2px solid #ccc;
            border-radius: 20px;
            text-align: center;
            cursor: pointer;
            font-weight: bold;
            background-color: #fff;
            color: #333;
            transition: background-color 0.3s, color 0.3s, border-color 0.3s;
        }
        .style .option.selected 
        {
            background-color: #28a745; /* Green for active state */
            color: #fff;
            border-color: #28a745;
        }
        .style .related-items-container 
        {
            width: 100%; 
            max-width: 1200px; 
            margin: 20px auto;
            padding: 20px; 
            border-radius: 10px;
        }
        .style .related-items-container h3 
        {
            font-size: 1.5em;
            font-weight: bold;
            margin-bottom: 5px; 
            color: #333;
            text-align: center;
            border-bottom: 2px solid #28a745;
            display: inline-block;
            padding-bottom: 5px;
        }
        .style .related-items-list 
        {
            display: grid;
            grid-template-columns: repeat(5, 1fr); 
            gap: 20px;
            justify-items: center; 
        }
        .style .related-items-list .product-card 
        {
            width: 100%; 
            max-width: 200px; 
            background-color: #F7F9FA;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            text-align: center;
            padding: 15px;
            transition: transform 0.3s;
        }
        .style .related-items-list .product-card:hover 
        {
            transform: scale(1.05);
            box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2);
        }
        .style .related-items-list .product-card img 
        {
            width: 100%;
            height: auto; /* Maintain image aspect ratio */
            max-height: 150px; /* Ensure consistent height for images */
            object-fit: cover; /* Crop the image properly */
            border-radius: 5px;
            transition: transform 0.3s ease;
        }
        .style .related-items-list .product-card h4 
        {
            font-size: 1.2em; /* Slightly larger font for visibility */
            margin: 10px 0;
            color: #007BFF;
            text-transform: capitalize;
            font-weight: bold;
        }
        .style .related-items-list .product-card p 
        {
            margin: 5px 0;
            font-size: 1em; /* Larger font size for clarity */
            color: #555;
        }
        .style .related-items-list .product-card p .original-price 
        {
            text-decoration: line-through;
            font-size: 0.9em;
            color: #888;
        }
        .style .related-items-list .product-card a 
        {
            display: inline-block;
            margin-top: 10px;
            padding: 10px 15px;
            background-color: #28a745;
            color: #FFFFFF;
            text-decoration: none;
            border-radius: 5px;
            font-size: 0.9em;
            font-weight: bold;
            transition: background-color 0.3s ease, box-shadow 0.3s ease;
        }
        .style .related-items-list .product-card a:hover 
        {
            background-color: #218838;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
        }
        .style .product-name 
        {
            color: black;
            font-size: 24px; /* Adjust the font size if necessary */
            font-weight: bold; /* Make it bold if desired */
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
        <div class ="style">
        <div style="margin-bottom: 20px;">
            <asp:Button ID="BackButton" runat="server" Text="Back" CssClass="back-button" OnClick="BackButton_Click" />
        </div>

        <div class="container">

            <!-- Image Section -->
            <div class="image-container">
                <img id="ProductImage" runat="server" alt="Product Image" src="placeholder.jpg" />
            </div>

            <!-- Details Section -->
            <div class="details-container">
                <!-- Product Name -->
                <asp:Label ID="ProductNameLabel" runat="server" class="product-name" Text="Red Roses Forever"></asp:Label>

                <!-- Product Price -->
                <p class="product-price">
                    Price: RM <asp:Label ID="ProductPriceLabel" runat="server" Text=""></asp:Label>
                </p>

                <!-- Size -->
                <div class="size-selection">
                    <label>Choose a Size:</label>
                    <div id="SizeOptions" runat="server" class="size-options">
                        <div class="size-option" onclick="selectOption(this, 'size')">Standard</div>
                        <div class="size-option" onclick="selectOption(this, 'size')">Premium</div>
                        <div class="size-option" onclick="selectOption(this, 'size')">Deluxe</div>
                    </div>
                    
                </div>
                <input type="hidden" id="SelectedSizeInput" name="SelectedSize" />
                <input type="hidden" id="SelectedPriceInput" name="SelectedPrice" />

                <!-- Color -->
                <div class="color-selection">
                    <label>Choose a Color:</label>
                    <div class="color-options">
                        <div class="color-option" onclick="selectOption(this, 'color')">Red</div>
                        <div class="color-option" onclick="selectOption(this, 'color')">Blue</div>
                        <div class="color-option" onclick="selectOption(this, 'color')">Green</div>
                    </div>
                </div>
                <input type="hidden" id="SelectedColorInput" name="SelectedColor" />


                <!-- Product Description -->
                <div class="description">
                    <h3>Description</h3>
                    <p id="ProductGeneralDescription" runat="server"></p>
    
                    <h3>SIZE</h3>
                    <p id="ProductSizeDescription" runat="server"></p>
    
                    <h3>BLOOMS</h3>
                    <p id="ProductBloomsDescription" runat="server"></p>
                </div>

                <!-- Product Stock -->
                <div class="remaining-stock-container">
                    <label class="stock-label">Remaining Stock:</label>
                    <span id="ProductStockDisplay" class="stock-count">
                        <asp:Label ID="ProductStockLabel" runat="server" Text="Loading..."></asp:Label>
                    </span>
                </div>

                <!-- Quantity -->
                <div class="quantity-container">
                    <label for="QuantityInput">Quantity:</label>
                    <div class="quantity-controls">
                        <button type="button" onclick="decreaseQuantity()" aria-label="Decrease Quantity">-</button>
                        <input id="QuantityInput" name="QuantityInput" type="text" value="1" readonly />
                        <button type="button" onclick="increaseQuantity()" aria-label="Increase Quantity">+</button>
                    </div>
                </div>
                <br/>

                <!-- Add to Cart Button -->
                <asp:Button ID="AddToCartButton" runat="server" Text="Add to Cart" CssClass="add-to-cart-btn" OnClick="AddToCart_Click" />
            </div>
        </div>

        <!-- Related Items Section -->
        <div class="related-items-container">
            <h2>Recommended Items</h2>
            <div class="related-items-list">
                <asp:Repeater ID="RelatedItemsRepeater" runat="server">
                    <ItemTemplate>
                        <div class="product-card">
                            <img src='<%# Eval("ImageURL") %>' alt='<%# Eval("ProductName") %>' class="product-image" />
                            <h4 style="color: black;"> <%# Eval("ProductName") %> </h4>
                            <p>Price: RM <%# Eval("DiscountPrice", "{0:0.00}") %>
                                <span class="original-price">RM <%# Eval("Price", "{0:0.00}") %></span>
                            </p>
                            <a href="ProductDetails.aspx?ProductID=<%# Eval("ProductID") %>">View Details</a>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
       </div>
    </form>
    <script>
        function selectOption(element, type) {
            const group = element.parentNode.children;

            for (let i = 0; i < group.length; i++) {
                group[i].classList.remove("active");
            }

            element.classList.add("active");

            if (type === 'color') {
                document.getElementById("SelectedColorInput").value = element.textContent.trim();
            } else if (type === 'size') {
                document.getElementById("SelectedSizeInput").value = element.textContent.trim();
            }
        }


        function increaseQuantity() {
            const quantityInput = document.getElementById("QuantityInput");
            let currentValue = parseInt(quantityInput.value);
            quantityInput.value = currentValue + 1;
        }

        function decreaseQuantity() {
            const quantityInput = document.getElementById("QuantityInput");
            let currentValue = parseInt(quantityInput.value);
            if (currentValue > 1) {
                quantityInput.value = currentValue - 1;
            }
        }

        function selectSizeOption(size, price, stock) {
            // Update active class for selected size
            const sizeOptions = document.querySelectorAll(".size-option");
            sizeOptions.forEach(option => option.classList.remove("active"));
            const selectedOption = [...sizeOptions].find(option => option.textContent.trim() === size);
            if (selectedOption) selectedOption.classList.add("active");

            // Update hidden inputs
            document.getElementById("SelectedSizeInput").value = size;
            document.getElementById("SelectedPriceInput").value = price;
            console.log(`Size: ${size}, Price: RM ${price.toFixed(2)}, Stock: ${stock}`);
            // Update displayed price and stock
            document.getElementById("<%= ProductPriceLabel.ClientID %>").innerText = price.toFixed(2);
            document.getElementById("<%= ProductStockLabel.ClientID %>").innerText = stock;
        }

    </script>
</asp:Content>