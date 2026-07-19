<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminProduct.aspx.cs" Inherits="Assg1.AdminProduct" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Product Management</title>
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
        .form-container {
            max-width: 800px;
            margin: 20px auto;
            padding: 20px;
            background: #fff;
            border-radius: 10px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }
        .form-container p {
            margin-bottom: 15px;
        }
        .form-container label {
            display: block;
            font-weight: bold;
            margin-bottom: 5px;
        }
        .form-container input,
        .form-container select,
        .form-container button,
        .form-container textarea {
            width: 100%;
            padding: 10px;
            margin-top: 5px;
            margin-bottom: 15px;
            border: 1px solid #ddd;
            border-radius: 5px;
            font-size: 1em;
        }
        .form-container button {
            background-color: #007bff;
            color: white;
            border: none;
            cursor: pointer;
            font-size: 1em;
            transition: all 0.3s ease;
        }
        .form-container button:hover {
            background-color: #0056b3;
        }
        .table-container {
            width: 100%;
            margin: 0 auto;
            overflow-x: auto;
            padding: 10px;
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
        .success-message {
            color: #28a745;
            text-align: center;
            margin-bottom: 15px;
        }
        .error-message {
            color: #dc3545;
            text-align: center;
            margin-bottom: 15px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <h1>Admin Product Management</h1>
        
        <div class="success-message">
            <asp:Label ID="SuccessMessageLabel" runat="server" ForeColor="Green"></asp:Label>
        </div>

        <div class="error-message">
            <asp:Label ID="ErrorMessageLabel" runat="server" ForeColor="Red"></asp:Label>
        </div>

        <div class="table-container">
            <asp:GridView ID="ProductGridView" runat="server" AutoGenerateColumns="False"
                DataKeyNames="ProductID" AllowPaging="True" PageSize="10"
                OnRowDeleting="ProductGridView_RowDeleting" 
                OnSelectedIndexChanged="ProductGridView_SelectedIndexChanged"
                OnPageIndexChanging="ProductGridView_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="ProductID" HeaderText="Product ID" ReadOnly="True" />
                    <asp:BoundField DataField="ProductName" HeaderText="Name" />
                    <asp:BoundField DataField="Price" HeaderText="Price" />
                    <asp:BoundField DataField="DiscountPrice" HeaderText="Discount Price" />
                    <asp:BoundField DataField="Stock" HeaderText="Stock" />
                    <asp:BoundField DataField="ImageURL" HeaderText="Image URL" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:BoundField DataField="Rating" HeaderText="Rating" />
                    <asp:BoundField DataField="Color" HeaderText="Color" />
                    <asp:BoundField DataField="Size" HeaderText="Size" />
                    <asp:BoundField DataField="CategoryName" HeaderText="Category Name" />
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                </Columns>
            </asp:GridView>
        </div>

        <div id="form-container" class="form-container">
            <h2>Add/Update Product</h2>
            <asp:HiddenField ID="HiddenProductID" runat="server" />
            <p>
                Name: <asp:TextBox ID="ProductNameTextBox" runat="server"></asp:TextBox>
            </p>
            <p>
                Price: <asp:TextBox ID="PriceTextBox" runat="server"></asp:TextBox>
            </p>
            <p>
                Standard Price: <asp:TextBox ID="StandardPriceTextBox" runat="server"></asp:TextBox>
            </p>
            <p>
                Premium Price: <asp:TextBox ID="PremiumPriceTextBox" runat="server"></asp:TextBox>
            </p>
            <p>
                Deluxe Price: <asp:TextBox ID="DeluxePriceTextBox" runat="server"></asp:TextBox>
            </p>
            <p>
                Standard Stock: <asp:TextBox ID="StandardStockTextBox" runat="server"></asp:TextBox>
            </p>
            <p>
                Premium Stock: <asp:TextBox ID="PremiumStockTextBox" runat="server"></asp:TextBox>
            </p>
            <p>
                Deluxe Stock: <asp:TextBox ID="DeluxeStockTextBox" runat="server"></asp:TextBox>
            </p>
            <p>
                Rating: <asp:TextBox ID="RatingTextBox" runat="server"></asp:TextBox>
            </p>
            <p>
                Color: 
                <asp:DropDownList ID="ColorDropDown" runat="server">
                    <asp:ListItem Text="--Select Color--" Value="0" />
                    <asp:ListItem Text="Red" Value="Red"></asp:ListItem>
                    <asp:ListItem Text="Pink" Value="Pink"></asp:ListItem>
                    <asp:ListItem Text="White" Value="White"></asp:ListItem>
                    <asp:ListItem Text="Yellow" Value="Yellow"></asp:ListItem>
                </asp:DropDownList>
            </p>
            <p>
                Category:
                <asp:DropDownList ID="CategoryDropdown" runat="server"></asp:DropDownList>
            </p>
            <p>
                Image URL: <asp:TextBox ID="ImageURLTextBox" runat="server"></asp:TextBox>
                or Upload: <asp:FileUpload ID="ImageFileUpload" runat="server" />
                <img id="photoPreview" class="img-thumbnail mt-2" style="display:none; max-width: 100px;" alt="Preview"/>
            </p>
            <p>
                Description: <asp:TextBox ID="DescriptionTextBox" runat="server" TextMode="MultiLine" Rows="4" Columns="50"></asp:TextBox>
            </p>
            <p>
                <asp:Button ID="AddUpdateButton" runat="server" Text="Add/Update" OnClick="AddUpdateButton_Click" />
                <asp:Button ID="ClearButton" runat="server" Text="Clear" OnClick="ClearButton_Click" />
            </p>
        </div>
    </form>
    <script>
        // Wait for the DOM to be ready
        document.addEventListener("DOMContentLoaded", function () {
            // Find the FileUpload element using its ClientID
            const fileUpload = document.getElementById('<%= ImageFileUpload.ClientID %>');
        fileUpload.addEventListener('change', function () {
            const file = this.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    const preview = document.getElementById('photoPreview');
                    preview.src = e.target.result;
                    preview.style.display = "block";
                };
                reader.readAsDataURL(file);
            }
        });
    });
</script>
</body>
</html>