--1. DDL (25 pts)
USE master
GO
CREATE DATABASE Bakery;
GO
USE Bakery;
GO

CREATE TABLE Countries(
Id INT NOT NULL IDENTITY CHECK (Id BETWEEN  0 AND  2147483647) PRIMARY KEY,
[Name] NVARCHAR(50) UNIQUE NOT NULL
);

CREATE TABLE Distributors(
Id INT NOT NULL CHECK (Id BETWEEN  0 AND  2147483647) PRIMARY KEY IDENTITY ,
[Name] NVARCHAR(25) UNIQUE NOT NULL,
AddressText NVARCHAR(30),
Summary NVARCHAR(200),
CountryId INT NOT NULL CHECK (CountryId BETWEEN  0 AND  2147483647) FOREIGN KEY (CountryId) REFERENCES Countries(Id)
);

CREATE TABLE Ingredients(
Id INT NOT NULL CHECK (Id BETWEEN  0 AND  2147483647) PRIMARY KEY IDENTITY ,
[Name] NVARCHAR(30) NOT NULL,
[Description] NVARCHAR(200),
OriginCountryId INT NOT NULL CHECK (OriginCountryId BETWEEN  0 AND  2147483647) FOREIGN KEY (OriginCountryId) REFERENCES Countries(Id),
DistributorId INT NOT NULL CHECK (DistributorId BETWEEN  0 AND  2147483647) FOREIGN KEY (DistributorId) REFERENCES Distributors(Id)
);

CREATE TABLE Products(
Id INT NOT NULL CHECK (Id BETWEEN  0 AND  2147483647) PRIMARY KEY IDENTITY,
[Name] NVARCHAR(25) NOT NULL UNIQUE,
[Description] NVARCHAR(250),
Recipe NVARCHAR(MAX),
Price MONEY NOT NULL CHECK (Price >= 0)
);

CREATE TABLE Customers(
Id INT NOT NULL CHECK (Id BETWEEN  0 AND  2147483647) PRIMARY KEY IDENTITY,
FirstName NVARCHAR(25) NOT NULL,
LastName NVARCHAR(25) NOT NULL,
Gender CHAR(1) CHECK (Gender IN ('M','F') ),
Age INT CHECK (Age BETWEEN  0 AND  2147483647),
PhoneNumber CHAR(10) NOT NULL,
CountryId INT NOT NULL CHECK (CountryId BETWEEN  0 AND  2147483647) FOREIGN KEY (CountryId) REFERENCES Countries(Id)
);

CREATE TABLE Feedbacks(
Id INT NOT NULL CHECK (Id BETWEEN  0 AND  2147483647) PRIMARY KEY IDENTITY,
[Description] NVARCHAR(255),
Rate DECIMAL(10,2) CHECK (Rate BETWEEN 0.00 AND 10.00),
ProductId INT NOT NULL CHECK (ProductId BETWEEN  0 AND  2147483647) FOREIGN KEY (ProductId) REFERENCES Products(Id),
CustomerId INT NOT NULL CHECK (CustomerId BETWEEN  0 AND  2147483647) FOREIGN KEY (CustomerId) REFERENCES Customers(Id),
);

CREATE TABLE ProductsIngredients(
ProductId INT NOT NULL CHECK (ProductId BETWEEN  0 AND  2147483647) FOREIGN KEY(ProductId) REFERENCES Products(Id),
IngredientId INT NOT NULL CHECK (IngredientId BETWEEN  0 AND  2147483647) FOREIGN KEY(IngredientId) REFERENCES Ingredients(Id)

CONSTRAINT PK_ProductId_IngredientId
PRIMARY KEY (ProductId, IngredientId)
);

--2. Insert
INSERT INTO Distributors([Name], CountryId, AddressText, Summary)
VALUES
('Deloitte & Touche', 2, '6 Arch St #9757', 'Customizable neutral traveling')
,('Congress Title ', 13, '58 Hancock St', 'Customer loyalty')
,('Kitchen People', 1, '3 E 31st St #77', 'Triple-buffered stable delivery')
,('General Color Co Inc', 21, '6185 Bohn St #72', 'Focus group')
,('Beck Corporation', 23, '21 E 64th Ave', 'Quality-focused 4th generation hardware')

INSERT INTO Customers(FirstName, LastName, Age, Gender, PhoneNumber, CountryId)
VALUES
('Francoise', 'Rautenstrauch', 15, 'M', '0195698399', 5)
,('Kendra', 'Loud', 22, 'F', '0063631526', 11)
,('Lourdes', 'Bauswell', 50, 'M', '0139037043', 8)
,('Hannah', 'Edmison', 18, 'F', '0043343686', 1)
,('Tom', 'Loeza', 31, 'M', '0144876096', 23)
,('Queenie', 'Kramarczyk', 30, 'F', '0064215793', 29)
,('Hiu', 'Portaro', 25, 'M', '0068277755', 16)
,('Josefa', 'Opitz', 43, 'F', '0197887645', 17)

--3. Update
UPDATE Ingredients
SET DistributorId = 35
WHERE [NAME] IN ( 'Bay Leaf', 'Paprika', 'Poppy');

UPDATE Ingredients
SET OriginCountryId = 14
WHERE OriginCountryId = 8;

--4. Delete
DELETE Feedbacks
WHERE CustomerId = 14 OR ProductId = 5;

--5. Products by Price
SELECT [Name], Price, [Description] FROM Products
ORDER BY Price DESC, [Name] ASC;

--6. Ingredients
SELECT [Name], [Description], OriginCountryId FROM Ingredients
WHERE OriginCountryId IN (1,10,20)
ORDER BY Id ASC;

--7. Ingredients from Bulgaria and Greece - WHY 1/3?????
SELECT TOP(15) i.[Name] AS [Name], i.[Description] AS [Description], c.[Name] AS CountryName FROM Ingredients AS i
JOIN Countries AS c
ON i.OriginCountryId = c.Id
WHERE OriginCountryId = 31 OR OriginCountryId = 32
ORDER BY i.[Name], c.[Name]; 

--8. Best Rated Products
--Select top 10 best rated products ordered by average rate (descending) then by amount of feedbacks (descending).
SELECT TOP(10)
		p.[Name]
		,p.[Description]
		,AVG(f.Rate) AS 'AverageRate'
		,COUNT(p.Name) AS  'FeedbacksAmount'
FROM Products AS p
JOIN Feedbacks AS f
ON p.Id = f.ProductId
GROUP BY 
		p.Name 
		,p.Description
ORDER BY
		AverageRate DESC
		,FeedbacksAmount DESC; 

--9. Negative Feedback
SELECT f.ProductId, f.Rate, f.[Description], f.CustomerId, c.Age, c.Gender FROM Feedbacks AS f
LEFT JOIN Customers AS c
ON f.CustomerId = c.Id
WHERE Rate < 5
ORDER BY f.ProductId DESC, f.Rate ASC;

--10. Customers without Feedback
SELECT CONCAT(c.FirstName, ' ', c.LastName) AS CustomerName, c.PhoneNumber, c.Gender FROM Customers AS c
LEFT JOIN Feedbacks AS f
ON c.Id = f.CustomerId
WHERE f.Id IS NULL
ORDER BY c.Id;
GO;

--11. Honorable Mentions
--Select all feedbacks given by customers which have at least 3 feedbacks. Order them by product Id then by
--customer name and lastly by feedback id – all ascending.
SELECT 
fc.ProductId
, fc.CustomerName
,ISNULL (fc.FeedbackDescription, ' ')
FROM (
SELECT 
			f.Id AS FeedBackID
			,f.ProductId AS ProductId
			,CONCAT(c.FirstName,' ', c.LastName) AS CustomerName
			,f.[Description] AS FeedbackDescription
			,COUNT(f.CustomerId) OVER (PARTITION BY f.CustomerId) AS [Count]
		FROM Feedbacks AS f
		JOIN Customers AS c
		ON f.CustomerId = c.Id) AS fc
		WHERE fc.[Count] > 2 
ORDER BY fc.ProductId, fc.CustomerName, fc.FeedBackID

--12. Customers by Criteria
--Select customers that are either at least 21 old and contain “an” in their first name or their phone number ends
--with “38” and are not from Greece. Order by first name (ascending), then by age(descending)
SELECT c.FirstName, c.Age, c.PhoneNumber FROM Customers AS c
WHERE (c.Age>=21 AND c.FirstName LIKE '%an%') OR (c.PhoneNumber LIKE '%38' AND c.CountryId <> 31)
ORDER BY c.FirstName ASC, c.Age DESC;
GO;

--13.	Middle Range Distributors
--Select all distributors which distribute ingredients used in the making process of all products having average rate between 5 and 8 (inclusive). Order by distributor name, ingredient name and product name all ascending.

SELECT		*
FROM	Ingredients
SELECT * FROM Distributors

--14.	The Most Positive Country
SELECT fin.CountryName, fin.FeedbackRate
FROM(	SELECT 
				t.CountryName	
				,t.FeedbackRate
				,RANK()OVER(ORDER BY t.FeedbackRate DESC) AS [Rank]
		FROM (SELECT cu.Name AS CountryName
					,AVG(f.Rate) OVER(PARTITION BY cu.Id) AS FeedbackRate
				FROM Feedbacks AS f
				JOIN Customers AS c
				ON c.Id = f.CustomerId
				RIGHT JOIN Countries cu
				ON cu.Id = c.CountryId) AS t
				) AS fin
				WHERE fin.Rank = 1
GROUP BY fin.CountryName, fin.FeedbackRate
	
--15.	Country Representative
SELECT w.CountryName, w.DisributorName
FROM(
SELECT 
	c.Name AS [CountryName],
	d.Name AS [DisributorName],
	COUNT(i.id) AS [Count],
	DENSE_RANK() OVER (PARTITION BY c.Name ORDER BY COUNT(i.id) DESC) AS [Rank]
FROM Countries AS c
INNER JOIN Distributors AS d
ON d.CountryId = c.Id
INNER JOIN Ingredients AS i
ON i.DistributorId = d.Id
GROUP BY c.Name, d.Name
) w
WHERE w.Rank = 1
ORDER BY w.CountryName, w.DisributorName

--16. Customers with Countries
CREATE VIEW v_UserWithCountries AS
SELECT CONCAT(cu.FirstName, ' ', cu.LastName) AS CustomerName,
				cu.Age,
				cu.Gender,
				co.Name AS CountryName
FROM Customers AS cu
JOIN Countries AS co
ON cu.CountryId = co.Id;
GO

--17.	Feedback by Product Name
CREATE FUNCTION dbo.udf_GetRating(@Name VARCHAR(MAX))
RETURNS VARCHAR(MAX)
AS
BEGIN
	DECLARE @result VARCHAR(MAX)
	SET @result = (
	SELECT CASE
		WHEN t.Rate IS NULL THEN 'No rating'
		WHEN t.Rate < 5 THEN 'Bad'
		WHEN t.Rate BETWEEN 5 AND 8 THEN 'Average'
		ELSE 'Good'
		END AS RatingAsName
	FROM(
			SELECT	p.Id
				,p.Name
				,AVG(f.Rate) as Rate
			FROM Products AS p
			LEFT JOIN Feedbacks AS f
			ON p.Id = f.ProductId
			GROUP BY p.Name, p.Id ) 
			AS t
WHERE t.[Name] = @Name
)
RETURN @result
END

	SELECT TOP 5 id, Name, dbo.udf_GetRating(Name)
	FROM Products
	ORDER BY Id

--18.	Send Feedback
GO
CREATE OR ALTER PROCEDURE usp_SendFeedback (@CustomerId INT, @ProductId INT,@Rate DECIMAL(10,2), @Descriprion NVARCHAR(255))
AS
	BEGIN TRANSACTION tr_temp

	DECLARE @FeedbackCountPerUser INT;
	SET @FeedbackCountPerUser = (SELECT COUNT(f.CustomerId) FROM Feedbacks AS f WHERE f.ProductId = @ProductId);

	INSERT INTO Feedbacks(CustomerId,[Description],Rate,ProductId)
	VALUES
			(@CustomerId, @Descriprion, @Rate , @ProductId)
	
	IF(@FeedbackCountPerUser > 3)
	BEGIN
		ROLLBACK;
		RAISERROR('You are limited to only 3 feedbacks per product!',16,1)
		RETURN;
	END
	COMMIT
	
--19.	Delete Products
GO
CREATE TRIGGER tr_DeleteProducts
ON Products
INSTEAD OF DELETE
AS
BEGIN
	DELETE Feedbacks
	WHERE ProductId IN (SELECT d.id FROM deleted AS d)

	DELETE ProductsIngredients
	WHERE ProductId IN (SELECT d.id FROM deleted AS d)

	DELETE Products 
	WHERE Id IN (SELECT d.Id FROM deleted AS d)
END

--20.	Products by One Distributor
WITH Res_CTE (ProductName, DistributorName)
AS
(
	SELECT
					p.Name AS [ProductName],
					d.Name AS [DistributorName]
				FROM Products AS p
				INNER JOIN Feedbacks AS f
				ON f.ProductId = p.Id
				INNER JOIN ProductsIngredients AS pin
				ON pin.ProductId = p.Id
				INNER JOIN Ingredients AS i
				ON i.Id = pin.IngredientId
				INNER JOIN Distributors AS d
				ON d.Id = i.DistributorId
				INNER JOIN Countries AS c
				ON c.Id = d.CountryId
				GROUP BY p.Id, p.Name, d.Name, c.Name
)

SELECT
	p.Name AS [ProductName],
	AVG(f.Rate) AS [ProductAverageRate],
	d.Name AS [DistributorName],
	c.Name AS [DistributorCountry]
FROM Products AS p
INNER JOIN Feedbacks AS f
ON f.ProductId = p.Id
INNER JOIN ProductsIngredients AS pin
ON pin.ProductId = p.Id
INNER JOIN Ingredients AS i
ON i.Id = pin.IngredientId
INNER JOIN Distributors AS d
ON d.Id = i.DistributorId
INNER JOIN Countries AS c
ON c.Id = d.CountryId
GROUP BY p.Id, p.Name, d.Name, c.Name
HAVING p.Name IN
				(
					SELECT r.ProductName 
					FROM Res_CTE AS r
					GROUP BY r.ProductName
					HAVING COUNT(r.DistributorName) = 1
				)
ORDER BY p.Id
