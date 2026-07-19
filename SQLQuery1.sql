ALTER TABLE Products
ADD CONSTRAINT FK_Products_Categories
FOREIGN KEY (CategoryID)
REFERENCES Categories(CategoryID);

-- Begin Transaction for Data Insertion
BEGIN TRANSACTION;

-- Insert Sample Data into Categories Table
INSERT INTO Categories (CategoryName)
VALUES 
    ('Birthday'), 
    ('Christmas');

-- Insert Sample Data into Products Table
INSERT INTO Products (ProductName, Description, Price, DiscountPrice, ImageURL, Rating, CategoryID, Color, Stock)
VALUES 
    ('Red Roses Forever', 'A bouquet of 50 red roses.', 89.00, 79.00, '/images/red_roses.jpg', 4.5, 1, 'Red', 10),
    ('Sweet Kisses', 'A lovely pink bouquet.', 59.99, 49.99, '/images/sweet_kisses.jpg', 4.2, 1, 'Pink', 15),
    ('Royal Red', 'A luxurious red bouquet.', 99.99, 89.99, '/images/royal_red.jpg', 4.8, 2, 'Red', 8);

-- Commit the Transaction to Save Changes
COMMIT TRANSACTION;

-- Query: Fetch Products by CategoryID and Color
SELECT * 
FROM Products
WHERE CategoryID = 1 
  AND Color = 'Red';

-- Query: Search for Products by Name (Containing 'Roses')
SELECT * 
FROM Products
WHERE ProductName LIKE '%Roses%';

-- Query: Fetch Products with Specific Discount Price Range and Color
SELECT ProductID, ProductName, Description, Price, DiscountPrice, ImageURL, Rating, Color, Stock 
FROM dbo.Products
WHERE DiscountPrice BETWEEN 100 AND 200
  AND Color = 'Red';