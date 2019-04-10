-- LOOP
-- The table should be created before
DECLARE @i INT = 0

WHILE @i < 10
BEGIN

INSERT INTO TestNumsFromLOOP VALUES (@i)
SET @i += 1

END

SELECT * FROM TestNumsFromLOOP

-- Creating FUNCTION
	-- when function returns whole TABLE
SELECT * FROM Countries
GO
CREATE FUNCTION v_CountryByCapital (@CapitalName VARCHAR(45))
RETURNS TABLE
AS
RETURN
   SELECT * FROM Countries AS c 
   WHERE c.Capital = @CapitalName;
   GO
   SELECT * FROM v_CountryByCapital('London')

	--when function returns single Value. The function can be used to define column in a new SELECT satement
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

--Create function level of Salary in SoftUni Employees in respect of the amount
GO

CREATE OR ALTER FUNCTION udf_SalaryLevel (@Salary MONEY)
RETURNS VARCHAR(10) AS
BEGIN
	DECLARE @SalaryLevel VARCHAR(10);
	IF(@Salary < 30000)
	BEGIN
		SET @SalaryLevel = 'Low'
	END

	ElSE IF(@Salary BETWEEN 30000 AND 50000)
	BEGIN
		SET @SalaryLevel = 'Average'
	END

	ELSE  IF(@Salary > 50000)
	BEGIN
		SET @SalaryLevel = 'High'
	END
		
	RETURN @SalaryLevel
END

SELECT e.FirstName, e.LastName,e.Salary, dbo.udf_SalaryLevel(e.Salary) 
FROM Employees AS e

--Create PROC or PROCEDURE
GO
CREATE OR ALTER PROCEDURE udf_SelectEmployeesInRespectOfYearsOfSlavery (@YearsOfSlavery INT)
AS
	SELECT e.FirstName
			,e.LastName
			, DATEDIFF(YEAR, e.HireDate, GETDATE()) AS [YearsOfSlavery]
	FROM Employees AS e
	WHERE DATEDIFF(YEAR, e.HireDate, GETDATE()) > @YearsOfSlavery
	
EXEC dbo.udf_SelectEmployeesInRespectOfYearsOfSlavery @YearsOfSlavery = 10 --declaring a variable is oprional  

--create trigger AFTER UPDATE
GO
CREATE TRIGGER tr_DeleteProducts
ON Products
INSTEAD OF DELETE
AS
BEGIN
	DECLARE @FeedbackIDTEST INT = (SELECT d.Id FROM deleted AS d)
	
	DELETE ProductsIngredients
	WHERE ProductId IN (SELECT d.Id FROM deleted AS d)
	
	DELETE Feedbacks
	WHERE ProductId IN (SELECT d.Id FROM deleted AS d)

	DELETE Products
	WHERE Id IN (SELECT d.Id FROM deleted AS d)
	
END

