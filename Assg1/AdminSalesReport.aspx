<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminSalesReport.aspx.cs" Inherits="Assg1.SalesReport" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sales Report</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
            background-color: #f4f7fa;
            color: #333;
        }
        h1, h2 {
            text-align: center;
            color: #000;
        }
        .report-container {
            max-width: 1200px;
            margin: 20px auto;
            padding: 20px;
            background: #fff;
            border-radius: 10px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }
        .report-table-container {
            width: 100%;
            margin: 0 auto;
            overflow-x: auto;
        }
        table {
            width: 100%;
            border-collapse: collapse;
        }
        th, td {
            padding: 15px;
            border: 1px solid #ddd;
            text-align: left;
        }
        th {
            background-color: #5DB996;
            color: white;
        }
        tr:nth-child(even) {
            background-color: #f9f9f9;
        }
        tr:hover {
            background-color: #f1f1f1;
        }
        .filters {
            margin-bottom: 20px;
            display: flex;
            justify-content: space-between;
        }
        .filters select, .filters button {
            padding: 10px;
            border: 1px solid #ddd;
            border-radius: 5px;
            font-size: 1em;
        }
        .summary-container {
            margin-top: 20px;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Sales Report</h1>
        <div class="report-container">
            <div class="filters">
                <div>
                    <label for="ReportType">Select Report:</label>
                    <asp:DropDownList ID="ReportTypeDropdown" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ReportTypeDropdown_SelectedIndexChanged">
                        <asp:ListItem Text="Monthly Report" Value="Monthly"></asp:ListItem>
                        <asp:ListItem Text="Yearly Report" Value="Yearly"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div>
                    <label for="YearDropdown">Year:</label>
                    <asp:DropDownList ID="YearDropdown" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
                <asp:Button ID="GenerateReportButton" runat="server" CssClass="btn btn-primary mt-2" Text="Generate Report" OnClick="GenerateReportButton_Click" />
            </div>
            
            <div class="report-table-container mt-4">
                <!-- Dynamic Placeholder for Monthly/Yearly Reports -->
                <asp:PlaceHolder ID="SalesReportPlaceholder" runat="server"></asp:PlaceHolder>
            </div>
            
            <div class="summary-container mt-4">
                <asp:Label ID="TotalRevenueLabel" runat="server" CssClass="text-success"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>
