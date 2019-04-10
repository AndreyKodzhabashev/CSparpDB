--Problem 1.	Find Names of All Employees by First Name
SELECT FirstName, LastName 
FROM Employees
WHERE FirstName LIKE 'sa%';

--Find Names of All employees by Last Name
SELECT FirstName, LastName
FROM Employees
WHERE LastName LIKE '%ei%';

--Problem 3.	Find First Names of All Employees
SELECT FirstName
FROM Employees
WHERE 
		DATEPART(yyyy, HireDate) BETWEEN 1995 AND 2005
AND
		DepartmentID IN (3,10);

--Problem 4.	Find All Employees Except Engineers
SELECT FirstName, LastName
FROM Employees
WHERE JobTitle NOT LIKE '%engineer%';

--Problem 5.	Find Towns with Name Length
SELECT Towns.Name
FROM Towns
WHERE LEN(Towns.Name) IN (5,6)
ORDER BY Towns.Name ASC;

--Problem 6. Find Towns Starting With
SELECT *
FROM Towns
WHERE SUBSTRING(Towns.Name,1,1) IN('M','K','B','E')
ORDER BY Towns.Name ASC;

--Problem 7.	 Find Towns Not Starting With
SELECT *
FROM Towns
WHERE SUBSTRING(Towns.Name,1,1) NOT IN('R','B','D')
ORDER BY Towns.Name ASC;
GO
--Problem 8.	Create View Employees Hired After 2000 Year
CREATE VIEW V_EmployeesHiredAfter2000 
AS
SELECT FirstName, LastName
FROM Employees
WHERE DATEPART(yyyy,HireDate) > 2000;

--Problem 9.	Length of Last Name
SELECT FirstName, LastName
FROM Employees
WHERE LEN(LastName) = 5;

--Problem 10 and 11. Rank Employees by Salary
-- for Problem 11 - the selection for Problem 10 is executed and named as table 'e'
--The "table 'e'" is used for FROM table
--The WHERE clause for Problem 10 is extracted from the internernal selection due to the SQL rules 
SELECT *
FROM (
	SELECT EmployeeID
		,FirstName
		,LastName
		,Salary
		,DENSE_RANK() OVER (
			PARTITION BY e.Salary ORDER BY e.EmployeeID
			) AS [Rank]
	FROM Employees AS e
	) AS t
WHERE t.Salary BETWEEN 10000
		AND 50000
	AND t.[Rank] = 2
ORDER BY t.Salary DESC;


--Problem 12.	Countries Holding ‘A’ 3 or More Times
SELECT CountryName AS 'Country Name'
	,IsoCode AS 'ISO Code'
FROM Countries
WHERE LEN(CountryName) - LEN(REPLACE(CountryName, 'A', '')) >= 3
ORDER BY [ISO Code];


--Problem 13. Mix of Peak and River Names
--Combine all peak names with all river names, so that the last letter of each peak name is the same as the first letter of its corresponding river name. Display the peak names, river names, and the obtained mix (mix should be in lowercase). Sort the results by the obtained mix.
SELECT	PeakName
		,RiverName
		,LOWER(CONCAT(LEFT(PeakName,LEN(PeakName)-1),RiverName)) AS Mix  
FROM Peaks, Rivers
WHERE RIGHT(PeakName,1) = LEFT(RiverName,1)
ORDER BY Mix

--Problem 14.	Games from 2011 and 2012 year

SELECT TOP(50) [Name], CONCAT(DATEPART(YEAR,[Start]),'-',RIGHT('00' + CONVERT(NVARCHAR(2), DATEPART(MONTH, Start)), 2),'-', RIGHT('00' + CONVERT(NVARCHAR(2), DATEPART(DAY, Start)), 2)) AS [Start]
 FROM Games
WHERE YEAR([Start]) IN (2011, 2012)
ORDER BY [Start], [Name];

-- Problem 15. User Email Providers
SELECT Username, RIGHT(Email,(LEN(Email) - CHARINDEX('@', Email, 1)))AS 'Email Provider'
FROM Users
ORDER BY RIGHT(Email,(LEN(Email) - CHARINDEX('@', Email, 1))), Username

-- Problem 16. Get Users with IPAdress Like Pattern
SELECT Username, IpAddress 
FROM Users
WHERE IpAddress LIKE '___.1%.%.___'
ORDER BY Username

--Problem 17. Show All Games with Duration and Part of the Day
SELECT	[Name]
		,CASE
			WHEN DATEPART(HOUR,[Start]) >=0 AND DATEPART(HOUR,[Start]) < 12 THEN 'Morning'
			WHEN DATEPART(HOUR,[Start]) >=12 AND DATEPART(HOUR,[Start]) < 18 THEN 'Afternoon'
			WHEN DATEPART(HOUR,[Start]) >=18 AND DATEPART(HOUR,[Start]) < 24 THEN 'Evening'
		END AS 'Part of the Day'
		,CASE
			WHEN Duration IS NULL THEN 'Extra Long'
			WHEN Duration <= 3 THEN 'Extra Short'
			WHEN Duration BETWEEN 4 AND 6 THEN 'Short'
			WHEN Duration > 6 THEN 'Long'
		END AS Duration
FROM Games AS g
ORDER BY [Name], Duration, [Part of the Day]
--Problem 18. Orders Table
SELECT ProductName, OrderDate, DATEADD(DAY, 3, OrderDate)AS 'Pay Due', DATEADD(MONTH, 1 ,OrderDate) AS 'Delivery Due' 
FROM	Orders
