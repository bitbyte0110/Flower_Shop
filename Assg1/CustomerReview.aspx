<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerReview.aspx.cs" Inherits="MyNamespace.CustomerReview" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <title>Customer Review Modal</title>
    <style>
        .star-rating {
            display: flex;
            flex-direction: row-reverse; /* Reverse order for easier left-to-right logic */
            justify-content: flex-start;
        }

        .star-rating input {
            display: none; /* Hide the actual radio buttons */
        }

        .star-rating label {
            font-size: 2rem;
            color: #ccc;
            cursor: pointer;
            transition: color 0.2s ease-in-out;
        }

        /* Highlight stars from the left on hover */
        .star-rating label:hover,
        .star-rating label:hover ~ label {
            color: #f5c518; /* Highlighted star color */
        }

        /* Keep stars highlighted from the left when selected */
        .star-rating input:checked ~ label {
            color: #f5c518;
        }

        .custom-green-btn {
            background-color: #00cc99; /* Custom green color */
            color: white;
            border: none;
            padding: 0.5rem 1rem;
            border-radius: 0.25rem;
            cursor: pointer;
        }

        .custom-green-btn:hover {
            background-color: #00b386; /* Darker green on hover */
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Button to trigger the modal -->
        <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#reviewModal">
            Open Review Modal
        </button>

        <!-- Modal -->
        <div class="modal fade" id="reviewModal" tabindex="-1" aria-labelledby="reviewModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content p-4">
                    <div class="modal-header">
                        <h5 class="modal-title" id="reviewModalLabel">Rate Product</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                         <h6><b>Order ID:</b> <asp:Literal ID="OrderIDLiteral" runat="server"></asp:Literal></h6>
                        
                        <!-- Product Quality Rating -->
                        <div class="mb-3">
                            <label class="form-label">Product Quality</label>
                            <div class="star-rating">
                                <input type="radio" id="quality-1" name="product-quality" value="1">
                                <label for="quality-1">&#9733;</label>
                                <input type="radio" id="quality-2" name="product-quality" value="2">
                                <label for="quality-2">&#9733;</label>
                                <input type="radio" id="quality-3" name="product-quality" value="3">
                                <label for="quality-3">&#9733;</label>
                                <input type="radio" id="quality-4" name="product-quality" value="4">
                                <label for="quality-4">&#9733;</label>
                                <input type="radio" id="quality-5" name="product-quality" value="5">
                                <label for="quality-5">&#9733;</label>
                            </div>
                        </div>

                        <!-- Review Text Area -->
                        <div class="mb-3">
                            <label for="CommentsTextBox" class="form-label">Comments:</label>
                            <asp:TextBox ID="CommentsTextBox" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" Placeholder="Write about this aspect..."></asp:TextBox>
                        </div>

                        <asp:HiddenField ID="ProductRatingHiddenField" runat="server" />

                        <!-- Add Photo -->
                        <div class="mb-3">
                            <label for="photoUpload" class="form-label">Add Photo</label>
                            <input type="file" class="form-control" id="photoUpload" accept="image/*">
                            <img id="photoPreview" class="img-thumbnail mt-2" style="display:none; max-width: 100px;" alt="Preview">
                        </div>
                    </div>

                    <div class="modal-footer d-flex justify-content-end gap-2">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                        <asp:Button ID="SubmitReviewButton" runat="server" Text="Submit" class="btn custom-green-btn" OnClick="SubmitReviewButton_Click" />
                    </div>

                    <div class="mb-3">
                        <asp:Label ID="ErrorMessageLabel" runat="server" CssClass="text-danger" style="display:none;"></asp:Label>
                        <asp:Label ID="SuccessMessageLabel" runat="server" CssClass="text-success" style="display:none;"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function updateProductRating(value) {
            // Access the server-side hidden field to update its value
            const hiddenField = document.getElementById('<%= ProductRatingHiddenField.ClientID %>');
            hiddenField.value = value;
        }

        document.getElementById('photoUpload').addEventListener('change', function () {
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
    </script>
</body>
</html>