--Problem 1.	Create Database
CREATE DATABASE Minions;
GO
USE Minions;
GO
--Problem 2.	Create Tables
CREATE TABLE Minions(
Id INT NOT NULL,
[Name] NVARCHAR(64) NOT NULL,
Age INT

CONSTRAINT PK_Minions
PRIMARY KEY(Id)
);
GO

CREATE TABLE Towns(
Id INT NOT NULL,
[Name] NVARCHAR(64) NOT NULL

CONSTRAINT PK_Towns
PRIMARY KEY(Id)
);
GO
ALTER TABLE Minions
ADD TownId INT NOT NULL

CONSTRAINT FK_Minions_Towns
FOREIGN KEY (TownId)
REFERENCES Towns(Id);
GO

-- Problem 4.	Insert Records in Both Tables
INSERT INTO Towns
VALUES
(1,'Sofia'),
(2,'Plovdiv'),
(3,'Varna');
GO
INSERT INTO Minions
VALUES
(1, 'Kevin', 22, 1),
(2, 'Bob', 15, 3),
(3, 'Steward', NULL, 2);

-- Problem 5.	Truncate Table Minions
TRUNCATE TABLE Minions;

-- Problem 6.	Drop All Tables
ALTER TABLE Minions
DROP FK_Minions_Towns;
GO
DROP TABLE Towns;
GO
DROP TABLE Minions;
GO

--Problem 7.	Create Table People
CREATE TABLE People(
Id INT NOT NULL IDENTITY,
[Name] NVARCHAR(200) NOT NULL,
Picture VARBINARY(MAX),
Height DECIMAL(10,2),
[Weight] DECIMAL(10,2),
Gender CHAR(1) NOT NULL CHECK (Gender = 'f' OR Gender = 'm'), 
Birthdate DATE NOT NULL,
Biography NVARCHAR(MAX)

CONSTRAINT PK_PeopleId
PRIMARY KEY (Id)
);
GO
INSERT INTO People
VALUES
('Stela',Null,1.65,44.55,'f','2000-09-22',Null),
('Ivan',Null,2.15,95.55,'m','1989-11-02',Null),
('Qvor',Null,1.55,33.00,'m','2010-04-11',Null),
('Karolina',Null,2.15,55.55,'f','2001-11-11',Null),
('Pesho',Null,1.85,90.00,'m','1983-07-22',Null)

--Problem 8.	Create Table Users
CREATE TABLE Users(
Id INT NOT NULL IDENTITY,
Username VARCHAR(30) NOT NULL UNIQUE,
[Password] VARCHAR(26) NOT NULL,
ProfilePicture VARBINARY(900),
LastLoginTime DATETIME2 ,
IsDeleted BIT

CONSTRAINT PK_UserId
PRIMARY KEY (Id)
);
INSERT INTO Users
VALUES
('Stela USERNAME',12345,Null,GETDATE(),1),
('Ivan USERNAME',12356,Null,GETDATE(),1),
('Qvor USERNAME',12323,Null,GETDATE(),1),
('Karolina USERNAME',12342,Null,GETDATE(),1),
('Pesho USERNAME',1233424,Null,GETDATE(),1);

--Problem 9.	Change Primary Key
ALTER TABLE Users
DROP PK_UserID
GO
ALTER TABLE Users
ADD CONSTRAINT PK_UserID_Username
PRIMARY KEY(Id,Username)

--Problem 10.	Add Check Constraint
ALTER TABLE Users
ADD CONSTRAINT CHK_PasswordLength
CHECK (LEN([Password]) >=5);

--Problem 11.	Set Default Value of a Field
ALTER TABLE Users 
ADD CONSTRAINT DF_LastLoginTime
DEFAULT GETDATE()
FOR LastLoginTime

--Problem 12.	Set Unique Field
ALTER TABLE Users
--DROP PK_UserID_Username
ADD CONSTRAINT PK_Id
PRIMARY KEY (Id);

ALTER TABLE Users
ADD CONSTRAINT CHK_UserNameUNIQUEand3CharsLong
CHECK(LEN(Username) >=3)

ALTER TABLE Users
ADD UNIQUE (Username)

ALTER TABLE Users
ADD CONSTRAINT UQ_Username
UNIQUE(Username);

--Problem 13.	Movies Database
CREATE DATABASE Movies
USE Movies

CREATE TABLE Directors(
Id INT NOT NULL IDENTITY,
DirectorName NVARCHAR(40) NOT NULL,
Notes NVARCHAR(MAX)

CONSTRAINT PK_DirectorId
PRIMARY KEY(Id)
);

CREATE TABLE Genres(
Id INT NOT NULL IDENTITY,
GenreName NVARCHAR(40) NOT NULL,
Notes NVARCHAR(MAX)

CONSTRAINT PK_GenreId
PRIMARY KEY(Id)
);

CREATE TABLE Categories(
Id INT NOT NULL IDENTITY,
CategoryName NVARCHAR(40) NOT NULL,
Notes NVARCHAR(MAX)

CONSTRAINT PK_CategoryId
PRIMARY KEY(Id)
);

CREATE TABLE Movies(
Id INT NOT NULL IDENTITY,
Title NVARCHAR(40) NOT NULL,
DirectorId INT NOT NULL,
CopyrightYear DATE NOT NULL,
[Length] INT NOT NULL,
GenreId INT NOT NULL,
CategoryId INT NOT NULL,
Rating INT,
Notes NVARCHAR(MAX)

CONSTRAINT PK_MovieId
PRIMARY KEY(Id),
CONSTRAINT UQ_TitelUnique
UNIQUE (Title),
CONSTRAINT FK_Movies_Directors
FOREIGN KEY (DirectorId)
REFERENCES Directors(Id),
CONSTRAINT FK_Movies_Genres
FOREIGN KEY (GenreId)
REFERENCES Genres(Id),
CONSTRAINT FK_Movies_Category
FOREIGN KEY (CategoryId)
REFERENCES Categories(Id)
);
SELECT * FROM Movies
INSERT INTO Directors
VALUES
('Ivan', NULL),('Dragan', NULL),('Petkan', 'Director of the YEAR 2018'),('Stamat', NULL),('Veneta', NULL);

INSERT INTO Genres
VALUES
('Action', NULL),('Comedy', NULL),('Drama', NULL),('Manga', 'JAPAN Cartoons'),('Anime', NULL);

INSERT INTO Categories
VALUES
('ONE Star', NULL),('TWO Stars', NULL),('THREE Stars', NULL),('FOUR Stars', NULL),('FIVE Stars', NULL);

INSERT INTO Movies
VALUES
('First Movie',2,'2016',100,1,5,10,NULL),
('Second Movie',5,'2017',100,2,3,11,NULL),
('Third Movie',1,'2010',100,3,4,5,NULL),
('Fourth Movie',4,'2001',100,4,2,17,NULL),
('Fifth Movie',2,'2014',100,5,1,20,NULL);

SELECT * FROM Categories

--Problem 14.	Car Rental Database
CREATE DATABASE CarRental
GO
USE CarRental

CREATE TABLE Categories(
Id INT NOT NULL IDENTITY, 
CategoryName NVARCHAR (30) NOT NULL, 
DailyRate INT, 
WeeklyRate INT, 
MonthlyRate INT, 
WeekendRate INT

CONSTRAINT PK_CategoriesId
PRIMARY KEY(Id)
);

CREATE TABLE Cars(
Id INT NOT NULL IDENTITY, 
PlateNumber VARCHAR (10) NOT NULL, 
Manufacturer VARCHAR(20) NOT NULL, 
Model VARCHAR(10) NOT NULL, 
CarYear DATE NOT NULL, 
CategoryId INT , 
Doors INT, 
Picture VARBINARY(MAX), 
Condition VARCHAR(10), 
Available BIT NOT NULL DEFAULT 0

CONSTRAINT PK_CarId
PRIMARY KEY (Id),
CONSTRAINT FK_Cars_Categories
FOREIGN KEY (CategoryId)
REFERENCES Categories(Id)
);

CREATE TABLE Employees(
Id INT NOT NULL IDENTITY,
FirstName NVARCHAR(10) NOT NULL, 
LastName NVARCHAR(10) NOT NULL,
Title CHAR(3), 
Notes NVARCHAR(Max)

CONSTRAINT PK_EmployeeId
PRIMARY KEY (Id),
);

CREATE TABLE Customers(
Id INT NOT NULL IDENTITY,
DriverLicenceNumber VARCHAR(10) NOT NULL, 
FullName NVARCHAR(20) NOT NULL, 
[Address] NVARCHAR(10)NOT NULL, 
City NVARCHAR(10) NOT NULL, 
ZIPCode INT NOT NULL, 
Notes NVARCHAR(Max)

CONSTRAINT PK_CustomerId
PRIMARY KEY (Id),
);

CREATE TABLE RentalOrders(
Id INT NOT NULL IDENTITY, 
EmployeeId INT NOT NULL, 
CustomerId INT NOT NULL, 
CarId INT NOT NULL, 
TankLevel INT NOT NULL DEFAULT 0, 
KilometrageStart INT NOT NULL DEFAULT 0, 
KilometrageEnd INT NOT NULL, 
TotalKilometrage INT NOT NULL, 
StartDate DATE NOT NULL, 
EndDate DATE NOT NULL, 
TotalDays INT NOT NULL, 
RateApplied INT, 
TaxRate DECIMAL (10,2) NOT NULL, 
OrderStatus BIT NOT NULL DEFAULT 1, 
Notes NVARCHAR (MAX)

CONSTRAINT PK_RentalOrdersId
PRIMARY KEY (Id),
CONSTRAINT FK_RentalOrder_Employees
FOREIGN KEY (EmployeeId)
REFERENCES Employees(Id),
CONSTRAINT FK_RentalOrder_Customers
FOREIGN KEY (CustomerId)
REFERENCES Customers(Id),
CONSTRAINT FK_RentalOrder_Cars
FOREIGN KEY (CarId)
REFERENCES Cars(Id)
);

INSERT INTO Categories
VALUES ('First',2,2,2,2),('Second',2,2,2,2),('Third',2,2,2,2);

INSERT INTO Cars
VALUES
('CA1111AA','FORD','Ka', '1990', 1, 2, NULL, 'Old',1),
('CA2222AA','VW','Passat', '2006', 2, 5, NULL, 'Ok',1),
('CA3333AA','Skoda','Octavia', '2015', 2, 4, NULL, 'New',1);

INSERT INTO Customers
VALUES
('AA13aBB55', 'Ivan Ivanov', 'Lyulin', 'Sofia',1409,NULL),
('CC17aBB55', 'Petar Petrov', 'Suha reka', 'Sofia',1220,NULL),
('BB14aBB55', 'Stamat Stamatov', 'Mladost', 'Sofia', 1133, NULL);

INSERT INTO Employees
VALUES
('Spas', 'Ivanov', NULL,NULL),
('Pipi', 'Toneva', NULL,NULL),
('Kiro', 'Pavlov', NULL,NULL);

INSERT INTO RentalOrders
VALUES
(3,1,1,45,0,1000, 1000,'01/01/2015', '01/15/2015', 15, 3, 20, 1,NULL),
(1,3,2,60,1300,5000, 3700,'01/01/2015', '01/15/2015', 15, 1, 40, 1,NULL),
(2,2,3,85,50,150, 100,'02/01/2015', '02/12/2015', 12, 2, 60, 1,NULL);

-- Problem 15.	Hotel Database
CREATE DATABASE Hotel;
USE Hotel;
CREATE TABLE Employees(
Id INT PRIMARY KEY IDENTITY NOT NULL,
FirstName VARCHAR(50),
LastName VARCHAR(50),
Title VARCHAR(50),
Notes VARCHAR(MAX)
)
 
INSERT INTO Employees
VALUES
('Velizar', 'Velikov', 'Receptionist', 'Nice customer'),
('Ivan', 'Ivanov', 'Concierge', 'Nice one'),
('Elisaveta', 'Bagriana', 'Cleaner', 'Poetesa')
 
CREATE TABLE Customers(
Id INT PRIMARY KEY IDENTITY NOT NULL,
AccountNumber BIGINT,
FirstName VARCHAR(50),
LastName VARCHAR(50),
PhoneNumber VARCHAR(15),
EmergencyName VARCHAR(150),
EmergencyNumber VARCHAR(15),
Notes VARCHAR(100)
)
 
INSERT INTO Customers
VALUES
(123456789, 'Ginka', 'Shikerova', '359888777888', 'Sistry mi', '7708315342', 'Kinky'),
(123480933, 'Chaika', 'Stavreva', '359888777888', 'Sistry mi', '7708315342', 'Lawer'),
(123454432, 'Mladen', 'Isaev', '359888777888', 'Sistry mi', '7708315342', 'Wants a call girl')
 
CREATE TABLE RoomStatus(
Id INT PRIMARY KEY IDENTITY NOT NULL,
RoomStatus BIT,
Notes VARCHAR(MAX)
)
 
INSERT INTO RoomStatus(RoomStatus, Notes)
VALUES
(1,'Refill the minibar'),
(2,'Check the towels'),
(3,'Move the bed for couple')
 
CREATE TABLE RoomTypes(
RoomType VARCHAR(50) PRIMARY KEY,
Notes VARCHAR(MAX)
)
 
INSERT INTO RoomTypes (RoomType, Notes)
VALUES
('Suite', 'Two beds'),
('Wedding suite', 'One king size bed'),
('Apartment', 'Up to 3 adults and 2 children')
 
CREATE TABLE BedTypes(
BedType VARCHAR(50) PRIMARY KEY,
Notes VARCHAR(MAX)
)
 
INSERT INTO BedTypes
VALUES
('Double', 'One adult and one child'),
('King size', 'Two adults'),
('Couch', 'One child')
 
CREATE TABLE Rooms (
RoomNumber INT PRIMARY KEY IDENTITY NOT NULL,
RoomType VARCHAR(50) FOREIGN KEY REFERENCES RoomTypes(RoomType),
BedType VARCHAR(50) FOREIGN KEY REFERENCES BedTypes(BedType),
Rate DECIMAL(6,2),
RoomStatus NVARCHAR(50),
Notes NVARCHAR(MAX)
)
 
INSERT INTO Rooms (Rate, Notes)
VALUES
(12,'Free'),
(15, 'Free'),
(23, 'Clean it')

CREATE TABLE Payments(
Id INT PRIMARY KEY IDENTITY NOT NULL,
EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
PaymentDate DATE,
AccountNumber BIGINT,
FirstDateOccupied DATE,
LastDateOccupied DATE,
TotalDays AS DATEDIFF(DAY, FirstDateOccupied, LastDateOccupied),
AmountCharged DECIMAL(14,2),
TaxRate DECIMAL(8, 2),
TaxAmount DECIMAL(8, 2),
PaymentTotal DECIMAL(15, 2),
Notes VARCHAR(MAX)
)
 SELECT * FROM Payments;
 TRUNCATE TABLE Payments
INSERT INTO Payments (EmployeeId, PaymentDate, AmountCharged, FirstDateOccupied, LastDateOccupied, TaxRate)
VALUES
(1, '12/12/2018', 2000.40, '01/01/2015', '01/15/2015', 10),
(2, '12/12/2018', 1500.40,'01/01/2015', '01/05/2015', 15),
(3, '12/12/2018', 1000.40,'01/01/2015', '01/15/2015', 20)
 
CREATE TABLE Occupancies(
Id  INT PRIMARY KEY IDENTITY NOT NULL,
EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
DateOccupied DATE,
AccountNumber BIGINT,
RoomNumber INT FOREIGN KEY REFERENCES Rooms(RoomNumber),
RateApplied DECIMAL(6,2),
PhoneCharge DECIMAL(6,2),
Notes VARCHAR(MAX)
)
 
INSERT INTO Occupancies (EmployeeId, RateApplied, Notes) VALUES
(1, 55.55, 'too'),
(2, 15.55, 'much'),
(3, 35.55, 'typing')

--Problem 19.	Basic Select All Fields
SELECT * FROM Towns;
SELECT * FROM Departments;
SELECT * FROM Employees;

--Problem 20.	Basic Select All Fields and Order Them
SELECT * FROM Towns
ORDER BY Name;
SELECT * FROM Departments
ORDER BY Name;
SELECT * FROM Employees
ORDER BY Salary DESC;

--Problem 21.	Basic Select Some Fields
SELECT Name FROM Towns
ORDER BY Name;
SELECT Name FROM Departments
ORDER BY Name;
SELECT FirstName, LastName, JobTitle, Salary FROM Employees
ORDER BY Salary DESC;

--Problem 22.	Increase Employees Salary
USE SoftUni;
UPDATE Employees
SET Salary *= 1.1;

SELECT Salary FROM Employees

--Problem 23.	Decrease Tax Rate
UPDATE Payments
SET TaxRate *= 0.97;
SELECT TaxRate FROM Payments;

--Problem 24.	Delete All Records
TRUNCATE TABLE Occupancies; 