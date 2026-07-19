<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="HOME.aspx.cs" Inherits="Assg1.HOME" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="h1.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
         
<section class="hero-section" style="background-image: url('../photo/pexels-emma-bauso-1183828-2253898.jpg');">
    <div class="hero-content text-center text-white">
        <h1>Express Your Feelings with Beautiful Blooms.</h1>
        <p class="description">Fresh flowers, delivered with love to brighten every moment.</p>
        <div class="hero-buttons">
            <a href="ProductListing.aspx" class="btn btn-primary">Flower Collection</a>
            
        </div>
    </div>
</section>


<section class="features-section py-4 bg-light" >
    <div class="container" >
        <div class="row text-center">
            <!-- Fast Shipping -->
            <div class="col-md-4">
                <div class="feature-item">
                    <i class="fas fa-truck text-primary mb-2" style="font-size: 2rem;"></i>
                    <h5 class="feature-title">Fast Shipping</h5>
                    <p class="feature-description">Quick Delivery, Every Order</p>
                </div>
            </div>

            <!-- 24/7 Customer Service -->
            <div class="col-md-4">
                <div class="feature-item">
                    <i class="fas fa-headset text-primary mb-2" style="font-size: 2rem;"></i>
                    <h5 class="feature-title">24/7 Support</h5>
                    <p class="feature-description">Get support all day</p>
                </div>
            </div>

            <!-- Stays Fresh for At Least 7 Days -->
            <div class="col-md-4">
                <div class="feature-item">
                    <i class="fas fa-seedling text-primary mb-2" style="font-size: 2rem;"></i>
                    <h5 class="feature-title">Stay Fresh For 7 Days</h5>
                    <p class="feature-description">Guaranteed freshness</p>
                </div>
            </div>
        </div>
    </div>
</section>
      <!-- container -->
   <section class="categories">
    <div class="container">
        <h2>Choose Your Perfect Bouquet</h2>
        <div class="category-grid">
            <div class="category-card">
                <asp:Image ID="flowerBirthday" runat="server" ImageUrl="~/Images/flower-birthday.jpg" />
                <h3>Birthday</h3>
                <a href="ProductListing.aspx" class="btn-shop-now">Shop Now</a>
            </div>
            <div class="category-card">
                <asp:Image ID="flowerAnniversary" runat="server" ImageUrl="~/Images/royal red.jpg" />
                <h3>Romance</h3>
                <a href="ProductListing.aspx" class="btn-shop-now">Shop Now</a>
            </div>
            <div class="category-card">
                <asp:Image ID="flowerSympathy" runat="server" ImageUrl="~/Images/sweet pea.jpg" />
                <h3>Christmas</h3>
                <a href="ProductListing.aspx" class="btn-shop-now">Shop Now</a>
            </div>
        </div>
    </div>
</section>

            <!-- New Promotional Section -->
<section class="hero-section" style="background-image: url('../photo/pexels-nadine-wuchenauer-695910-1563650.jpg'); background-color: rgba(0, 0, 0, 0);padding: 240px 0">
    <div class="hero-content text-left text-white" style="margin-left: 15%; max-width: 600px; position: absolute; top: 50%; transform: translateY(-50%); ">
        
        <h1>UPTO 40% OFF!</h1>
        <p class="description">Fresh flowers to celebrate every moment.</p>
        <div class="hero-buttons">
            <a href="ProductListing.aspx" class="btn btn-primary">Shop Now</a>
            
        </div>
    </div>
</section>
            <section class="testimonials" aria-labelledby="testimonials-heading" style="background-color: #faf3dd;">
                <div class="container">
                    <h2 id="testimonials-heading">What Our Customers Say</h2>
                    <div class="testimonial-slider" role="region" aria-label="Customer Testimonials">
                        <div class="testimonial-card">
                            <div class="testimonial-rating" aria-label="5 out of 5 stars">
                                <span class="star active">★</span>
                                <span class="star active">★</span>
                                <span class="star active">★</span>
                                <span class="star active">★</span>
                                <span class="star active">★</span>
                            </div>

                            <p
                               itemprop="reviewBody"
                                class="testimonial-text">
                                "Beautiful flowers, delivered on time and exactly as pictured! The bouquet was fresh and stunning."  
                            </p>

                            <div class="customer-info" itemprop="author">
                                <asp:Image ID="customerAvatar1" runat="server" ImageUrl="~/Images/customer-avatar1.png" AlternateText="Sarah M." CssClass="customer-avatar" />
                                <div class="customer-details">
                                    <span
                                        itemprop="name"
                                        class="customer-name">Sarah M.  
                                    </span>
                                    <span class="customer-badge verified-buyer">Verified Buyer  
                                    </span>
                                    <small
                                        class="review-date"
                                        itemprop="datePublished">Reviewed on January 15, 2024  
                                    </small>
                                </div>
                            </div>

                            <div class="product-context" itemprop="itemReviewed">
                                <meta itemprop="name" content="Rose Bouquet Deluxe" />
                                <meta itemprop="description" content="Luxurious rose arrangement" />
                            </div>
                        </div>

                        <div class="testimonial-card">
                            <div class="testimonial-rating" aria-label="5 out of 5 stars">
                                <span class="star active">★</span>
                                <span class="star active">★</span>
                                <span class="star active">★</span>
                                <span class="star active">★</span>
                                <span class="star active">★</span>
                            </div>

                            <p
                                itemprop="reviewBody"
                                class="testimonial-text">
                                "I was impressed by the quality and the care taken in packaging. Will definitely order again for special occasions!"  
                            </p>

                            <div class="customer-info" itemprop="author">
                                <asp:Image ID="customerAvatar2" runat="server" ImageUrl="~/Images/customer-avatar2.png" AlternateText="Michael T." CssClass="customer-avatar" />
                                <div class="customer-details">
                                    <span
                                        itemprop="name"
                                        class="customer-name">Michael T.  
                                    </span>
                                    <span class="customer-badge verified-buyer">Verified Buyer  
                                    </span>
                                    <small
                                        class="review-date"
                                        itemprop="datePublished">Reviewed on February 3, 2024  
                                    </small>
                                </div>
                            </div>

                            <div class="product-context" itemprop="itemReviewed">
                                <meta itemprop="name" content="Anniversary Flower Package" />
                                <meta itemprop="description" content="Elegant anniversary flower arrangement" />
                            </div>
                        </div>

                        <div class="testimonial-card">
                            <div class="testimonial-rating" aria-label="5 out of 5 stars">
                                <span class="star active">★</span>
                                <span class="star active">★</span>
                                <span class="star active">★</span>
                                <span class="star active">★</span>
                                <span class="star active">★</span>
                            </div>

                            <p
                                itemprop="reviewBody"
                                class="testimonial-text">
                                "Amazing service and such beautiful arrangements. The surprise flower delivery made my wife's day!"  
                            </p>

                            <div
                                class="customer-info" itemprop="author">
                                <asp:Image ID="customerAvatar3" runat="server" ImageUrl="~/Images/customer-avatar3.png" AlternateText="David R." CssClass="customer-avatar" />
                                <div class="customer-details">
                                    <span
                                        itemprop="name"
                                        class="customer-name">David R.  
                                    </span>
                                    <span class="customer-badge verified-buyer">Verified Buyer  
                                    </span>
                                    <small
                                        class="review-date"
                                        itemprop="datePublished">Reviewed on March 10, 2024  
                                    </small>
                                </div>
                            </div>

                            <div class="product-context" itemprop="itemReviewed">
                                <meta itemprop="name" content="Surprise Romance Bouquet" />
                                <meta itemprop="description" content="Romantic surprise flower delivery" />
                            </div>
                        </div>
                    </div>
                </div>
            </section>

</asp:Content>
