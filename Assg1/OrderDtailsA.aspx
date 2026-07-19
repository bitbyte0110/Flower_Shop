<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderDtailsA.aspx.cs" Inherits="flower.OrderDtailsA" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Order Page</title>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.7.1/css/all.min.css" integrity="sha512-5Hs3dF2AEPkpNAR7UiOHba+lRSJNeM2ECkwxUIxC1Q/FLycGTbNapWXB4tP889k5T5Ju8fs4b1P5z/iB4nMfSQ==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    <link href="Styles/Security.css" rel="stylesheet" type="text/css" />
  


     <!-- Bootstrap CSS -->
 <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet"/>
    <style>
        /* General Styles */
       body {
    background-color: #f8fafc;
    font-family: "Poppins", sans-serif;
}
     
 .container,
  .row,
  .col {
      width: 1200px !important;
      max-width: 1200px !important;
      margin-left: auto !important;
      margin-right: auto !important;
  }

  /* Optionally, you can disable Bootstrap’s breakpoints:
     for example: force the same layout for all screens */
  @media (max-width: 767px),
         (max-width: 991px),
         (max-width: 1199px) {
      .container,
      .row,
      .col {
          width: 1200px !important;
          max-width: 1200px !important;
      }
  }
.container {
    display: flex;
    margin: 50px 20px;
}

.menu {
    margin: 0 20px;
    width: 300px;
}

.menu-link {
    display: block;
    text-decoration: none;
    color: #768499;
    padding: 10px 20px;
    margin: 10px 0;
    border-radius: 10px;
    font-size: 14px;
}

.menu-link:nth-child(2) {
    color: #fff;
    background-color: #5DB996;
}

.menu-link:hover {
    background-color: #f1f5f9;
    color: #1e293b;
}

.menu-link:nth-child(2):hover {
    background-color: #5DB996;
    color: #fff;
}

.menu-icon {
    margin-right: 10px;
}

.security {
    flex-grow: 1;
    background-color: #fff;
    box-shadow: rgba(0, 0, 0, 0.16) 0px 1px 4px;
    border-radius: 5px;
    padding: 20px;
}

/* Progress Tracker */
.progress-tracker {
    display: flex;
    align-items: center;
    position: relative;
    justify-content: space-between;
    margin: 30px 0;
}

.tracker-step {
    position: relative;
    text-align: center;
    z-index: 1; /* Keeps steps above the lines */
    flex: none; /* Steps take only the space they need */
}

.tracker-step .circle {
    width: 50px;
    height: 50px;
    border-radius: 50%;
    background-color: #e0e0e0; /* Default gray for incomplete steps */
    display: flex;
    align-items: center;
    justify-content: center;
    color: white;
    font-size: 18px;
    margin: 0 auto;
}

.tracker-step.completed .circle {
    background-color: #5DB996; /* Green for completed steps */
}

.tracker-step p {
    margin-top: 10px;
    font-size: 14px;
    color: #555;
}

/* Lines between steps */
/* Common tracker line style */
/* Common tracker line style */
/* General tracker line style */
.tracker-line {
    position: absolute;
    height: 4px;
    background-color: #e0e0e0; /* Default gray */
    z-index: 0;
    top: 30%; /* Align with the center of circles */
    transform: translateY(-50%);
    width: 200px; /* Increased width for longer lines */
    padding-right:10px;
}

/* Line 1: Between step1 and step2 */
#line1 {
    left: calc(2% + 25px); /* Align with step1 and step2 */
}

/* Line 2: Between step2 and step3 */
#line2 {
    left: calc(25% + 25px); /* Align with step2 and step3 */
}

/* Line 3: Between step3 and step4 */
#line3 {
    left: calc(49% + 25px); /* Align with step3 and step4 */
}

/* Line 4: Between step4 and step5 */
#line4 {
    left: calc(72% + 25px); /* Align with step4 and step5 */
}

/* Completed Line */
.tracker-line.completed {
    background-color: #5DB996; /* Green for completed lines */
}


/* Completed Line */
.tracker-line.completed {
    background-color: #5DB996; /* Green for completed lines */
}


/* Step Circle */
.tracker-step .circle {
    width: 50px;
    height: 50px;
    border-radius: 50%;
    background-color: #e0e0e0; /* Default gray */
    display: flex;
    align-items: center;
    justify-content: center;
    color: white;
    font-size: 18px;
    margin: 0 auto;
}

.tracker-step.completed .circle {
    background-color: #5DB996; /* Green for completed steps */
}

/* Step Text */
.tracker-step p {
    margin-top: 10px;
    font-size: 14px;
    color: #555;
    text-align: center;
}


/* Buttons */
.order-actions {
    display: flex;
    gap: 10px;
    margin: 20px 0;
    justify-content: flex-end
  
}

.received-btn {
    background-color: #5DB996;
    color: white;
    border: none;
    padding: 10px 20px;
    border-radius: 5px;
    font-size: 14px;
    cursor: pointer;
}

.contact-btn {
    background-color: white;
    color: black;
    border: 1px solid #ddd;
    padding: 10px 20px;
    border-radius: 5px;
    font-size: 14px;
    cursor: pointer;
}

.contact-btn:hover, .received-btn:hover {
    opacity: 0.8;
}

/* Delivery Details */
.delivery-details {
    margin-top: 20px;
}

.delivery-details h3, .order-details h3 {
    font-size: 18px;
    color: #333;
    margin-bottom: 10px;
}

.delivery-details ul {
    padding-left: 20px;
    margin-top: 10px;
}

.delivery-details ul li {
    font-size: 14px;
    color: #555;
    margin-bottom: 5px;
}

/* Order Details */
.order-item {
    display: flex;
    gap: 15px;
    margin-bottom: 20px;
}

.order-img {
    width: 100px;
    height: 100px;
    border-radius: 5px;
    object-fit: cover;
}


.order-info p {
    margin: 4px 0; /* Less vertical space */
    line-height: 1.4; /* Adjust line height */
    font-size: 14px; /* Optional: Smaller font size */
}

.order-item {
    display: flex;
    gap: 10px; /* Adjusts spacing between the image and text */
}

.order-img {
    width: 100px; /* Ensures the image size remains consistent */
    height: auto;
    border-radius: 5px;
}


.product-name {
    font-size: 16px;
    font-weight: bold;
    margin-bottom: 5px;
}

.order-summary {
    width: 100%;
    margin-top: 10px;
    border-collapse: collapse;
}

.order-summary td, .order-summary th {
    padding: 10px;
    font-size: 14px;
}

.order-summary th {
    text-align: left;
}

/* Delivery Container */
.delivery-container {
    display: flex;
    justify-content: space-between;
    gap: 20px;
    margin-top: 20px;
}

.delivery-address, .delivery-status {
    flex: 1;
    background: #f8f9fa;
    padding: 20px;
    border-radius: 5px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.delivery-address h3, .delivery-status h3 {
    font-size: 18px;
    margin-bottom: 10px;
    color: #333;
}

.delivery-address p, .delivery-status p {
    margin: 5px 0;
    font-size: 14px;
    color: #555;
}


    </style>
</head>
<body>
    <form id="form1" runat="server">
        
        <div class="container">
        <div class="menu">
      <a href="Account.aspx" class="menu-link"><i class="fa-solid fa-circle-user menu-icon"></i>Account</a>
      <a href="OrderListA.aspx" class="menu-link"><i class="fa-solid fa-box menu-icon"></i>My Orders</a>
      <a href="Security.aspx" class="menu-link"><i class="fa-solid fa-lock menu-icon"></i>Security</a>
  </div>

            <div class="security">
            <div class="order-details-view">
               
        <div class="d-flex justify-content-between align-items-center mb-4">
    <!-- Back Button -->
    <button class="btn btn-secondary" onclick="history.back()">
        <i class="fa-solid fa-arrow-left"></i> Back
    </button>
    <!-- Order ID -->
   <h5 class="text-end m-0">Order ID: <span class="text-primary"><asp:Label ID="lblOrderID" runat="server"></asp:Label></span></h5>

</div>

    <!-- Progress Tracker -->
<div class="progress-tracker">
    <div id="step1" runat="server" class="tracker-step">
        <div class="circle"><i class="fa-solid fa-cart-shopping"></i></div>
        <p>Order Placed</p>
    </div>
    <div id="line1" runat="server" class="tracker-line"></div> <!-- Line between step1 and step2 -->
    <div id="step2" runat="server" class="tracker-step">
        <div class="circle"><i class="fa-solid fa-box"></i></div>
        <p>To Ship</p>
    </div>
    <div id="line2" runat="server" class="tracker-line"></div> <!-- Line between step2 and step3 -->
    <div id="step3" runat="server" class="tracker-step">
        <div class="circle"><i class="fa-solid fa-truck"></i></div>
        <p>Order Shipped Out</p>
    </div>
    <div id="line3" runat="server" class="tracker-line"></div> <!-- Line between step3 and step4 -->
    <div id="step4" runat="server" class="tracker-step">
        <div class="circle"><i class="fa-solid fa-envelope-open-text"></i></div>
        <p>Order Received</p>
    </div>
    <div id="line4" runat="server" class="tracker-line"></div> <!-- Line between step4 and step5 -->
    <div id="step5" runat="server" class="tracker-step">
        <div class="circle"><i class="fa-solid fa-star"></i></div>
        <p>Rate</p>
    </div>
</div>



    <!-- Delivery Details -->
   <div class="delivery-container">
    <!-- Delivery Address -->
 <div class="delivery-address">
    <h3>Delivery Address</h3>
    <p><strong>Recipient:</strong> <asp:Label ID="lblRecipient" runat="server"></asp:Label></p>
    <p><strong>Phone:</strong> <asp:Label ID="lblPhone" runat="server"></asp:Label></p>
    <p><strong>Address:</strong> <asp:Label ID="lblAddress" runat="server"></asp:Label></p>
</div>


    <!-- Delivery Status -->
    <div class="delivery-status">
        <h3>Delivery Status</h3>
        <p><strong>SPX Express (West Malaysia)</strong></p>
        <p>Tracking Number: <strong>SPXMY04482646165C</strong></p>

        <ul class="tracking-details">
            <li>
                <span class="status-icon completed"></span>
                <span><strong>Today 15:52:</strong> Preparing to ship</span>
            </li>

        </ul>
    </div>
</div>


    <!-- Order Details -->
                <br/> 
    <div class="order-details">
        <h3>Order Details:</h3>
       <asp:Repeater ID="rptOrderItems" runat="server">
    <ItemTemplate>
        <div class="order-item">
            <!-- Product Image -->
            <img src='<%# Eval("ImageURL") %>' alt="Product Image" class="order-img" />

            <div class="order-info">
                <!-- Product Name -->
                <p class="product-name"><%# Eval("ProductName") %></p>
                <!-- Variation (Size and Color) -->
                <p>Variation: <%# Eval("ProductSize") %>, <%# Eval("ProductColor") %></p>
                <!-- Quantity -->
                <p>Quantity: <%# Eval("Quantity") %></p>
                <!-- Unit Price -->
                <p>Unit Price: RM <%# Eval("Price", "{0:F2}") %></p>
            </div>
        </div>
    </ItemTemplate>
</asp:Repeater>



        <table class="order-summary">
    <tr>
        <td>Merchandise Subtotal</td>
        <!-- Label for Subtotal -->
        <td style="text-align: right;"><asp:Label ID="lblSubTotal" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td>Shipping Fee</td>
        <!-- Label for Shipping Fee -->
        <td style="text-align: right;"><asp:Label ID="lblShippingFee" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <th>Total</th>
        <!-- Label for the total -->
        <th style="text-align: right;"><asp:Label ID="lblTotal" runat="server"></asp:Label></th>
    </tr>
   
</table>

    </div>
</div>
 <!-- Action Buttons -->
 <div class="order-actions">
     <button class="received-btn">Order Received</button>
     <button class="contact-btn">Contact Us</button>
 </div>
                </div>
            </div>
             
    </form>
</body>
</html>