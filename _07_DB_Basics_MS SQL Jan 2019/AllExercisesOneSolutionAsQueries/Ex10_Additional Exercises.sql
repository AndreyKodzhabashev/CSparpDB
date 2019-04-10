--PART I – Queries for Diablo Database
--Problem 1.	Number of Users for Email Provider
--Find number of users for email provider from the largest to smallest, then by Email Provider in ascending order. 
SELECT   SUBSTRING(u.Email,CHARINDEX('@' ,u.Email,1)+1,LEN(u.Email)) AS 'Email Provider'
		,COUNT(u.Id) AS 'Number Of Users'
FROM Users AS u
GROUP BY SUBSTRING(u.Email,CHARINDEX('@' ,u.Email,1)+1,LEN(u.Email))
ORDER BY COUNT(u.Id) DESC
		,SUBSTRING(u.Email,CHARINDEX('@' ,u.Email,1)+1,LEN(u.Email)) ASC

--Problem 2.	All User in Games
SELECT	g.Name AS Game
		,gt.Name AS 'Game Type'
		,u.Username AS Username
		,ug.[Level] AS [Level]
		,ug.Cash AS Cash
		,c.Name AS 'Character'
FROM Users AS u
LEFT JOIN UsersGames AS ug
	ON ug.UserId = u.Id
LEFT JOIN Games AS g
	ON g.Id = ug.GameId	
LEFT JOIN GameTypes AS gt
	ON gt.Id = g.GameTypeId
LEFT JOIN Characters AS c
	ON c.Id = ug.CharacterId
ORDER BY ug.[Level] DESC
		,u.Username ASC
		,g.[Name]
	
--Problem 3.	Users in Games with Their Items

SELECT	*
FROM (	SELECT u.Username AS Username
			,g.Name AS Game
			,COUNT(ugi.ItemId) AS 'Items Count'
			,SUM(i.Price) AS 'Items Price'
		FROM Games AS g
		JOIN UsersGames AS ug ON ug.GameId = g.Id
		JOIN UserGameItems AS ugi ON ugi.UserGameId = ug.Id
		JOIN Items AS i ON i.Id = ugi.ItemId
		JOIN Users AS u	ON u.Id = ug.UserId
		GROUP BY g.Name,u.Username) AS t		
WHERE t.[Items Count]>=10
ORDER BY t.[Items Count] DESC
		,t.[Items Price] DESC
		,t.Username

SELECT u.Username,
       g.Name AS Game, 
       COUNT(ugi.ItemId) AS 'Items Count', 
	   SUM(i.Price) AS 'Items Price'
  FROM [dbo].[Games] AS g
 INNER JOIN [dbo].[UsersGames] AS ug ON ug.GameId = g.Id
 INNER JOIN [dbo].[UserGameItems] AS ugi ON ugi.UserGameId = ug.Id
 INNER JOIN [dbo].[Items] AS i ON i.Id = ugi.ItemId
 INNER JOIN [dbo].[Users] AS u ON u.Id = ug.UserId
 GROUP BY g.Name, u.Username
HAVING COUNT(ugi.ItemId) >= 10
 ORDER BY COUNT(ugi.ItemId) DESC, SUM(i.Price) DESC, u.Username

--Problem 5.	All Items with Greater than Average Statistics
--Find all items with statistics larger than average. 
--Display only items that have Mind, Luck and Speed greater than average Items mind, luck and speed. 
--Sort the results by item names in alphabetical order. Submit your query statement as Prepare DB & run queries in Judge.
DECLARE @averageMind DECIMAL(15,2) = (SELECT AVG(Mind) FROM [Statistics])
DECLARE @averageLuck DECIMAL(15,2) = (SELECT AVG(Luck) FROM [Statistics])
DECLARE @averageSpeed DECIMAL(15,2) = (SELECT AVG(Speed) FROM [Statistics])

SELECT i.[Name]
		, i.Price
		, i.MinLevel
		, s.Strength
		, s.Defence
		, s.Speed
		, s.Luck
		, s.Mind
FROM Items AS i
JOIN [Statistics] AS s ON s.Id = i.StatisticId
WHERE s.Luck >  @averageLuck AND s.Mind > @averageMind AND s.Speed > @averageSpeed
ORDER BY i.[Name] ASC

--Problem 6.	Display All Items with Information about Forbidden Game Type
--Find all items and information whether and what forbidden game types they have. 
--Display item name, price, min level and forbidden game type. Display all items. 
--Sort the results by game type in descending order, then by item name in ascending order.
SELECT i.[Name] AS Item
		,i.Price AS Price
		,i.MinLevel AS MinLevel
		,gt.[Name] AS 'Forbidden Game Type'
FROM Items AS i
LEFT JOIN GameTypeForbiddenItems As gtfi ON gtfi.ItemId = i.Id
LEFT JOIN GameTypes AS gt ON gt.Id = gtfi.GameTypeId
ORDER BY gt.[Name] DESC, i.[Name] ASC

--Problem 7.	Buy Items for User in Game
DECLARE @userIdAlex INT = (SELECT DISTINCT u.Id 
						FROM Users AS u
						JOIN UsersGames AS ug ON ug.UserId = u.Id AND u.Username = 'Alex'
						JOIN Games AS g ON ug.GameId = g.Id AND g.Name = 'Edinburgh')

DECLARE @alexGameIdEdinburgh INT = (SELECT ug.GameId
									FROM Users AS u
									JOIN UsersGames AS ug ON ug.UserId = u.Id AND u.Username = 'Alex'
									JOIN Games AS g ON ug.GameId = g.Id AND g.[Name]  = 'Edinburgh')

INSERT INTO UserGameItems(ItemId,UserGameId)
SELECT @userIdAlex, i.Id FROM Items AS i
									WHERE i.[Name] IN ('Blackguard', 'Bottomless Potion of Amplification', 'Eye of Etlich (Diablo III)', 'Gem of Efficacious Toxin', 'Golden Gorget of Leoric', 'Hellfire Amulet')

DECLARE @totalPrice INT = (SELECT SUM(i.Price) FROM Items AS i
									WHERE i.[Name] IN ('Blackguard', 'Bottomless Potion of Amplification', 'Eye of Etlich (Diablo III)', 'Gem of Efficacious Toxin', 'Golden Gorget of Leoric', 'Hellfire Amulet'))

UPDATE UsersGames
SET Cash -= @totalPrice
WHERE UserId = @userIdAlex AND GameId = @alexGameIdEdinburgh

SELECT u.Username
		, g.[Name]
		,ug.Cash
		,i.Name AS [Item Name]
FROM Users AS u
JOIN UsersGames AS ug ON ug.UserId = u.Id
JOIN Games AS g ON ug.GameId = g.Id AND g.[Name]  = 'Edinburgh'
JOIN UserGameItems AS ugi ON ugi.UserGameId = g.Id
JOIN Items AS i ON i.Id = ugi.ItemId
ORDER BY [Item Name]

--Problem 8.	Peaks and Mountains
SELECT p.PeakName
		,m.MountainRange AS Mountain
		,p.Elevation
FROM Peaks AS p
LEFT JOIN Mountains AS m ON m.Id = p.MountainId
ORDER BY p.Elevation DESC
		,p.PeakName ASC

--Problem 9.	Peaks with Their Mountain, Country and Continent
SELECT p.PeakName
		,m.MountainRange AS Mountain
		,c.CountryName
		,co.ContinentName
 
FROM Mountains AS m
 JOIN MountainsCountries AS mc ON mc.MountainId = m.Id
 JOIN Countries AS c ON c.CountryCode = mc.CountryCode
 JOIN Continents AS co ON co.ContinentCode = c.ContinentCode
 JOIN Peaks AS p ON p.MountainId = m.Id
ORDER BY p.PeakName ASC
		,c.CountryName ASC

--Problem 10.	Rivers by Country
WITH cte_RiversByCountries (CountryName,ContinentName,RiverName, RiverLength)
AS(
SELECT c.CountryName
		,co.ContinentName
		,r.RiverName
		,r.Length
FROM Countries AS c
LEFT JOIN CountriesRivers AS cr ON cr.CountryCode = c.CountryCode
LEFT JOIN Rivers AS r ON r.Id = cr.RiverId
LEFT JOIN Continents AS co ON co.ContinentCode = c.ContinentCode
)
SELECT c.CountryName
		,c.ContinentName
		,ISNULL(COUNT(c.RiverName),0) AS RiversCount
		,ISNULL(SUM(c.RiverLength),0) AS TotalLength
FROM cte_RiversByCountries AS c
GROUP BY c.CountryName, c.ContinentName
ORDER BY COUNT(c.RiverName) DESC
		,SUM(c.RiverLength) DESC
		,c.CountryName ASC

--Problem 11.	Count of Countries by Currency
SELECT cur.CurrencyCode
		,cur.Description AS Currency
		,COUNT(c.CurrencyCode) AS NumberOfCountries
FROM Countries AS c
RIGHT JOIN Currencies AS cur ON cur.CurrencyCode = c.CurrencyCode
GROUP BY cur.CurrencyCode, cur.Description 
ORDER BY COUNT(c.CurrencyCode) DESC
		, cur.Description ASC

--Problem 12.	Population and Area by Continent
SELECT co.ContinentName
		,SUM(c.AreaInSqKm) AS CountriesArea
		,SUM (CONVERT(bigint, c.Population)) AS CountriesPopulation
FROM Continents AS co
LEFT JOIN Countries AS c ON c.ContinentCode = co.ContinentCode
GROUP BY co.ContinentName
ORDER BY SUM (CONVERT(bigint, c.Population)) DESC

--Problem 13.	Monasteries by Country
CREATE TABLE Monasteries
(Id INT PRIMARY KEY IDENTITY
, Name NVARCHAR(50) NOT NULL
, CountryCode CHAR(2) FOREIGN KEY REFERENCES Countries(CountryCode) 
)

INSERT INTO Monasteries(Name, CountryCode) VALUES
('Rila Monastery “St. Ivan of Rila”', 'BG'), 
('Bachkovo Monastery “Virgin Mary”', 'BG'),
('Troyan Monastery “Holy Mother''s Assumption”', 'BG'),
('Kopan Monastery', 'NP'),
('Thrangu Tashi Yangtse Monastery', 'NP'),
('Shechen Tennyi Dargyeling Monastery', 'NP'),
('Benchen Monastery', 'NP'),
('Southern Shaolin Monastery', 'CN'),
('Dabei Monastery', 'CN'),
('Wa Sau Toi', 'CN'),
('Lhunshigyia Monastery', 'CN'),
('Rakya Monastery', 'CN'),
('Monasteries of Meteora', 'GR'),
('The Holy Monastery of Stavronikita', 'GR'),
('Taung Kalat Monastery', 'MM'),
('Pa-Auk Forest Monastery', 'MM'),
('Taktsang Palphug Monastery', 'BT'),
('Sümela Monastery', 'TR')

--ALTER TABLE Countries
--ADD IsDeleted BIT NOT NULL DEFAULT 0

UPDATE Countries
SET IsDeleted = 1
WHERE Countries.CountryCode IN(SELECT cr.CountryCode 
								FROM CountriesRivers AS cr
								GROUP BY CountryCode
								HAVING COUNT(RiverId)>3)

SELECT mo.Name AS [Monastery]
		,c.CountryName AS [Country]
FROM Monasteries AS mo
JOIN Countries AS c ON c.CountryCode = mo.CountryCode AND c.IsDeleted != 1
ORDER BY [Monastery]

--Problem 14.	Monasteries by Continents and Countries
UPDATE Countries
SET CountryName = 'Burma'
WHERE CountryName = 'Myanmar'

DECLARE @TanzID CHAR(2)= (SELECT CountryCode FROM Countries WHERE CountryName = 'Tanzania')
DECLARE @MyanID CHAR(2) = (SELECT CountryCode FROM Countries WHERE CountryName = 'Myanmar')

INSERT INTO Monasteries
VALUES
('Hanga Abbey',@TanzID)
,('Myin-Tin-Daik',@MyanID);

WITH cte_CountriesNotDeleted(CountryCode, CountryName, ContinentName)
AS(
SELECT c.CountryCode
		,c.CountryName
		,co.ContinentName
FROM Countries AS c
JOIN Continents AS co ON c.IsDeleted <> 1 ANd co.ContinentCode = c.ContinentCode
)
SELECT c.ContinentName
		,c.CountryName
		,COUNT (m.Name) AS [MonasteriesCount]
FROM cte_CountriesNotDeleted AS c
LEFT JOIN Monasteries AS m ON m.CountryCode = c.CountryCode
GROUP BY c.CountryCode, c.ContinentName,c.CountryName
ORDER BY [MonasteriesCount] DESC
		,[CountryName] ASC