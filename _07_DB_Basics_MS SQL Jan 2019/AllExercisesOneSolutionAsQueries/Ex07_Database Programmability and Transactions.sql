--Part 1. Queries for SoftUni Database
--Problem 1. Employees with Salary Above 35000
CREATE PROCEDURE usp_GetEmployeesSalaryAbove35000
AS
SELECT FirstName
		,LastName
FROM Employees
WHERE Salary>35000

GO
--Problem 2. Employees with Salary Above Number
CREATE PROC usp_GetEmployeesSalaryAboveNumber(@MinSalary DECIMAL(18,4))
AS
SELECT e.FirstName
		,e.LastName
FROM Employees AS e
WHERE Salary>=@MinSalary

--Problem 3. Town Names Starting With
GO
CREATE PROCEDURE usp_GetTownsStartingWith (@TownName NVARCHAR(MAX))
AS
DECLARE @Concat NVARCHAR(MAX);
SET @Concat = @TownName + '%';
SELECT t.Name
FROM Towns AS t
WHERE t.Name LIKE @Concat

--Problem 4. Employees from Town
GO
CREATE PROCEDURE usp_GetEmployeesFromTown (@TownName NVARCHAR(MAX))
AS
SELECT e.FirstName
		, e.LastName
FROM Employees AS e
JOIN Addresses AS a ON a.AddressID = e.AddressID 
JOIN Towns AS t ON t.TownID = a.TownID
WHERE t.Name = @TownName

--Problem 5. Salary Level Function
GO
--Write a function ufn_GetSalaryLevel(@salary DECIMAL(18,4)) that receives salary of an employee and returns the level of the salary.
--•	If salary is < 30000 return “Low”
--•	If salary is between 30000 and 50000 (inclusive) return “Average”
--•	If salary is > 50000 return “High”
CREATE FUNCTION ufn_GetSalaryLevel(@Salary DECIMAL(18,4))
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @Result NVARCHAR(MAX);
		
	IF @Salary < 30000 SET @Result = 'Low'
	ELSE IF @Salary BETWEEN 30000 AND 50000  SET @Result = 'Average'
	ELSE SET @Result = 'High'
	
	RETURN @Result;
END

-- Problem 6. Employees by Salary Level
--Write a stored procedure usp_EmployeesBySalaryLevel that receive 
--as parameter level of salary (low, average or high) 
--and print the names of all employees that have given level of salary. 
--You should use the function - “dbo.ufn_GetSalaryLevel(@Salary)”, which was part of the previous task, inside your “CREATE PROCEDURE …” query.
GO
CREATE PROCEDURE usp_EmployeesBySalaryLevel (@Salary NVARCHAR(MAX))
AS
	SELECT e.FirstName
			,e.LastName
	FROM Employees AS e
	WHERE dbo.ufn_GetSalaryLevel(e.Salary) = @Salary

--Problem 7. Define Function
--Define a function ufn_IsWordComprised(@setOfLetters, @word) 
--that returns true or false depending on that if the word is a comprised of the given set of letters. 
GO
CREATE FUNCTION ufn_IsWordComprised(@setOfLetters NVARCHAR(MAX), @word NVARCHAR(MAX))
RETURNS INT
AS
	BEGIN	
	DECLARE @temp NCHAR
			,@result INT = 1
			,@endOfWhile INT = LEN(@word)
			,@wordToChange NVARCHAR(MAX)= @word;

	WHILE (@endOfWhile > 0)
	BEGIN
	 	IF(CHARINDEX(LEFT(@wordToChange , 1), @setOfLetters, 1) = 0)
		RETURN 0
			
		SET @endOfWhile -=1
		SET @wordToChange = REPLACE(@wordToChange, @wordToChange, RIGHT(@wordToChange,@endOfWhile))
	END

	RETURN @result
	END

--Problem 8. * Delete Employees and Departments
GO
CREATE OR ALTER PROC usp_DeleteEmployeesFromDepartment(@DepartmentId INT)
AS
	BEGIN
		declare @delTargets Table(
    [Id] int,
    [Name] nvarchar(max),
    [DepartmentID] int);
 
insert into @delTargets
    select e.[EmployeeID], d.[Name], d.[DepartmentID]
    from Employees as e
        inner join [Departments] as d
        on e.[DepartmentID] = d.[DepartmentID]
    where e.DepartmentID = @DepartmentId
 
alter table dbo.Departments
alter column [ManagerID] int
 --SELECT * FROM EmployeesProjects
 --SELECT * FROM Employees
 --WHERE DepartmentID = 5
delete from EmployeesProjects
where [EmployeeID] in (select [Id] from @delTargets)
                     
update Employees set [ManagerID] = NULL
where [ManagerID] in (select [Id] from @delTargets)
 
update Departments set [ManagerID] = NULL
where [ManagerID] in (select [Id] from @delTargets)
 
delete from Employees
where [DepartmentID] in (select [DepartmentID] from @delTargets)
 
delete from dbo.Departments
where [Name] in (select [Name] from @delTargets)
	END
GO
EXEC usp_DeleteEmployeesFromDepartment 5

--Part 2. Queries for Bank Database
--Problem 9. Find Full Name
--You are given a database schema with tables 
--AccountHolders(Id (PK), FirstName, LastName, SSN) 
--and Accounts(Id (PK), AccountHolderId (FK), Balance).
--Write a stored procedure usp_GetHoldersFullName that selects the full names of all people. 
GO
CREATE PROC usp_GetHoldersFullName
AS
	SELECT a.FirstName + ' '+ a.LastName AS 'Full Name'
	FROM AccountHolders AS a

-- Problem 10. People with Balance Higher Than
--Your task is to create a stored procedure usp_GetHoldersWithBalanceHigherThan 
--that accepts a number as a parameter and returns all people who have more money in total of all their accounts than the supplied number.
--NB 
--Òàçè çàäà÷à å ñ ãðåøíî ðåøåíèå â Judge. Àêî ñëîæèø â GROUP BY ïúðâî ïî ID â òàáëèöàòà AccountHolders, íÿìà äà ìèíå
GO 
CREATE OR ALTER PROCEDURE usp_GetHoldersWithBalanceHigherThan (@value DECIMAL(18,4))
AS
	SELECT f.FirstName
			,f.LastName
	FROM(
		SELECT ah.FirstName
				,ah.LastName
				,SUM(a.Balance) AS Total
		FROM AccountHolders AS ah
		JOIN Accounts AS a ON a.AccountHolderId = ah.Id
		GROUP BY ah.FirstName,ah.LastName) AS f	
	WHERE f.Total > @value
	ORDER BY f.FirstName,f.LastName

	--SELECT ah.FirstName
	--		,ah.LastName 
	--FROM AccountHolders AS ah
	--JOIN	(SELECT a.AccountHolderId
	--				,SUM(a.Balance) AS TotalBalance
	--		FROM Accounts AS a
	--		GROUP BY a.AccountHolderId) as f ON f.AccountHolderId = ah.Id
	--WHERE f.TotalBalance > @value 
	
--Problem 11. Future Value Function
GO
CREATE FUNCTION ufn_CalculateFutureValue (@sum float, @yearlyInterestRate float, @numberOfYears int)
RETURNS DECIMAL(15,4)
AS
BEGIN
	DECLARE @result float;
	DECLARE @temp float;
	SET @temp = CONVERT(float, @sum)
	SET @result = @temp * POWER((1 + @yearlyInterestRate), @numberOfYears)
	RETURN @result
END

--Problem 12. Calculating Interest
GO
CREATE PROC usp_CalculateFutureValueForAccount(@accountId INT, @interest FLOAT)
AS
 SELECT a.Id AS 'Account Id'
		,ah.FirstName AS 'First Name'
		,ah.LastName AS 'Last Name'
		,a.Balance AS 'Current Balance'
		,dbo.ufn_CalculateFutureValue(a.Balance,@interest,5) AS 'Balance in 5 years'
 FROM Accounts AS a
JOIN AccountHolders AS ah ON ah.Id = a.AccountHolderId AND a.Id = @accountId

--Part 3. Queries for Diablo Database
--Problem 13. *Scalar Function: Cash in User Games Odd Rows
CREATE FUNCTION ufn_CashInUsersGames(@GameName VARCHAR(MAX))
RETURNS @RtnTable TABLE
(
SumCash MONEY
)
AS
	BEGIN
	DECLARE @CashSum MONEY

	SET @CashSum =  (SELECT SUM(ug.Cash) AS 'SumCash'
	FROM (
			SELECT Cash, GameId, ROW_NUMBER() OVER (ORDER BY Cash DESC) AS RoWNum
			FROM UsersGames
			WHERE GameId = (SELECT Id FROM Games WHERE Name = @GameName)
		 ) ug
	WHERE ug.RoWNum % 2 != 0
	)

	INSERT @RtnTable SELECT @CashSum
	RETURN
END
--Problem 14. Create Table Logs
--Create a table – Logs (LogId, AccountId, OldSum, NewSum). 
--Add a trigger to the Accounts table that enters a new entry into the Logs table every time the sum on an account changes. 
--Submit only the query that creates the trigger.
CREATE TABLE Logs (
	LogId INT PRIMARY KEY IDENTITY
	, AccountId INT NOT NULL
	, OldSum DECIMAL(18,2) NOT NULL
	, NewSum DECIMAL(18,2) NOT NULL
	)
GO
CREATE TRIGGER trg_InsertTansactionsIntoLogTable
ON Accounts
INSTEAD OF UPDATE
AS
BEGIN	
	DECLARE @accountID INT = (SELECT i.Id FROM inserted AS i)
	DECLARE @newBalance DECIMAL(18,4) = (SELECT i.Balance FROM inserted AS i)
	DECLARE @oldBalance DECIMAL(18,4) = (SELECT a.Balance FROM Accounts as a WHERE a.Id = @accountID)

	UPDATE Accounts
	SET Balance = @newBalance
	WHERE id = @accountID
	
	INSERT INTO Logs
	VALUES (@accountID,@oldBalance, @newBalance)
END
GO

--Problem 15. Create Table Emails
--Create another table – NotificationEmails(Id, Recipient, Subject, Body). Add a trigger to logs table and create new email whenever new record is inserted in logs table. The following data is required to be filled for each email:
--•	Recipient – AccountId
--•	Subject – “Balance change for account: {AccountId}”
--•	Body - “On {date} your balance was changed from {old} to {new}.”
--Submit your query only for the trigger action.
GO
CREATE TABLE NotificationEmails(
Id INT PRIMARY KEY IDENTITY
, Recipient INT NOT NULL
, [Subject] NVARCHAR(50) NOT NULL
, Body NVARCHAR(MAX) NOT NULL
)
CREATE OR ALTER TRIGGER trg_NewEmailAfterInsertRecordInToLogs
ON Logs
FOR INSERT
AS
BEGIN
	DECLARE @dateTime VARCHAR(50) = (SELECT FORMAT (GETDATE(),'MMM dd yyyy HH:MMtt'))
	DECLARE @accNum INT = (SELECT i.AccountId FROM inserted AS i)
	DECLARE @oldBalance DECIMAL(18,2) = (SELECT i.OldSum FROM inserted AS i)
	DECLARE @newBalance DECIMAL(18,2) = (SELECT i.NewSum FROM inserted AS i)

	DECLARE @subject NVARCHAR(50) = 'Balance change for account: '+ CAST(@accNum AS nvarchar);
	DECLARE @body NVARCHAR(100) = 'On' + ' ' + CAST(@dateTime AS NVARCHAR) + ' ' +  'your balance was changed from ' + CAST(@oldBalance AS nvarchar) +' to ' + CAST(@newBalance AS nvarchar) +'.'

	INSERT INTO NotificationEmails
	VALUES (@accNum, @subject, @body)
END

--Problem 16. Deposit Money
--Add stored procedure usp_DepositMoney (AccountId, MoneyAmount) that deposits money to an existing account. 
--Make sure to guarantee valid positive MoneyAmount with precision up to fourth sign after decimal point. The procedure should produce exact results working with the specified precision.
GO
CREATE PROC usp_DepositMoney (@AccountId INT, @MoneyAmount DECIMAL(15,4))
AS
BEGIN
	DECLARE @isValid INT = (SELECT  COUNT(Id) FROM Accounts
						WHERE id = @AccountId)
	IF(@isValid !=0)
	BEGIN
		UPDATE Accounts
		SET Balance += @MoneyAmount
		WHERE id = @AccountId
	END
	ELSE RAISERROR ('Ivalid account number',16,1)
END

--Problem 17. Withdraw Money
--Add stored procedure usp_WithdrawMoney (AccountId, MoneyAmount) 
--that withdraws money from an existing account. 
--Make sure to guarantee valid positive MoneyAmount with precision up to fourth sign after decimal point. 
--The procedure should produce exact results working with the specified precision.
CREATE PROCEDURE usp_WithdrawMoney (@AccountId INT, @MoneyAmount DECIMAL(15,4))
AS
BEGIN
	DECLARE @isValid INT = (SELECT  COUNT(Id) FROM Accounts
						WHERE id = @AccountId)
	IF(@isValid !=0)
	BEGIN
		UPDATE Accounts
		SET Balance -= @MoneyAmount
		WHERE id = @AccountId
	END
	ELSE RAISERROR ('Ivalid account number',16,1)
END

--Problem 18. Money Transfer
--Write stored procedure usp_TransferMoney(SenderId, ReceiverId, Amount) that transfers money from one account to another. 
CREATE PROCEDURE usp_TransferMoney(@SenderId INT, @ReceiverId INT, @Amount DECIMAL(15,4))
AS
BEGIN
	DECLARE @isValidSender INT = (SELECT  COUNT(Id) FROM Accounts
						WHERE id = @SenderId)
	DECLARE @isValidReceiver INT = (SELECT  COUNT(Id) FROM Accounts
						WHERE id = @ReceiverId)
						
	BEGIN TRANSACTION

			EXEC usp_WithdrawMoney @SenderId, @Amount

			EXEC usp_DepositMoney @ReceiverId, @Amount
		IF(@isValidSender <> 0 AND @isValidReceiver <> 0)
		BEGIN
			COMMIT;
		END
		ELSE ROLLBACK;
END

--Problem 20. *Massive Shopping
DECLARE @User VARCHAR(MAX) = 'Stamat'
DECLARE @GameName VARCHAR(MAX) = 'Safflower'
DECLARE @UserId INT = (SELECT Id FROM Users WHERE Username = @User)
DECLARE @GameId INT = (SELECT Id FROM Games WHERE Name = @GameName)
DECLARE @UserMoney MONEY = (SELECT Cash FROM UsersGames WHERE UserId = @UserId AND GameId = @GameId)
DECLARE @ItemsBulkPrice MONEY
DECLARE @UserGameId INT = (SELECT Id FROM UsersGames WHERE UserId = @UserId AND GameId = @GameId)

BEGIN TRAN--11 to 12
		SET @ItemsBulkPrice = (SELECT SUM(Price) FROM Items WHERE MinLevel BETWEEN 11 AND 12)
		IF (@UserMoney - @ItemsBulkPrice >= 0)
		BEGIN
			INSERT UserGameItems
			SELECT i.Id, @UserGameId FROM Items AS i
			WHERE i.id IN (Select Id FROM Items WHERE MinLevel BETWEEN 11 AND 12)
			UPDATE UsersGames
			SET Cash = Cash - @ItemsBulkPrice
			WHERE GameId = @GameId AND UserId = @UserId
			COMMIT
		END
		ELSE
		BEGIN
			ROLLBACK
		END
			

SET @UserMoney = (SELECT Cash FROM UsersGames WHERE UserId = @UserId AND GameId = @GameId)
BEGIN TRAN--19 to 21
		SET @ItemsBulkPrice = (SELECT SUM(Price) FROM Items WHERE MinLevel BETWEEN 19 AND 21)
		IF (@UserMoney - @ItemsBulkPrice >= 0)
		BEGIN
			INSERT UserGameItems
			SELECT i.Id, @UserGameId FROM Items AS i
			WHERE i.id IN (Select Id FROM Items WHERE MinLevel BETWEEN 19 AND 21)
			UPDATE UsersGames
			SET Cash = Cash - @ItemsBulkPrice
			WHERE GameId = @GameId AND UserId = @UserId
			COMMIT
		END
		ELSE
		BEGIN
			ROLLBACK
		END

 SELECT Name AS 'Item Name' FROM Items
 WHERE Id IN (SELECT ItemId FROM UserGameItems WHERE UserGameId = @UserGameId)
 ORDER BY [Item Name]

--Problem 21. Employees with Three Projects
--Create a procedure usp_AssignProject(@emloyeeId, @projectID) that assigns projects to employee. If the employee has more than 3 project throw exception and rollback the changes. The exception message must be: "The employee has too many projects!" with Severity = 16, State = 1.
GO
CREATE PROCEDURE usp_AssignProject(@emloyeeId INT, @projectID INT)
AS
BEGIN
	DECLARE @maxProjectsCountPerEmpl INT = 3;
	DECLARE @currentCountProjectsPerEmpl INT = (SELECT COUNT(*) 
											FROM EmployeesProjects AS ep
											WHERE ep.EmployeeID = @emloyeeId)
	BEGIN TRANSACTION
	INSERT INTO EmployeesProjects
	VALUES(@emloyeeId, @projectID);

	IF( @currentCountProjectsPerEmpl
		 >= 
		@maxProjectsCountPerEmpl)
	BEGIN
	 RAISERROR('The employee has too many projects!',16,1)
	 ROLLBACK;
	END
	ELSE COMMIT

END
--Problem 22. Delete Employees
 CREATE TABLE Deleted_Employees(
--SELECT * FROM Employees
	EmployeeID INT PRIMARY KEY
	, FirstName NVARCHAR(40) NOT NULL
	, LastName NVARCHAR(40) NOT NULL
	, MiddleName NVARCHAR(40)
	, JobTitle NVARCHAR(40) NOT NULL
	, DepartmentId INT NOT NULL
	, Salary DECIMAL (15,2))
GO
	CREATE TRIGGER trg_DeleteEmployees
  ON Employees
  AFTER DELETE
AS
  BEGIN
    INSERT INTO Deleted_Employees
      SELECT FirstName,LastName,MiddleName,JobTitle,DepartmentID,Salary
      FROM deleted
  END
