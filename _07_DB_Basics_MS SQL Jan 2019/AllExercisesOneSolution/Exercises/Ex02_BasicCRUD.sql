
--Problem 6 - Find Email Address of Each Employee
SELECT FirstName + '.' + LastName + '@softuni.bg' AS 'Full Email Address'
FROM Employees;

--Problem 7 - Find All Different Employee�s Salaries
SELECT DISTINCT Salary FROM Employees;

--Problem 8 - Find all Information About Employees
SELECT * FROM Employees
WHERE JobTitle = 'Sales Representative';

--Problem 9 - Find Names of All Employees by Salary in Range
SELECT FirstName, LastName, JobTitle FROM Employees
WHERE Salary BETWEEN 20000 AND 30000;

--Problem 10 - Find Names of All Employees
SELECT FirstName + ' ' + MiddleName + ' ' + LastName AS 'Full Name' FROM Employees
WHERE Salary = 25000 OR Salary = 14000 OR Salary = 12500 OR Salary = 23600;

--Problem 11 - Find All Employees Without Manager
SELECT FirstName, LastName FROM Employees
WHERE ManagerID IS NULL;

--Problem 12 - Find All Employees with Salary More Than 50000
SELECT FirstName,LastName,Salary FROM Employees
WHERE Salary > 50000
ORDER BY Salary DESC;

--Problem 13 - Find 5 Best Paid Employees.
SELECT TOP (5) FirstName, LastName 
FROM Employees
ORDER BY Salary DESC;

--Problem 14 - Find All Employees Except Marketing
SELECT FirstName,LastName
FROM Employees
WHERE DepartmentID != 4;

--Problem 15 - Sort Employees Table
SELECT *
FROM Employees
ORDER BY Salary DESC,FirstName ASC,LastName DESC,MiddleName ASC;
GO
--Problem 16 - Create View Employees with Salaries
CREATE VIEW V_EmployeeNameJobTitle AS
SELECT  FirstName, LastName, Salary 
FROM Employees;
GO
--17.	Create View Employees with Job Titles
CREATE VIEW V_EmployeeNameJobTitle AS
SELECT	CONCAT(' ', e.FirstName
		,' '
		, CASE
			WHEN  e.MiddleName IS NULL THEN ''
			ELSE e.MiddleName
		END
        ,' '
		, e.LastName) AS 'Full Name'
		, e.JobTitle AS 'Job Title'
FROM Employees AS e;
GO

--Problem 18 - Distinct Job Titles
SELECT DISTINCT JobTitle FROM Employees;

--Problem 19 - Find First 10 Started Projects
SELECT TOP (10) * FROM Projects
ORDER By StartDate ASC, Name ASC;

--Problem 20 - Last 7 Hired Employees
SELECT TOP (7) FirstName, LastName, HireDate FROM Employees
ORDER BY HireDate DESC;

--Problem 21 - Increase Salaries
UPDATE Employees
SET Salary *= 1.12
WHERE DepartmentID IN (1,2,4,11); 

SELECT Salary FROM Employees

--Problem 22 - All Mountain Peaks
SELECT PeakName FROM Peaks
ORDER BY PeakName;

--Problem 23 - Biggest Countries by Population
SELECT TOP(30) CountryName,[Population] 
FROM Countries
WHERE ContinentCode = 'EU'
ORDER BY [Population] DESC, CountryName ASC;

--Problem 24 - *Countries and Currency (Euro / Not Euro)
SELECT CountryName, CountryCode,  
CASE
    WHEN CurrencyCode = 'EUR' THEN 'Euro'
    ELSE 'Not Euro'
END AS CurrencyCode
FROM Countries
ORDER BY CountryName ASC;

--Problem 25 - All Diablo Characters
SELECT Name FROM Characters
ORDER BY Name;