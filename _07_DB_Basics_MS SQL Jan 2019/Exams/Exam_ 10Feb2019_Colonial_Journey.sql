CREATE TABLE Planets
(
Id INT PRIMARY KEY IDENTITY CHECK (Id > 0)
,[Name] VARCHAR(30) NOT NULL

)

CREATE TABLE Spaceports
(
Id INT PRIMARY KEY IDENTITY CHECK (Id > 0)
,[Name] VARCHAR(50) NOT NULL
,PlanetId INT NOT NULL FOREIGN KEY REFERENCES Planets(Id) CHECK (PlanetId > 0)
)

CREATE TABLE Spaceships
(
Id INT PRIMARY KEY IDENTITY
,[Name] VARCHAR(50) NOT NULL
,Manufacturer VARCHAR(30) NOT NULL
,LightSpeedRate INT DEFAULT 0
)

CREATE TABLE Colonists
(
Id INT PRIMARY KEY IDENTITY
,[FirstName] VARCHAR(20) NOT NULL
,[LastName] VARCHAR(20) NOT NULL
,Ucn VARCHAR(10) NOT NULL UNIQUE
,BirthDate DATE NOT NULL
)

CREATE TABLE Journeys
(
Id INT PRIMARY KEY IDENTITY
,JourneyStart DATETIME NOT NULL
,JourneyEnd DATETIME   NOT NULL
,Purpose VARCHAR(11) CHECK ( Purpose IN ('Medical', 'Technical', 'Educational', 'Military'))
,DestinationSpaceportId INT NOT NULL FOREIGN KEY REFERENCES Spaceports(Id)
,SpaceshipId INT NOT NULL FOREIGN KEY REFERENCES Spaceships(Id)
)

CREATE TABLE TravelCards
(
Id INT PRIMARY KEY IDENTITY
,CardNumber CHAR(10) NOT NULL UNIQUE
,JobDuringJourney VARCHAR(8) CHECK (JobDuringJourney IN ('Pilot', 'Engineer', 'Trooper', 'Cleaner', 'Cook'))
,ColonistId INT NOT NULL FOREIGN KEY REFERENCES Colonists(Id)
,JourneyId INT NOT NULL FOREIGN KEY REFERENCES Journeys(Id)
)

--2.	Insert
--Insert sample data into the database. Write a query to add the following records into the corresponding tables. All Ids should be auto-generated.
INSERT INTO Planets([Name]) VALUES
('Mars')
,('Earth')
,('Jupiter')
,('Saturn')

INSERT INTO Spaceships VALUES
('Golf','VW',3)
,('WakaWaka','Wakanda',4)
,('Falcon9','SpaceX',1)
,('Bed','Vidolov',6)

--3.	Update
--Update all spaceships light speed rate with 1 where the Id is between 8 and 12.
SELECT * FROM Spaceships
WHERE Id BETWEEN 8 AND 12

UPDATE Spaceships
SET LightSpeedRate +=1
WHERE Id BETWEEN 8 AND 12

SELECT * FROM Spaceships
WHERE Id BETWEEN 8 AND 12

--4.	Delete
--Delete first three inserted Journeys (be careful with the relationships).
DELETE FROM TravelCards
WHERE JourneyId BETWEEN 1 AND 3 

DELETE FROM Journeys
WHERE Id BETWEEN 1 AND 3 

--5.	Select all travel cards
SELECT tc.CardNumber
		,tc.JobDuringJourney
FROM TravelCards AS tc
ORDER BY tc.CardNumber

--6.	Select all colonists
--Extract from the database, all colonists. Sort the results by first name, them by last name, and finally by id in ascending order.
SELECT c.Id
		,c.FirstName +' '+ c.LastName AS FullName
		,c.Ucn
FROM Colonists AS c
ORDER BY c.FirstName
		,c.LastName
		, c.Id

--7.	Select all military journeys
--Extract from the database, all Military journeys. Sort the results ascending by journey start.
SELECT j.Id
		,CONVERT(VARCHAR(10), j.JourneyStart, 103) AS JourneyStart
		,CONVERT(VARCHAR(10), j.JourneyEnd, 103) AS JourneyEnd
FROM Journeys AS j
WHERE j.Purpose = 'Military'
ORDER BY j.JourneyStart

--8.	Select all pilots
--Extract from the database all colonists, which have a pilot job. Sort the result by id, ascending.
SELECT c.Id
		, c.FirstName + ' ' + c.LastName AS 'full_name'
FROM Colonists AS c
JOIN TravelCards AS tc ON tc.ColonistId = c.Id AND tc.JobDuringJourney = 'Pilot'
ORDER BY c.Id

----9.	Count colonists
--Count all colonists that are on technical journey. 
SELECT COUNT (c.Id) AS [count]
FROM Colonists AS c
JOIN TravelCards AS tc ON tc.ColonistId = c.Id AND tc.JobDuringJourney = 'Engineer'

--10.	Select the fastest spaceship
--Extract from the database the fastest spaceship and its destination spaceport name. In other words, the ship with the highest light speed rate.
DECLARE @SpaceshipId INT = (SELECT MAX(s.LightSpeedRate)FROM Spaceships AS s)

SELECT ss.[Name] AS [SpaceshipName] 
		,sp.[Name]AS [SpaceportName]
FROM Spaceships AS ss
JOIN Journeys AS j ON j.SpaceshipId = ss.Id
JOIN Spaceports AS sp ON sp.Id = j.DestinationSpaceportId
WHERE ss.Id = @SpaceshipId

--11.	Select spaceships with pilots younger than 30 years
SELECT s.[Name]
		,s.Manufacturer
FROM Colonists AS c
JOIN TravelCards AS t ON t.ColonistId = c.Id AND t.JobDuringJourney = 'Pilot'
JOIN Journeys AS j ON j.Id = t.JourneyId
JOIN Spaceships AS s ON s.Id = j.SpaceshipId
WHERE DATEDIFF(YEAR, c.BirthDate,'01/01/2019')<30
ORDER BY s.[Name]

----12.	Select all educational mission planets and spaceports
--Extract from the database names of all planets and their spaceports, which have educational missions. Sort the results by spaceport name in descending order.

SELECT p.[Name] AS PlanetName
		,sp.[Name] AS SpaceportName
FROM Journeys AS j
JOIN Spaceports AS sp ON sp.Id = j.DestinationSpaceportId AND Purpose = 'Educational'
JOIN Planets AS p ON p.Id = sp.PlanetId
ORDER BY sp.[Name] DESC

--13.	Select all planets and their journey count
--Extract from the database all planets’ names and their journeys count. Order the results by journeys count, descending and by planet name ascending.
SELECT p.[Name] AS 'PlanetName'
		,COUNT (j.Id) AS 'JourneysCount'
FROM Planets AS p
JOIN Spaceports AS sp ON sp.PlanetId = p.Id
JOIN Journeys AS j ON j.DestinationSpaceportId = sp.Id
GROUP BY p.[Name]
ORDER BY COUNT (j.Id) DESC
		,PlanetName ASC

--14.	Select the longest journey
--Extract from the database the longest journey, its destination spaceport name, planet name and purpose.
 SELECT TOP 1 j.Id AS Id
		,p.[Name] AS PlanetName
		,sp.[Name] AS SpaceportName
		,j.Purpose AS JourneyPurpose
 FROM Journeys AS j
 JOIN Spaceports AS sp ON sp.Id = j.DestinationSpaceportId
 JOIN Planets AS p ON p.Id = sp.PlanetId
 ORDER BY DATEDIFF(DAY,j.JourneyStart, j.JourneyEnd)

 --15.	Select the less popular job
--Extract from the database the less popular job in the longest journey. In other words, the job with less assign colonists.
SELECT TOP 1 
		j.Id AS Id
		,p.[Name] AS PlanetName
		,sp.[Name] AS SpaceportName
		,j.Purpose AS JourneyPurpose
INTO #temp
FROM Journeys AS j
JOIN Spaceports AS sp ON sp.Id = j.DestinationSpaceportId
JOIN Planets AS p ON p.Id = sp.PlanetId
ORDER BY DATEDIFF(DAY,j.JourneyStart, j.JourneyEnd)DESC
 
DECLARE @journeyID INT = (Select id FROM #temp)

SELECT TOP 1 
		tc.JourneyId AS JourneyId
		,tc.JobDuringJourney AS JobName
FROM TravelCards AS tc
WHERE tc.JourneyId = @journeyID
GROUP BY tc.JourneyId,tc.JobDuringJourney
ORDER BY COUNT(tc.JobDuringJourney) ASC

--16.	Select Second Oldest Important Colonist
--Find all colonists and their job during journey with rank 2. Keep in mind that all the selected colonists with rank 2 must be the oldest ones. You can use ranking over their job during their journey.
SELECT *
FROM
		(SELECT 
				tc.JobDuringJourney
				,c.FirstName + ' ' + c.LastName AS FullName
				,DENSE_RANK() OVER (PARTITION BY tc.JobDuringJourney ORDER BY DATEDIFF   (DAY,	c.BirthDate, GETDATE()) DESC) AS JobRank
		FROM Colonists AS c
		JOIN TravelCards AS tc ON tc.ColonistId = c.Id) AS k
WHERE k.JobRank = 2
 
--17.	Planets and Spaceports
--Find all planets and all of their spaceports. Select planet name and the count of the spaceports. Sort them by spaceports count (descending), then by name (ascending).
SELECT p.[Name]
		,COUNT(sp.Id) AS Count
FROM Planets AS p
left JOIN Spaceports AS sp ON sp.PlanetId = p.Id
GROUP BY p.Name
ORDER BY COUNT(sp.Id) DESC
		, p.Name ASC

--18.	Get Colonists Count
--Create a user defined function with the name dbo.udf_GetColonistsCount(PlanetName VARCHAR (30)) that receives planet name and returns the count of all colonists sent to that planet.
GO
CREATE FUNCTION udf_GetColonistsCount(@PlanetName VARCHAR (30))
RETURNS VARCHAR(30)
AS
BEGIN
	DECLARE @result INT=
	(SELECT COUNT (tc.ColonistId)
	FROM Planets AS p
	LEFT JOIN Spaceports AS sp ON sp.PlanetId = p.Id AND p.[Name] = @PlanetName
	LEFT JOIN Journeys AS j ON j.DestinationSpaceportId = sp.Id
	LEFT JOIN TravelCards AS tc ON tc.JourneyId = j.Id);

	RETURN @result
END 

--19.	Change Journey Purpose
GO
  CREATE PROCEDURE usp_ChangeJourneyPurpose(@JourneyId INT, @NewPurpose VARCHAR(11))
  AS
   IF((SELECT id FROM Journeys WHERE @JourneyId = Id)IS NULL)
	BEGIN
	RAISERROR('The journey does not exist!',16,1)
	RETURN
	END
   IF((SELECT id FROM Journeys AS j WHERE j.Id = @JourneyId AND j.Purpose = @NewPurpose)IS NOT NULL)
	BEGIN
	RAISERROR('You cannot change the purpose!',16,2)
	RETURN
	END
  UPDATE Journeys
  SET Purpose = @NewPurpose
  WHERE id = @JourneyId

  --20. DeletedJourneys
CREATE TRIGGER trg_DeleteJourney
ON Journeys
INSTEAD OF DELETE
AS
BEGIN
	DELETE FROM TravelCards
	WHERE JourneyId IN (SELECT Id FROM deleted)

	DELETE FROM Journeys
	WHERE id IN (SELECT Id FROM deleted)

	INSERT INTO DeletedJourneys
    SELECT d.Id, d.JourneyStart, d.JourneyEnd, d.Purpose, d.DestinationSpaceportId, d.SpaceshipId
    FROM deleted AS d
END
