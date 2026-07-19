<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CheckOutA.aspx.cs" Inherits="Assg1.CheckOutA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <!-- Bootstrap CSS -->
  <!-- Bootstrap CSS & JS -->
     <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet">
     <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
     <!-- Font Awesome -->
     <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" rel="stylesheet">
     <!-- Google Fonts -->
     <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@400;600&display=swap" rel="stylesheet">
     <!-- Custom CSS -->
     <link href="h1.css" rel="stylesheet">
 <!-- Custom CSS -->
 <link href="h1.css" rel="stylesheet">
     <script>
         // Function to toggle billing address visibility

         document.addEventListener('DOMContentLoaded', function () {
             // Initialize Bootstrap tabs
             if (typeof bootstrap !== 'undefined') {
                 var triggerTabList = [].slice.call(document.querySelectorAll('#paymentTabs button'));
                 triggerTabList.forEach(function (triggerEl) {
                     var tabTrigger = new bootstrap.Tab(triggerEl);
                     triggerEl.addEventListener('click', function (event) {
                         event.preventDefault();
                         tabTrigger.show();
                     });
                 });
             }

             // Setup billing address toggle
             const checkbox = document.getElementById('SameAsBilling');
             if (checkbox) {
                 checkbox.addEventListener('change', toggleBillingAddress);
                 // Initial check
                 toggleBillingAddress();
             }

             // Setup pay now button validation
             const payButton = document.getElementById('btnPayNow');
             if (payButton) {
                 payButton.addEventListener('click', function (e) {
                     if (!validateCheckoutForm()) {
                         e.preventDefault();
                         return false;
                     }
                     return true;
                 });
             }
         });

         function toggleBillingAddress() {
             const checkbox = document.getElementById('SameAsBilling');
             const billingSection = document.getElementById('billingSection');

             if (!checkbox || !billingSection) return;

             if (checkbox.checked) {
                 billingSection.style.display = 'none';
                 copyShippingToBilling();
             } else {
                 billingSection.style.display = 'block';
                 clearBillingFields();
             }
         }

         function copyShippingToBilling() {
             const fieldMappings = {
                 'ShippingFirstName': 'FirstName',
                 'ShippingLastName': 'LastName',
                 'ShippingAddress': 'Address',
                 'ShippingAddress2': 'Address2',
                 'ShippingCountry': 'Country',
                 'ShippingState': 'State',
                 'ShippingCode': 'Postal',
                 'phone1': 'phone'
             };

             for (const [shippingId, billingId] of Object.entries(fieldMappings)) {
                 const shippingElement = document.getElementById(shippingId);
                 const billingElement = document.getElementById(billingId);
                 if (shippingElement && billingElement) {
                     billingElement.value = shippingElement.value;
                 }
             }
         }

         function clearBillingFields() {
             const billingFields = ['FirstName', 'LastName', 'Address', 'Address2', 'Country', 'State', 'Postal', 'phone'];
             billingFields.forEach(fieldId => {
                 const element = document.getElementById(fieldId);
                 if (element) {
                     element.value = '';
                 }
             });
         }

         function validateCheckoutForm() {
             // Required fields validation
             const requiredFields = {
                 'ShippingFirstName': 'First Name',
                 'ShippingLastName': 'Last Name',
                 'ShippingAddress': 'Address',
                 'phone1': 'Phone Number',
                 'ShippingCode': 'Postal Code'
             };

             for (const [id, label] of Object.entries(requiredFields)) {
                 const element = document.getElementById(id);
                 if (!element || !element.value.trim()) {
                     alert(`Delivery ${label} is required`);
                     element?.focus();
                     return false;
                 }
             }

             // Country and State validation
             const countryElement = document.getElementById('ShippingCountry');
             const stateElement = document.getElementById('ShippingState');

             if (countryElement?.value === 'Choose...') {
                 alert('Please select a delivery country');
                 countryElement.focus();
                 return false;
             }

             if (stateElement?.value === 'Choose...') {
                 alert('Please select a delivery state');
                 stateElement.focus();
                 return false;
             }

             // Phone number validation
             const phoneElement = document.getElementById('phone1');
             if (phoneElement) {
                 const phoneRegex = /^01\d{8, 9}$/;
                 if (!phoneRegex.test(phoneElement.value.trim())) {
                     alert('Please enter a valid Malaysian phone number (e.g., 0123456789)');
                     phoneElement.focus();
                     return false;
                 }
             }

             // Postal code validation
             const postalElement = document.getElementById('ShippingCode');
             if (postalElement) {
                 const postalRegex = /^\d{5}$/;
                 if (!postalRegex.test(postalElement.value.trim())) {
                     alert('Please enter a valid Malaysian postal code (5 digits)');
                     postalElement.focus();
                     return false;
                 }
             }

             // Billing validation (if not same as shipping)
             if (!document.getElementById('SameAsBilling')?.checked) {
                 const billingFields = {
                     'FirstName': 'First Name',
                     'LastName': 'Last Name',
                     'Address': 'Address',
                     'phone': 'Phone Number',
                     'Postal': 'Postal Code'
                 };

                 for (const [id, label] of Object.entries(billingFields)) {
                     const element = document.getElementById(id);
                     if (!element || !element.value.trim()) {
                         alert(`Billing ${label} is required`);
                         element?.focus();
                         return false;
                     }
                 }

                 // Billing phone validation
                 const billingPhone = document.getElementById('phone')?.value.trim();
                 if (billingPhone && !(/^01\d{8, 9}$/).test(billingPhone)) {
                     alert('Please enter a valid Malaysian phone number for billing');
                     document.getElementById('phone')?.focus();
                     return false;
                 }

                 // Billing postal code validation
                 const billingPostal = document.getElementById('Postal')?.value.trim();
                 if (billingPostal && !(/^\d{5}$/).test(billingPostal)) {
                     alert('Please enter a valid Malaysian postal code for billing');
                     document.getElementById('Postal')?.focus();
                     return false;
                 }
             }

             return true;
         }

     </script>
 <style>
     .cart-item {
         display: flex;
         align-items: center;
         gap: 15px;
     }

     .cart-item img {
         width: 50px;
         height: 50px;
         object-fit: cover;
         border-radius: 5px;
     }

     .quantity-badge {
         position: absolute;
         top: -5px;
         right: -5px;
         background-color: #6c757d;
         color: white;
         border-radius: 50%;
         font-size: 12px;
         padding: 2px 6px;
     }

     .cart-summary {
         text-align: right;
     }

     .cart-summary .total {
         font-size: 1.25rem;
         font-weight: bold;
     }
 </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">

    <!-- Delivery and Checkout Section -->
       <div class="container mt-4">
        <div class="row">
            <!-- Cart Section -->
            <div class="col-md-4 order-2">
                <h4 class="d-flex justify-content-between align-items-center mb-3">
                    <span class="text-muted">Your Cart</span>
                    <span class="badge rounded-pill bg-secondary">3</span>
                </h4> 
                <ul class="list-group mb-3">
                                        <asp:Repeater ID="rptCartItems" runat="server">
                                            <ItemTemplate>
                                                <li class="list-group-item d-flex justify-content-between align-items-center">
                                                    <div class="cart-item">
                                                        <div class="position-relative">
                                                            <img src='<%# Eval("ImageURL") %>' alt='<%# Eval("ProductName") %>' class="img-thumbnail" style="width: 50px; height: 50px; object-fit: cover; border-radius: 5px;">
                                                            <span class="quantity-badge"><%# Eval("Quantity") %></span>
                                                        </div>
                                                        <div>
                                                            <h6 class="my-0"><%# Eval("ProductName") %></h6>
                                                            <small class="text-muted">RM <%# Eval("Price", "{0:0.00}") %></small>
                                                        </div>
                                                    </div>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>

                
                                        <div class="cart-summary mt-3">
                            <div class="d-flex justify-content-between">
                                <span>Subtotal</span>
                                <asp:Label ID="lblSubtotal" runat="server" Text="RM 0.00"></asp:Label>
                            </div>
                            <div class="d-flex justify-content-between">
                                <span>Shipping</span>
                                <span>RM 5.00</span>
                            </div>
                            <div class="d-flex justify-content-between total">
                                <span>Total</span>
                                <asp:Label ID="lblTotal" runat="server" Text="RM 0.00"></asp:Label>
                            </div>
                        </div>

            </div>

            

            <!-- Shipping Address Section -->
            <div class="col-md-8 order-1">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <h5 class="card-title">
                            <i class="fas fa-info-circle text-primary me-2"></i>Delivery Details
                        </h5>
                       <p class="mb-2">Delivery Date: <strong><asp:Label ID="lblDeliveryDate" runat="server" /></strong></p>
<p>Delivery Timeslot: <strong><asp:Label ID="lblTimeSlot" runat="server" /></strong></p>

                    </div>
                </div>
                <br>
                                  
                <h4 class="mb-3">Delivery:</h4>
              
                <div class="row">
                    <div class="col mb-4">
                        <label for="ShippingFirstName">First Name</label>
                        <input type="text" class="form-control" runat="server" id="ShippingFirstName" placeholder="First name">
                    </div>
                    <div class="col mb-4">
                        <label for="ShippingLastName">Last Name</label>
                        <input type="text" class="form-control" id="ShippingLastName" placeholder="Last name" runat="server">
                    </div>
                </div>
                <div class="mb-4">
                    <label for="ShippingAddress">Address</label>
                    <input type="text" class="form-control" runat="server" id="ShippingAddress" placeholder="1234 Main St">
                </div>
                <div class="mb-4">
                    <label for="ShippingAddress2">Address 2 (optional)</label>
                    <input type="text" class="form-control" runat="server" id="ShippingAddress2" placeholder="Apartment or suite">
                </div>
                <div class="row">
                    <div class="col">
                        <label for="ShippingCountry">Country</label>
                        <select class="form-select" id="ShippingCountry" runat="server">
                            <option selected>Choose...</option>
                            <option value="1">Malaysia</option>
                        </select>
                    </div>
                    <div class="col">
                        <label for="ShippingState">State</label>
                        <select class="form-select" id="ShippingState" runat="server">
                            <option selected>Choose...</option>
                            <option value="Kuala Lumpur">Kuala Lumpur</option>
                            <option value="Selangor">Selangor</option>
                        </select>
                    </div>
                    <div class="col mb-4">
                        <label for="ShippingCode">Postal Code</label>
                        <input type="text" class="form-control" runat="server" id="ShippingCode" placeholder="Postal code" />
                    </div>
                </div>
                                  <div class="row">
                    <div class="mb-4">
    <label for="phone1">Phone Number</label>
    <input type="text" class="form-control" runat="server" id="phone1" placeholder="01123872151">
</div>
                      </div>

                <div class="container mt-4">
    <div class="row">
        <!-- Payment Section -->
        <div class="col-12">
            <h4 class="mb-3">Payment:</h4>
            <p class="text-muted">All transactions are secure and encrypted.</p>
            <div class="card shadow-sm p-4">
                <!-- Payment Method Tabs -->
                <ul class="nav nav-tabs mb-3" id="paymentTabs" role="tablist">
                    <li class="nav-item" role="presentation">
                        <button class="nav-link active" id="credit-tab" data-bs-toggle="tab" data-bs-target="#credit" type="button" role="tab" aria-controls="credit" aria-selected="true">
                            Credit Card / Debit Card
                            <img src="https://img.icons8.com/color/48/visa.png" alt="Visa" height="20" class="ms-2">
                            <img src="https://img.icons8.com/color/48/mastercard.png" alt="MasterCard" height="20">
                        </button>
                    </li>
                    <li class="nav-item" role="presentation">
<button class="nav-link" id="other-tab" data-bs-toggle="tab" data-bs-target="#other" type="button" role="tab" aria-controls="other" aria-selected="false">
    Curlec (FPX, Wallets, Cards and more)
    <img src="https://vectorise.net/logo/wp-content/uploads/2019/09/Logo-FPX.png" alt="FPX" height="20" class="ms-2">
    <i class="fas fa-wallet ms-2" style="font-size: 20px;"></i>
    <span class="badge bg-secondary ms-2"></span>
</button>



                    </li>
                </ul>


                <!-- Tab Content -->
                <div class="tab-content" id="paymentTabsContent">
                    <!-- Credit/Debit Card Tab -->
                    <div class="tab-pane fade show active" id="credit" role="tabpanel" aria-labelledby="credit-tab">
                        
                            <div class="mb-3">
                                <label for="cardNumber" class="form-label">Card Number</label>
                                <div class="input-group">
                                    <input type="text" class="form-control" id="cardNumber" placeholder="Card number">
                                    <span class="input-group-text">
                                        <i class="fas fa-lock"></i>
                                    </span>
                                </div>
                            </div>
                            <div class="row g-2">
                                <div class="col-md-6">
                                    <label for="expiryDate" class="form-label">Expiration Date (MM / YY)</label>
                                    <input type="text" class="form-control" id="expiryDate" placeholder="MM / YY">
                                </div>
                                <div class="col-md-6">
                                    <label for="securityCode" class="form-label">Security Code</label>
                                    <input type="text" class="form-control" id="securityCode" placeholder="CVV">
                                </div>
                            </div>
                            <div class="mb-3">
                                <label for="cardName" class="form-label">Name on Card</label>
                                <input type="text" class="form-control" id="cardName" placeholder="Name on card">
                            </div>
                                          <div class="form-check mb-3">
    <input class="form-check-input" type="checkbox" id="SameAsBilling">
    <label class="form-check-label" for="SameAsBilling">Billing address Same as shipping address.</label>
</div>
                       
                    </div>


                    <!-- Other Payment Methods Tab -->
                    <div class="tab-pane fade" id="other" role="tabpanel" aria-labelledby="other-tab">
                        <div class="text-center py-4">
                            <!-- Illustrative Image -->
                           <i class="fas fa-arrow-right" style="font-size: 24px;"></i>

                            <!-- Description Text -->
                            <p>
                                After clicking <strong>“Pay now”</strong>, you will be redirected to Curlec (FPX, Wallets, Cards, and more)
                                to complete your purchase securely.
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
                 <br>
                  
                <!-- Billing Address Section -->
                <div id="billingSection">
                    <h4 class="mb-3">Billing Address:</h4>
                    <div class="row">
                        <div class="col mb-4">
                            <label for="FirstName">First Name</label>
                            <input type="text" class="form-control" id="FirstName" placeholder="First name">
                        </div>
                        <div class="col mb-4">
                            <label for="LastName">Last Name</label>
                            <input type="text" class="form-control" id="LastName" placeholder="Last name" runat="server">
                        </div>
                    </div>
                    <div class="mb-4">
                        <label for="Address">Address</label>
                        <input type="text" class="form-control" id="Address" placeholder="1234 Main St" runat="server">
                    </div>
                    <div class="mb-4">
                        <label for="Address2">Address 2 (optional)</label>
                        <input type="text" class="form-control" id="Address2" placeholder="Apartment or suite" runat="server">
                    </div>
                    <div class="row">
                        <div class="col">
                            <label for="Country">Country</label>
                            <select class="form-select" id="Country" runat="server">
                                <option selected>Choose...</option>
                                <option value="1">Malaysia</option>

                            </select>
                        </div>
                        <div class="col">
                            <label for="State">State</label>
                            <select class="form-select" id="State" runat="server">
                              <option selected>Choose...</option>
                                <option value="1">Kuala Lumpur</option>
                                <option value="2">Selangor</option>
                            </select>
                        </div>
                        <div class="col mb-4">
                            <label for="Postal">Postal Code</label>
                            <input type="text" class="form-control" id="Postal" placeholder="Postal code" runat="server">
                        </div>
                    </div>
                    <div class="row">
                      <div class="mb-4">
      <label for="phone">Phone Number</label>
      <input type="text" class="form-control" id="phone" placeholder="01123872151" runat="server">
  </div>
                        </div>
                </div>
                 <div class="d-grid gap-2">
   <asp:Button 
    ID="btnPayNow" 
    runat="server" 
    Text="Pay Now" 
    OnClick="btnPayNow_Click"
    CssClass="btn btn-lg" 
    Style="background-color: #5DB996; color: white;" />

            </div>
        </div>
    </div>

       </div>
    <br/> <br/> <br/>
    </form>
</asp:Content>