--Problem 1.	Records’ Count
SELECT COUNT(*) AS Count
FROM WizzardDeposits

--Problem 2.	Longest Magic Wand
SELECT TOP(1) MagicWandSize AS LongestMagicWand
FROM WizzardDeposits
ORDER BY MagicWandSize DESC;

--Problem 3.	Longest Magic Wand per Deposit Groups
SELECT DepositGroup, MAX(MagicWandSize) AS LongestMagicWand
FROM WizzardDeposits
GROUP BY DepositGroup

--Problem 4.	* Smallest Deposit Group per Magic Wand Size
SELECT TOP (2) DepositGroup
FROM WizzardDeposits
GROUP BY DepositGroup
ORDER BY AVG(MagicWandSize) ASC

--Problem 5.	Deposits Sum
SELECT DepositGroup, SUM(DepositAmount) AS 'TotalSum'
FROM WizzardDeposits
GROUP BY DepositGroup
--Problem 6.	Deposits Sum for Ollivander Family
SELECT DepositGroup, SUM(DepositAmount) AS 'TotalSum'
FROM WizzardDeposits
WHERE MagicWandCreator LIKE 'Ollivander%'
GROUP BY DepositGroup

--Problem 7.	Deposits Filter
SELECT DepositGroup, SUM(DepositAmount) AS 'TotalSum'
FROM WizzardDeposits
WHERE MagicWandCreator LIKE 'Ollivander%'
GROUP BY DepositGroup
HAVING SUM(DepositAmount) < 150000
ORDER BY TotalSum DESC

--Problem 8.	 Deposit Charge
SELECT DepositGroup, MagicWandCreator, MIN(DepositCharge) AS 'MinDepositCharge'
FROM WizzardDeposits
GROUP BY DepositGroup, MagicWandCreator
ORDER BY MagicWandCreator, DepositGroup

--Problem 9.	Age Groups
SELECT f.AgeGroup 
	,COUNT(*) AS WizardCount
FROM (
	SELECT CASE 
			WHEN Age >= 0 AND Age < 11 THEN '[0-10]'
			WHEN Age >= 11 AND Age < 21 THEN '[11-20]'
			WHEN Age >= 21 AND Age < 31	THEN '[21-30]'
			WHEN Age >= 31 AND Age < 41	THEN '[31-40]'
			WHEN Age >= 41 AND Age < 51	THEN '[41-50]'
			WHEN Age >= 51 AND Age < 61	THEN '[51-60]'
			WHEN Age >= 61				THEN '[61+]'
			END AS AgeGroup
	FROM WizzardDeposits
	) AS f
GROUP BY f.AgeGroup

--Problem 10.	First Letter
SELECT DISTINCT LEFT(FirstName,1) AS FirstLetter
FROM WizzardDeposits
WHERE DepositGroup = 'Troll Chest'
ORDER BY FirstLetter

--Problem 11.	Average Interest 
SELECT	DepositGroup
		,IsDepositExpired
		,AVG(DepositInterest) AS AverageInterest
FROM WizzardDeposits
WHERE DepositStartDate > '01/01/1985'
GROUP BY DepositGroup, IsDepositExpired
ORDER BY DepositGroup DESC, IsDepositExpired ASC

--Problem 13.	Departments Total Salaries
SELECT DepartmentID
	,SUM(Salary) AS TotalSalary
FROM Employees
GROUP BY DepartmentID
ORDER BY DepartmentID ASC

--Problem 14.	Employees Minimum Salaries
SELECT DepartmentID
	,MIN(Salary) AS MinimumSalary
FROM Employees
WHERE HireDate > '01/01/2000'
GROUP BY DepartmentID
HAVING DepartmentID IN (2,5,7)
ORDER BY DepartmentID ASC

--Problem 15.	Employees Average Salaries

SELECT * 
INTO NewTableSalary
FROM Employees
WHERE Salary > 30000

DELETE FROM NewTableSalary
WHERE ManagerID = 42;

UPDATE NewTableSalary
SET Salary += 5000
WHERE DepartmentID = 1

SELECT DepartmentID, AVG(Salary)
FROM NewTableSalary
GROUP BY DepartmentID

--Problem 16.	Employees Maximum Salaries
SELECT DepartmentID
		,Max(Salary) as MaxSalary
FROM Employees
GROUP BY DepartmentID
HAVING MAX(Salary) NOT BETWEEN 30000 AND 70000

--Problem 17.	Employees Count Salaries
SELECT COUNT(*) AS Count
FROM Employees
WHERE ManagerID IS NULL

--Problem 18.	*3rd Highest Salary
SELECT new2.DepartmentID, new2.Salary AS ThirdHighestSalary
FROM (SELECT new.DepartmentID
			,new.Salary
			,RANK()OVER(PARTITION BY new.DepartmentID 
						ORDER BY new.Salary DESC) AS [Rank] 
		FROM (SELECT DISTINCT  e1.Salary, e2.DepartmentID 
				FROM Employees AS e1
				JOIN Employees AS e2 
				ON e1.EmployeeID = e2.EmployeeID
				) AS new)
AS new2
WHERE new2.Rank = 3
GO
--Problem 18.	*3rd Highest Salary
--Variant with 2 VIEWS
-- WORKS AS Expected, but cannot be evaluated from JUDGE due a compile time error
CREATE VIEW V_DistinctSalaryByDepartment AS
SELECT DISTINCT  e1.Salary, e2.DepartmentID 
				FROM Employees AS e1
				JOIN Employees AS e2 
				ON e1.EmployeeID = e2.EmployeeID
GO
CREATE VIEW V_SalaryRankingByDepartments AS
SELECT v.DepartmentID
			,v.Salary
			,RANK()OVER(PARTITION BY v.DepartmentID 
						ORDER BY v.Salary DESC) AS [Rank] 
		FROM V_DistinctSalaryByDepartment AS v
GO
SELECT s.DepartmentID, s.Salary
FROM V_SalaryRankingByDepartments AS s
WHERE s.Rank = 3
--END OF Solution for Problem 18.	*3rd Highest Salary with 2 VIEWS

-- Problem 19.	**Salary Challenge
--Select all employees who have salary higher than the average salary of their respective departments. Select only the first 10 rows. Order by DepartmentID.
-- FirstName, LastName, DepartmentID

SELECT 	TOP(10)
		e.FirstName
		, e.LastName 
		, secondLevel.DepartmentID
FROM (SELECT firstLevel.DepartmentID, AVG (Salary) AS AverageSalary
		FROM Employees AS firstLevel
		GROUP BY firstLevel.DepartmentID) AS secondLevel
JOIN Employees AS e
ON secondLevel.DepartmentID = e.DepartmentID
WHERe e.Salary > secondLevel.AverageSalary
ORDER BY e.DepartmentID