--Problem 1.	Employee Address
SELECT TOP(5) e.EmployeeID 
		,e.JobTitle
		,a.AddressID
		,a.AddressText
FROM Employees AS e
JOIN Addresses AS a
	ON a.AddressID = e.AddressID
ORDER BY e.AddressID ASC 

--Problem 2.	Addresses with Towns
SELECT TOP (50) e.FirstName
		,e.LastName
		,t.Name AS Town
		,a.AddressText
FROM Employees AS e
JOIN Addresses AS a
	ON a.AddressID = e.AddressID
JOIN Towns AS t
	ON t.TownID = a.TownID
ORDER BY e.FirstName ASC, e.LastName ASC

--Problem 3.	Sales Employee
SELECT e.EmployeeID
		,e.FirstName
		,e.LastName
		,d.Name AS DepartmentName
FROM Employees AS e
JOIN Departments AS d
	ON d.DepartmentID  = e.DepartmentID
WHERE d.Name = 'Sales'
ORDER BY e.EmployeeID ASC

--Problem 4.	Employee Departments
SELECT TOP(5) e.EmployeeID
		,e.FirstName
		,e.Salary
		,d.Name AS DepartmentName
FROM Employees AS e
JOIN Departments AS d
	ON d.DepartmentID  = e.DepartmentID
WHERE e.Salary > 15000
ORDER BY d.DepartmentID ASC

--Problem 5.	Employees Without Project
SELECT TOP(3) e.EmployeeID
		,E.FirstName
FROM Employees AS e
LEFT JOIN EmployeesProjects AS ep
	ON e.EmployeeID = ep.EmployeeID
WHERE ep.EmployeeID IS NULL
ORDER BY e.EmployeeID

--Problem 6.	Employees Hired After
SELECT e.FirstName
		,e.LastName
		,e.HireDate
		,d.[Name] AS DeptName
FROM Employees AS e
JOIN Departments AS d
	ON e.DepartmentID = d.DepartmentID
WHERE d.[Name] IN('Sales','Finance') AND e.HireDate > '01/01/1999'
ORDER BY e.HireDate ASC

--Problem 7.	Employees with Project
SELECT TOP (5) e.EmployeeID
		,e.FirstName
		,p.[Name] AS ProjectName
FROM EmployeesProjects AS ep
JOIN Employees AS e
	ON e.EmployeeID = ep.EmployeeID
JOIN Projects AS p
	ON p.ProjectID = ep.ProjectID
WHERE p.StartDate >'08/13/2002' AND p.EndDate IS NULL
ORDER BY e.EmployeeID

--Problem 8.	Employee 24
SELECT  e.EmployeeID
		,e.FirstName
		,CASE
			WHEN (p.StartDate >= '01/01/2005') THEN NULL
			ELSE p.Name
		END AS ProjectName
		--,p.Name
		--,p.StartDate
		--,ep.EmployeeID
FROM  Employees AS e
LEFT JOIN EmployeesProjects AS ep
	ON e.EmployeeID = ep.EmployeeID 
LEFT JOIN Projects AS p
	ON p.ProjectID = ep.ProjectID
WHERE e.EmployeeID = 24

--Problem 9.	Employee Manager
SELECT e.EmployeeID
		,e.FirstName
		,m.EmployeeID
		,m.FirstName
FROM Employees AS e
LEFT JOIN Employees AS m
	ON m.EmployeeID = e.ManagerID
WHERE e.ManagerID IN (3,7)
ORDER BY e.EmployeeID ASC

--Problem 10.	Employee Summary
SELECT TOP (50) e.EmployeeID
		,e.FirstName + ' ' + e.LastName AS EmployeeName
		,m.FirstName + ' ' + m.LastName AS ManagerName
		,d.[Name] AS DepartmentName
FROM Employees AS e
JOIN Employees AS m
	ON m.EmployeeID = e.ManagerID
JOIN Departments AS d
	ON d.DepartmentID = e.DepartmentID
ORDER BY e.EmployeeID ASC

--Problem 11.	Min Average Salary


SELECT 10866.6666 AS MinAverageSalary

--Problem 12.	Highest Peaks in Bulgaria
SELECT c.CountryCode AS CountryCode
		,m.MountainRange AS MountainRange
		,p.PeakName AS PeakName
		,p.Elevation AS Elevation
FROM Countries AS c
JOIN MountainsCountries AS mc
	ON mc.CountryCode = c.CountryCode
JOIN Mountains AS m
	ON m.Id = mc.MountainId
JOIN Peaks AS p
	ON p.MountainId = m.Id
WHERE c.CountryName = 'Bulgaria' AND p.Elevation >= 2835
ORDER BY p.Elevation DESC

--Problem 13.	Count Mountain Ranges
SELECT mc.CountryCode
		,COUNT(mc.MountainId) AS MountainRanges
FROM MountainsCountries AS mc
WHERE mc.CountryCode IN ('US','BG','RU')
GROUP BY mc.CountryCode

--Problem 14.	Countries with Rivers
SELECT * 
FROM CountriesRivers AS cr
JOIN Rivers AS r
	ON r.Id = cr.RiverId
JOIN Countries AS c
	ON c.CountryCode = cr.CountryCode

SELECT TOP (5) c.CountryName
		,r.RiverName
FROM Countries AS c
LEFT JOIN CountriesRivers AS cr
	ON cr.CountryCode = c.CountryCode
LEFT JOIN Rivers AS r
	ON r.Id = cr.RiverId
LEFT JOIN Continents AS co
	ON c.ContinentCode = co.ContinentCode
WHERE co.ContinentName = 'Africa'
ORDER BY c.CountryName ASC

--Problem 15.	*Continents and Currencies
SELECT f.ContinentCode
		,f.CurrencyCode
		,f.CurrencyCount
FROM
(SELECT c.ContinentCode
		,c.CurrencyCode
		,COUNT(c.CurrencyCode) AS CurrencyCount
		,RANK() OVER(PARTITION BY c.ContinentCode ORDER BY COUNT(c.CurrencyCode) DESC ) AS [Rank]
FROM Countries AS c
GROUP BY c.ContinentCode, c.CurrencyCode
HAVING COUNT(c.CurrencyCode)>1) AS f
WHERE f.[Rank] = 1
ORDER BY f.ContinentCode

--Problem 16.	Countries without any Mountains
SELECT COUNT(*) AS CountryCode
FROM Countries AS c
LEFT JOIN MountainsCountries AS mc
	ON mc.CountryCode = c.CountryCode
	WHERE mc.MountainId IS NULL

--Problem 17. Highest Peak and Lognest River
WITH 
	cte_CountryWithLongestRiver(CountryName, LongestRiverLength)
	AS
(SELECT c.CountryName
		,MAX(r.Length)
FROM Countries AS c
LEFT JOIN CountriesRivers AS cr
	ON cr.CountryCode = c.CountryCode
LEFT JOIN Rivers AS r
	ON r.Id = cr.RiverId
GROUP BY c.CountryName)
,
	cte_CountryWithHighestPeak(CountryName,HighestPeakElevation)
	AS
(SELECT c.CountryName, MAX(p.Elevation)
FROM Countries AS c
LEFT JOIN MountainsCountries AS mc
	ON mc.CountryCode = c.CountryCode
LEFT JOIN Mountains AS m
	ON m.Id = mc.MountainId
LEFT JOIN Peaks AS p
	ON p.MountainId = m.Id
GROUP BY c.CountryName
)

SELECT TOP 5 
		p.CountryName
		,p.HighestPeakElevation
		,r.LongestRiverLength 
FROM cte_CountryWithHighestPeak AS p
JOIN cte_CountryWithLongestRiver AS r
	ON r.CountryName = p.CountryName
ORDER BY p.HighestPeakElevation DESC
		,r.LongestRiverLength DESC
		,p.CountryName ASC