CREATE DATABASE Supermarket
--INSERT INTO Categories (Id, Name) VALUES
CREATE TABLE Categories(
Id INT NOT NULL IDENTITY
,[Name] NVARCHAR(30) NOT NULL

CONSTRAINT PK_Categories
PRIMARY KEY (Id)
)
--INSERT INTO Items (Id, Name, Price, CategoryId) VALUES
CREATE TABLE Items(
Id INT NOT NULL IDENTITY
,[Name] NVARCHAR(30) NOT NULL
,Price DECIMAL (15,2) NOT NULL
,CategoryId INT NOT NULL

CONSTRAINT PK_Items
PRIMARY KEY (Id)

, CONSTRAINT FK_Items_Categories
FOREIGN KEY (CategoryId)
REFERENCES Categories(Id)
)
--INSERT INTO Employees (Id, FirstName, LastName, Phone, Salary) VALUES
CREATE TABLE Employees(
Id INT NOT NULL IDENTITY
,[FirstName] NVARCHAR(50) NOT NULL
,[LastName] NVARCHAR(50) NOT NULL
,Phone CHAR(12) NOT NULL
,Salary DECIMAL (15,2) NOT NULL

CONSTRAINT PK_Employees
PRIMARY KEY (Id)

,CONSTRAINT chk_PhoneLegth
CHECK(LEN(Phone) = 12)
)
--INSERT INTO Orders(Id, DateTime, EmployeeId) VALUES
CREATE TABLE Orders(
Id INT NOT NULL IDENTITY
,[DateTime] DATETIME NOT NULL
,EmployeeId INT NOT NULL

CONSTRAINT PK_Orders
PRIMARY KEY (Id)

, CONSTRAINT FK_Orders_Employees
FOREIGN KEY (EmployeeId)
REFERENCES Employees(Id)
)
--INSERT INTO OrderItems (OrderId, ItemId, Quantity) VALUES
CREATE TABLE OrderItems(
OrderId INT NOT NULL
,ItemId INT NOT NULL
,Quantity INT NOT NULL

CONSTRAINT PK_OrderItem
PRIMARY KEY (OrderId, ItemId)

,CONSTRAINT FK_OrderItem_Orders
FOREIGN KEY (OrderId)
REFERENCES Orders(Id)

,CONSTRAINT FK_OrderItem_Items
FOREIGN KEY (ItemId)
REFERENCES Items(Id)
--additional constraint aboiut Qty minvalue = 1
,CONSTRAINT chk_QtyMinValue1
CHECK(Quantity>=1)
)
--INSERT INTO Shifts(Id, EmployeeId, CheckIn, CheckOut) VALUES
CREATE TABLE Shifts(
Id INT NOT NULL IDENTITY
,EmployeeId INT  
,CheckIn DATETIME NOT NULL
,CheckOut DATETIME NOT NULL

CONSTRAINT PK_Shifts
PRIMARY KEY (Id,EmployeeId)

,CONSTRAINT FK_Shifts_Employees
FOREIGN KEY (EmployeeId)
REFERENCES Employees(Id)

, CONSTRAINT chk_OutTimeAfterInTime
CHECK (CheckOut > CheckIn)
)

--2. Insert
INSERT INTO Employees
VALUES
('Stoyan','Petrov',	'888-785-8573','500.25')
,('Stamat','Nikolov','789-613-1122','999995.25')
,('Evgeni','Petkov','645-369-9517','1234.51')
,('Krasimir','Vidolov','321-471-9982','50.25')
 
INSERT INTO Items
VALUES
('Tesla battery','154.25',8)
,('Chess','30.25',8)
,('Juice','5.32',1)
,('Glasses','10',8)
,('Bottle of water','1',1)


--3. Update
--Make all items’ prices 27% more expensive where the category ID is either 1, 2 or 3.
UPDATE Items
SET Price *=1.27
WHERE CategoryId IN (1,2,3)

--4. Delete
--Delete all order items where the order id is 48 (be careful with the relationships)
DELETE FROM OrderItems
WHERE OrderId = 48

Delete FROM Orders
WHERE  Orders.Id = 48

--5. Richest People
--Select all employees who have a salary above 6500. Order them by first name, then by employee id.
SELECT id,FirstName
FROM Employees
WHERE Salary > 6500
ORDER BY FirstName,Id

--6. Cool Phone Numbers
--Select all full names from employees, whose phone number start with ‘3’.Order them by first name (ascending), then by phone number (ascending).
SELECT FirstName+' '+LastName, Phone
FROM Employees
WHERE Phone LIKE '3%'
ORDER BY FirstName,Phone

--7. Employee Statistics
--Select all employees who have orders with the total count of the orders they processed. Order them by their orders count (descending), then by first name. Select their first name, last name and total count of orders.
SELECT e.FirstName, e.LastName, COUNT(o.EmployeeId) AS [Count]
FROM Employees AS e
JOIN Orders AS o ON e.Id = o.EmployeeId
GROUP BY e.FirstName,e.LastName
ORDER BY COUNT(o.EmployeeId)DESC, e.FirstName ASC

--8. Hard Workers Club
SELECT t.FirstName,t.LastName,t.[Work hours] 
FROM (SELECT e.id
		,e.FirstName
		,e.LastName
		,AVG(DATEDIFF(HOUR,CheckIn,CheckOut)) AS 'Work hours'
FROM Shifts AS s
JOIN Employees AS e ON e.Id = s.EmployeeId
GROUP BY e.Id,e.FirstName,e.LastName,s.EmployeeId) AS t
WHERE t.[Work hours]>7
ORDER BY t.[Work hours] DESC,t.Id

--9. The Most Expensive Order
SELECT TOP 1 t.OrderId
		,SUM(t.Quantity*t.Price) AS Price
FROM (SELECT oi.OrderId AS OrderId 
			,i.Id AS ItemID
			,oi.Quantity
			,i.Price
		FROM OrderItems AS oi
		JOIN Orders AS o ON o.Id = oi.OrderId
		JOIN Items AS i ON i.Id = oi.ItemId) AS t
GROUP BY t.OrderId
ORDER BY SUM(t.Quantity*t.Price) DESC

--10. Rich Item, Poor Item
SELECT	TOP (10 )oi.OrderId
		,MAX(i.Price) AS ExpensivePrice
		,MIN(i.Price) AS CheapPrice
FROM OrderItems AS oi
LEFT JOIN Items AS i
	ON i.Id = oi.ItemId
GROUP BY oi.OrderId
ORDER BY MAX(i.Price) DESC, oi.OrderId ASC

--11. Cashiers
--Find all employees who have orders. Select their id, first name and last name. Order them by employee id.
SELECT *
FROM (SELECT DiSTINCT o.EmployeeId
					,e.FirstName
					,e.LastName  
		FROM Orders AS o
		JOIN Employees AS e ON e.Id = o.EmployeeId) AS t
ORDER BY t.EmployeeId

--12. Lazy Employees
--Find all employees, who have below 4 work hours per day.
--Order them by employee id.

SELECT DISTINCT t.Id
		,t.[Full Name]
FROM(SELECT e.Id AS Id
				,e.FirstName + ' ' + e.LastName AS 'Full Name'
				,DATEDIFF(HOUR,s.CheckIn,s.CheckOut) AS HoursPerDay
		FROM Employees AS e
		LEFT JOIN Shifts AS s
			ON s.EmployeeId = e.Id) AS t
WHERE t.HoursPerDay < 4

--16. Average Profit per Day
--Find the average profit for each day. Select the day of month and average daily profit of sold products.
--Sort them by day of month (ascending) and format the profit to the second digit after the decimal point.

WITH ItemsQtyPerOrder_CTE (OrderId,ItemId,Quantity,[DateTime])
AS
	(SELECT oi.OrderId
			, oi.ItemId
			, oi.Quantity
			, o.[DateTime]
	FROM Orders AS o
	LEFT JOIN OrderItems AS oi
		ON oi.OrderId = o.Id)

SELECT 
		DAY(g.DateTime) AS [Day]
		,CAST(AVG(g.TotalPricePerOrderRow)AS decimal(10,2)) AS 'Total Profit'
FROM
		(SELECT s.DateTime AS [DateTime]
				,s.Quantity*s.Price AS TotalPricePerOrderRow
		FROM
			(SELECT it.*
					,i.Price 
			FROM (SELECT oi.OrderId
						,oi.ItemId
						,oi.Quantity
						,o.[DateTime]
			FROM Orders AS o
			LEFT JOIN OrderItems AS oi
				ON oi.OrderId = o.Id) AS it
				LEFT JOIN Items AS i
					ON i.Id = it.ItemId) AS s ) AS g
GROUP BY DAY(g.DateTime)
ORDER BY DAY(g.DateTime) ASC
--17. Top Products
--Find information about all products. Select their name, category, how many of them were sold and the total profit they produced.
--Sort them by total profit (descending) and their count (descending)

WITH ItemList_CTE (ItemID,ItemName,ItemPrice,CategoryName)
AS
(SELECT	i.Id AS ItemID
		,i.Name AS ItemName
		,i.Price AS ItemPrice
		,c.Name AS CategoryName
FROM Items AS i
JOIN Categories AS c
ON c.Id = i.CategoryId)

SELECT * FROM ItemList_CTE;

WITH QtyOrderedItemsWithPrice_CTE(ItemID,ItemName,ItemPrice,TotalQty,CategoryID)
AS(
SELECT i.Id AS ItemID
		,i.Name AS ItemName
		,i.Price AS ItemPrice
		,SUM(oi.Quantity) AS TotalQty
		,i.CategoryId AS CategoryID
FROM Items as i
LEFT JOIN OrderItems as oi
ON oi.ItemId = i.Id
GROUP BY i.Id, i.Name,i.Price,i.CategoryId)

SELECT t.ItemName as Item
		,c.[Name] AS Category
		,ISNULL(CAST(t.TotalQty AS VARCHAR),' ') AS [Count]
		,ISNULL(CAST((t.ItemPrice*t.TotalQty) AS VARCHAR), ' ') AS TotalPrice
FROM QtyOrderedItemsWithPrice_CTE AS t
LEFT JOIN Categories AS c
	ON c.Id = t.CategoryID
ORDER BY ISNULL(t.ItemPrice*t.TotalQty,0) DESC, [Count] DESC

--19. Cancel order
GO
CREATE PROCEDURE usp_CancelOrder(@OrderId INT, @CancelDate DATE)
AS
	IF NOT EXISTS (SELECT * FROM Orders WHERE id = @OrderId)
	BEGIN
		RAISERROR('The order does not exist!',16,1)
		RETURN;
	END
	ELSE IF (DATEDIFF(DAY,(SELECT [DateTime] FROM Orders WHERE Id = @OrderId),@CancelDate))>=3
	BEGIN
		DECLARE @TEST INT = DATEDIFF(DAY,(SELECT [DateTime] FROM Orders WHERE Id = @OrderId),@CancelDate);
		RAISERROR('You cannot cancel the order!',16,1)
		RETURN;
	END
	ELSE
	BEGIN
		DELETE FROM OrderItems
		WHERE OrderId = @OrderId;

		DELETE FROM Orders
		WHERE Id  = @OrderId;
	END
--20. Deleted Order

CREATE TABLE DeletedOrders(
OrderId INT NOT NULL
,ItemId INT NOT NULL
,ItemQuantity INT NOT NULL
)
GO
CREATE TRIGGER tr_DeleteProducts
ON OrderItems
INSTEAD OF DELETE
AS
BEGIN
	
	DELETE FROM OrderItems
	WHERE OrderId IN (SELECT d.OrderId FROM deleted AS d GROUP BY d.OrderId);

	INSERT INTO DeletedOrders
	SELECT * FROM deleted
		
END