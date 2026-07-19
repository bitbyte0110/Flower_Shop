<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="Assg1.AdminDashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Dashboard</title>
    <link href="style/AdminDashboard.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.7.1/css/all.min.css" integrity="sha512-5Hs3dF2AEPkpNAR7UiOHba+lRSJNeM2ECkwxUIxC1Q/FLycGTbNapWXB4tP889k5T5Ju8fs4b1P5z/iB4nMfSQ==" crossorigin="anonymous" referrerpolicy="no-referrer" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">  
            <!-- Sidebar -->
            <div class="sidebar">  
                <div class="logo-container">  
                    <i class="fas fa-flower"></i>  
                    <h2>Admin Menu</h2>  
                </div>  
                <ul>  
                    <li class="active">  
                        <a href="#"><i class="fas fa-home"></i> Dashboard</a>  
                    </li>  
                    <li>  
                        <a href="AdminProduct.aspx"><i class="fas fa-box"></i> Product Details</a>  
                    </li>  
                    <li>  
                        <a href="AdminSalesReport.aspx"><i class="fas fa-chart-line"></i> Sales Report</a>  
                    </li>  
                    <li>
                        <a href="AdminOrderList.aspx"><i class="fas fa-list"></i> Order List</a>
                    </li>
                    <li>
                        <a href="LockedAccountsManagement.aspx"><i class="fas fa-lock-open"></i> Unlock Account</a>
                    </li>
                </ul>  
            </div>  

            <!-- Main Content -->
            <div class="main-content">  
                <div class="top-bar">  
                    <div class="page-title">  
                        <h1>Dashboard</h1>  
                    </div>  
                    <div class="user-actions">  
                        <div class="notifications">  
                            <i class="fas fa-bell"></i>  
                            <span class="badge">3</span>  
                        </div>  
                        <div class="user-profile">  
                            <asp:Image ID="admin" runat="server" ImageUrl="~/Images/admin.jpg" /> 
                            <span>Admin</span>  
                        </div>  
                    </div>  
                </div>  

                <!-- Dashboard Summary -->
                <div class="dashboard-summary">  
                    <h2>Overview</h2>  
                    <div class="summary-cards-container">  
                        <div class="summary-card" onclick="location.href='AdminProduct.aspx';" style="cursor: pointer;">  
                            <div class="card-icon">  
                                <i class="fas fa-box-open"></i>  
                            </div>  
                            <div class="card-content">  
                                <h3>Total Products</h3>  
                                <p>20</p>  
                            </div>  
                        </div>  
                        <div class="summary-card" onclick="location.href='AdminSalesReport.aspx';" style="cursor: pointer;">  
                            <div class="card-icon">  
                                <i class="fas fa-dollar-sign"></i>  
                            </div>  
                            <div class="card-content">  
                                <h3>Total Sales</h3>  
                                <p>RM12,500</p>  
                            </div>  
                        </div>  
                        <div class="summary-card">  
                            <div class="card-icon">  
                                <i class="fas fa-warehouse"></i>  
                            </div>  
                            <div class="card-content">  
                                <h3>Current Stock</h3>  
                                <p>1239</p>  
                            </div>  
                        </div>  
                    </div>  
                </div>  

                <!-- Quick Actions -->
                <div class="quick-actions">  
                    <button class="button" onclick="location.href='AdminProduct.aspx'; return false;">  
                        <i class="fas fa-plus"></i> Add New Product  
                    </button>   
                </div>  

                <!-- Recent Activity -->
                <div class="recent-activity">  
                    <h2>Recent Activity</h2>  
                    <div class="activity-list">  
                        <div class="activity-item">  
                            <i class="fas fa-shopping-cart"></i>  
                            <div class="activity-details">  
                                <p>New order received</p>  
                                <span>2 hours ago</span>  
                            </div>  
                        </div>  
                        <div class="activity-item">  
                            <i class="fas fa-box"></i>  
                            <div class="activity-details">  
                                <p>Product restocked</p>  
                                <span>4 hours ago</span>  
                            </div>  
                        </div>  
                    </div>  
                </div>  
            </div>  
        </div>  
    </form>
</body>
</html>