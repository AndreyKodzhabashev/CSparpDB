
CREATE TABLE Cities
(
Id INT PRIMARY KEY IDENTITY
,[Name]	NVARCHAR(20) NOT NULL 
,CountryCode CHAR(2) NOT NULL
)

CREATE TABLE Hotels
(
Id INT PRIMARY KEY IDENTITY
,[Name]	NVARCHAR(30) NOT NULL 
,CityId	INT NOT NULL FOREIGN KEY REFERENCES Cities(Id)
,EmployeeCount INT NOT NULL
,BaseRate DECIMAL(15,2)	
)

CREATE TABLE Rooms
(
Id INT PRIMARY KEY Identity
,Price DECIMAL(15,2) NOT NULL
,[Type] NVARCHAR(20) NOT NULL
,Beds INT NOT NULL
,HotelId INT NOT NULL FOREIGN KEY REFERENCES Hotels(Id)
)

CREATE TABLE Trips
(
Id INT PRIMARY KEY Identity
,RoomId	INT NOT NULL FOREIGN KEY REFERENCES Rooms(Id)
,BookDate DATE NOT NULL
,ArrivalDate DATE NOT NULL
,ReturnDate	DATE NOT NULL
,CancelDate	DATE
)

CREATE TABLE Accounts
(
Id INT PRIMARY KEY Identity
,FirstName	NVARCHAR(50) NOT NULL
,MiddleName	NVARCHAR(20)	
,LastName NVARCHAR(50) NOT NULL
,CityId INT FOREIGN KEY REFERENCES Cities(Id)
,BirthDate Date	NOT NULL
,Email	VARCHAR(100) NOT NULL UNIQUE
)

ALTER TABLE Trips
ADD CONSTRAINT chk_BookDateIsBegoreArrival
CHECK(BookDate < ArrivalDate)
,CONSTRAINT chk_ArrivalDateIsBegoreReturn
CHECK(ArrivalDate<ReturnDate)

CREATE TABLE AccountsTrips
(
AccountId INT NOT NULL FOREIGN KEY REFERENCES Accounts(Id)
,TripId INT NOT NULL FOREIGN KEY REFERENCES Trips(Id)
,Luggage INT NOT NULL CHECK (Luggage >=0)
PRIMARY KEY (AccountId,TripId)
)

INSERT INTO Accounts VALUES
('John','Smith','Smith',34,'1975-07-21','j_smith@gmail.com')
,('Gosho',NULL,	'Petrov',11,'1978-05-16','g_petrov@gmail.com')
,('Ivan','Petrovich','Pavlov',59,'1849-09-26','i_pavlov@softuni.bg')
,('Friedrich','Wilhelm','Nietzsche',2,'1844-10-15',	'f_nietzsche@softuni.bg')

INSERT INTO Trips VALUES
 (101,'2015-04-12','2015-04-14','2015-04-20','2015-02-02')
,(102,'2015-07-07','2015-07-15','2015-07-22','2015-04-29')
,(103,'2013-07-17','2013-07-23','2013-07-24',NULL)
,(104,'2012-03-17','2012-03-31','2012-04-01','2012-01-10')
,(109,'2017-08-07','2017-08-28','2017-08-29',NULL)

--5
--Select all cities in Bulgaria. Order them by city name.
SELECT Id,Name
FROM Cities
WHERE CountryCode = 'BG'
ORDER BY Name

--6
--Select all full names and birth years from accounts, who are born after 1991.
--Order them by birth year (descending), then by first name (ascending). Keep in mind that middle names can be NULL 😊
SELECT CONCAT(FirstName,' ',ISNULL(MiddleName + ' ',''),LastName) AS [Full Name]
		,DatePart(YEAR,BirthDate) AS BirthYear
FROM Accounts
WHERE DatePart(YEAR,BirthDate)>'1991'
ORDER BY BirthYear DESC
		,FirstName ASC
--7
--Select accounts whose emails start with the letter “e”. Select their first and last name, their birthdate in the format "MM-dd-yyyy", and their city name.
--Order them by city name (descending)
SELECT a.FirstName
		,a.LastName
		,FORMAT(a.BirthDate,'MM-dd-yyyy') AS BirthDate
		,c.Name AS Hometown
		,a.Email
FROM Accounts AS a
JOIN Cities AS c ON c.Id = a.CityId
WHERE a.Email LIKE ('e%')
ORDER BY c.Name DESC

--8. City Statistics
--Select all cities with the count of hotels in them. Order them by the hotel count (descending), then by city name. 
--Include cities, which have no hotels in them as well.
SELECT c.[Name] AS City
		,COUNT(h.Id) AS Hotels 
FROM Cities AS c 
LEFT JOIN Hotels AS h ON h.CityId = c.Id
GROUP BY c.[Name]
ORDER BY COUNT(h.Id) DESC
		, c.[Name]

--9. Expensive First-Class Rooms
--Find all First-Class rooms and select the Id, Price, Hotel name and City name. 
--Order them by Price (descending), then by Room ID.

SELECT r.Id
		,r.Price
		,h.Name
		,c.Name
FROM Rooms AS r
JOIN Hotels AS h ON h.Id = r.HotelId AND r.Type = 'First Class'
JOIN Cities AS c ON c.Id = h.CityId
ORDER BY r.Price DESC
		,r.Id ASC

--10. Longest and Shortest Trips
--Find the longest and shortest trip for each account, in days. Filter the results to accounts with no middle name and trips, which aren’t cancelled (CancelDate is null).
--Order the results by Longest Trip days (descending), then by Account ID.

SELECT a.Id
		,a.FirstName +' '+ a.LastName
		,MAX(DATEDIFF(DAY,t.ArrivalDate,t.ReturnDate))
		,MIN(DATEDIFF(DAY,t.ArrivalDate,t.ReturnDate)) 
FROM Accounts AS a
JOIN AccountsTrips AS [at] ON [at].AccountId = a.Id
JOIN Trips AS t ON t.Id = [at].TripId
GROUP BY a.Id, a.FirstName, a.MiddleName,a.LastName, t.CancelDate
HAVING (a.MiddleName IS NULL AND t.CancelDate IS NULL)
ORDER BY MAX(DATEDIFF(DAY,t.ArrivalDate,t.ReturnDate)) DESC
		,a.Id ASC

--11. Metropolis
--Find the top 5 cities, which have the most registered accounts in them. Order them by the count of accounts (descending).
select top 5 c.Id
		,c.Name
		,c.CountryCode
		,count(a.Id)
from Cities as c 
join Accounts as a on a.CityId = c.Id
group by c.Id, c.Name, c.CountryCode
order by count(a.Id) desc

--12. Romantic Getaways
--Find all accounts, which have had one or more trips to a hotel in their hometown.
--Order them by the trips count (descending), then by Account ID.
select a.Id
		,a.Email
		,c.Name
		,count(t.Id)
from Accounts as a
join AccountsTrips as at on at.AccountId = a.Id
join Trips as t on t.Id = at.TripId
join Rooms as r on r.Id = t.RoomId
join Hotels as h on h.Id = r.HotelId
join Cities as c on c.Id = h.CityId and c.id = a.CityId
group by a.Id, a.Email, c.Name
order by count(t.Id) desc
		, a.Id

--13. Lucrative Destinations
--Find the top 10 cities’ Total Revenue Sum (Hotel Base Rate + Room Price) and trip count.
--Count only trips, which were booked in 2016.
--Order them by Total Revenue (descending), then by Trip count (descending)
select TOP 10 c.Id
		,c.Name
		,SUM(h.BaseRate + r.Price)
		,count(t.Id)		 
from Cities as c
join Hotels as h on c.Id = h.CityId
join Rooms as r on r.HotelId = h.id
right join Trips as t on t.RoomId = r.Id AND DATEPART(year,t.BookDate) = '2016'
WHERE DATEPART(year,t.BookDate) = '2016'
GROUP BY c.Id, c.Name
order by SUM(h.BaseRate + r.Price) DESC
		, count(t.Id) DESC

--14. Trip Revenues
--Find all trips’ revenue (hotel base rate + room price). If a trip is canceled, its revenue is always 0. Extract the trip’s ID, the hotel’s name, the room type and the revenue.
--Order the results by Room type, then by the Trip ID.
select t.Id
		,h.Name
		,r.Type
		, SUM( case when t.CancelDate is not null THEN 0
					else BaseRate + r.Price end)
from AccountsTrips as at
join Trips as t on t.Id = at.TripId
join Rooms as r on r.Id = t.RoomId
join hotels as h on h.Id = r.HotelId
GROUP BY  t.Id
		,h.Name
		,r.Type
order by r.Type, t.Id

--15. Top Travelers
--Find the top traveler for each country. The top traveler is the account, which has the most trips to that country.
--Order the results by the count of trips (descending), then by Account ID.


WITH cte_TEMP(Id,Email,CountryCode, Count, Rank)
AS
(
select DiSTINCT a.Id AS id
		,a.Email AS email
		,c.CountryCode as CountryCode
		,count(c.CountryCode) AS count
		,ROW_Number()OVER(PARTITION BY c.CountryCode ORDER BY COUNT(t.id)DESC) as rank
from Cities as c
join Hotels as h on h.CityId = c.Id
join Rooms as r on r.HotelId = h.Id
join Trips as t on t.RoomId = r.Id
join AccountsTrips as at on at.TripId = t.Id
join Accounts as a on a.Id = at.AccountId
GROUP BY a.Id, a.Email, c.CountryCode)

Select f.Id,f.Email, f.CountryCode, f.Count from cte_TEMP as f
where f.Rank = 1
order by f.Count desc
		,f.Id ASC

--16. Luggage Fees
--Apart from its base rate and room price, each hotel also has a hidden “luggage fee”. It’s in the terms and conditions, but nobody reads those…
--The luggage fee only comes into action if a trip has more than 5 items of luggage and it’s equal to the number of luggage items, multiplied by 5.
--Take into account only trips, which have more than 0 luggage. 
--Order the results by the count of luggage (descending)

Select at.TripId
		,SUM(at.Luggage)
		,CASE 
			WHEN SUM(at.Luggage) >5 then'$' + cast((SUM(at.Luggage)*5)as varchar)
			else '$0'
			end 
from AccountsTrips as at
Group by at.TripId
having (SUM(at.Luggage) > 0)
order by SUM(at.Luggage) desc

--17. GDPR Violation
--Retrieve the following information about each trip:
--•	Trip ID
--•	Account Full Name
--•	From – Account hometown
--•	To – Hotel city
--•	Duration – the duration between the arrival date and return date in days. If a trip is cancelled, the value is “Canceled”
select t.Id
		,CONCAT(FirstName,' ',ISNULL(MiddleName + ' ',''),LastName) AS [Full Name]
		,c.Name as [From]
		,ch.Name as [To]
		,Case
			when t.CancelDate is not null then 'Canceled'
			else CAST(DATEDIFF(day,t.ArrivalDate,t.ReturnDate ) as NVARCHAR) + ' days'
			end
from Trips as t
join AccountsTrips as at on at.TripId = t.Id
join Accounts as a on a.Id = at.AccountId
join Cities as c on c.Id = a.CityId
join Rooms as r on r.Id = t.RoomId
join Hotels as h on h.Id = r.HotelId
join Cities as ch on ch.Id = h.CityId
order by CONCAT(FirstName,' ',ISNULL(MiddleName + ' ',''),LastName)
		,t.Id

--18. Available Room
--Create a user defined function, named udf_GetAvailableRoom(@HotelId, @Date, @People), that receives a hotel ID, a desired date, and the count of people that are going to be signing up.
GO
CREATE FUNCTION udf_GetAvailableRoom(@HotelId INT, @Date DATE, @People INT)
RETURNS NVARCHAR(MAX)
BEGIN
	declare @result nvarchar(max), @arrival date, @return date;

	set @arrival = select from Trips as t where t.RoomId =  
	
	
	RETURN 
END

-- 	19. Switch Room
--Create a user defined stored procedure, named usp_SwitchRoom(@TripId, @TargetRoomId), that receives a trip and a target room, and attempts to move the trip to the target room. A room will only be switched if all of these conditions are true:
--•	If the target room ID is in a different hotel, than the trip is in, raise an exception with the message “Target room is in another hotel!”.
--•	If the target room doesn’t have enough beds for all the trip’s accounts, raise an exception with the message “Not enough beds in target room!”.
--If all the above conditions pass, change the trip’s room ID to the target room ID.
GO
CREATE PROCEDURE usp_SwitchRoom(@TripId INT, @TargetRoomId INT)
AS
	IF
	select h.Name 
	from Trips as t
	join Rooms as r ON r.Id = t.RoomId
	join Hotels as h on h.Id = r.HotelId
	
--20. Cancel Trip
--Create a trigger, which fires when a trip is deleted. Instead of deleting a trip, set its cancel date to the current date and IGNORE trips, which have already been canceled.
go
CREATE TRIGGER trg_InsteadOfDeleteTrips
ON Trips
INSTEAD OF DELETE
AS 
 UPDATE Trips
 SET CancelDate = GETDATE()
 WHERE Id IN (SELECT d.Id FROM deleted as d) AND CancelDate IS NULL	