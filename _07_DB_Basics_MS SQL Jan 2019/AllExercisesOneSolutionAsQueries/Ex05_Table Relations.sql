
--Problem 1.	One-To-One Relationship
CREATE TABLE Persons(
PersonID INT NOT NULL
,FirstName NVARCHAR (20) NOT NULL
,Salary DECIMAL(15,2) DEFAULT 0
,PassportID INT NOT NULL
)

CREATE TABLE Passports(
PassportID INT NOT NULL
, PassportNumber NVARCHAR(20) NOT NULL
)

ALTER TABLE Passports
ADD CONSTRAINT PK_PassportID
PRIMARY KEY (PassportID)

ALTER TABLE Persons
ADD CONSTRAINT PK_Persons
PRIMARY KEY (PersonID)

, CONSTRAINT FK_PersonID_PassportID
FOREIGN KEY (PassportID) 
REFERENCES Passports(PassportID)

INSERT INTO Passports
VALUES
(101,'N34FG21B')
,(102,'K65LO4R7')
  ,(103,'ZE657QP2')    
  
INSERT INTO Persons
VALUES
(1,'Roberto',43300.00,102)
,(2,'Tom',56100.00,103)
  ,(3,'Yana',60200.00,101)

--Problem 2.	One-To-Many Relationship
CREATE TABLE Manufacturers(
ManufacturerID INT NOT NULL
,[Name] NVARCHAR(50) NOT NULL
,EstablishedOn DATE NOT NULL

CONSTRAINT PK_Manufacturers
PRIMARY KEY (ManufacturerID))

CREATE TABLE Models(
ModelID INT NOT NULL
,[Name] NVARCHAR(40) NOT NULL
,ManufacturerID INT NOT NULL

CONSTRAINT PK_Models
PRIMARY KEY(ModelID)

,CONSTRAINT FK_Models_Manufacturers
FOREIGN KEY(ManufacturerID)
REFERENCES Manufacturers(ManufacturerID)
)

INSERT INTO Manufacturers
VALUES
(1, 'BMW', '07/03/1916')
,(2,'Tesla','01/01/2003')
,(3,'Lada', '01/05/1966')
GO
INSERT INTO Models
VALUES
(101,'X1',1)
,(102,'i6',1)
,(103,'Model S',2)
,(104,'Model X',2)
,(105,'Model 3',2)
,(106,'Nova',3)

--Problem 3.	Many-To-Many Relationship
CREATE TABLE Students(
StudentID INT NOT NULL
,[Name] NVARCHAR(30)

CONSTRAINT PK_Students
PRIMARY KEY (StudentID)
)

CREATE TABLE Exams(
ExamID INT NOT NULL
,[Name] NVARCHAR(30)

CONSTRAINT PK_Exams
PRIMARY KEY (ExamID)
)

CREATE TABLE StudentsExams(
StudentID INT NOT NULL
,ExamID INT NOT NULL

CONSTRAINT PK_StudentsExams
PRIMARY KEY(StudentID,ExamID)

,CONSTRAINT  FK_StudentsExams_Students
FOREIGN KEY (StudentID)
REFERENCES Students(StudentID)

,CONSTRAINT FK_StudentsExams_Exams
FOREIGN KEY (ExamID)
REFERENCES Exams(ExamID)
)
INSERT INTO Students
VALUES
(1,'Mila')                                
,(2,'Toni')
,(3,'Ron')

INSERT INTO Exams
VALUES
(101,'SpringMVC')
,(102,'Neo4j')
,(103,'Oracle 11g')
				
INSERT INTO StudentsExams
VALUES
(1,101)
,(1,102)
,(2,101)
,(3,103)
,(2,102)
,(2,103)

--Problem 4.	Self-Referencing 
CREATE TABLE Teachers(
TeacherID INT NOT NULL	
,[Name]	NVARCHAR(30)
,ManagerID INT 

CONSTRAINT PK_Teachers
PRIMARY KEY (TeacherID)

,CONSTRAINT FK_SelfReference
FOREIGN KEY (ManagerID)
REFERENCES  Teachers(TeacherID)
)

INSERT INTO Teachers
VALUES
(101,'John',NULL)
,(102,'Maya',106)
,(103,'Silvia',106)
,(104,'Ted',105)
,(105,'Mark',101)
,(106,'Greta',101)

--Problem 5.	Online Store Database
USE master
GO
CREATE DATABASE OnlineStore

CREATE TABLE Cities(
CityID INT NOT NULL
,[Name] VARCHAR(50) NOT NULL

CONSTRAINT PK_Cities
PRIMARY KEY (CityID)
)

CREATE TABLE Customers(
CustomerID INT NOT NULL
,[Name] VARCHAR(50) NOT NULL
, Birthday DATE 
, CityID INT NOT NULL

CONSTRAINT PK_Customers
PRIMARY KEY (CustomerID)

,CONSTRAINT FK_Customers_Cities
FOREIGN KEY(CityID)
REFERENCES Cities(CityID)
)

CREATE TABLE Orders(
OrderID INT NOT NULL
,CustomerID INT NOT NULL

CONSTRAINT PK_Orders
PRIMARY KEY (OrderID)

,CONSTRAINT FK_Orders_Customers
FOREIGN KEY(CustomerID)
REFERENCES Customers(CustomerID)
)

CREATE TABLE ItemTypes (
ItemTypeID INT NOT NULL
,[Name] VARCHAR(50) NOT NULL

, CONSTRAINT PK_ItemTypes
PRIMARY KEY (ItemTypeID)
)

CREATE TABLE Items(
ItemID INT NOT NULL
,[Name] VARCHAR(50) NOT NULL
,ItemTypeID INT NOT NULL

, CONSTRAINT PK_Items
PRIMARY KEY (ItemID)
, CONSTRAINT FK_Items_ItemTypes
FOREIGN KEY(ItemTypeID)
REFERENCES ItemTypes(ItemTypeID)
)

CREATE TABLE OrderItems(
OrderID INT NOT NULL
,ItemID INT NOT NULL

CONSTRAINT PK_OrderItems
PRIMARY KEY(OrderID,ItemID)

,CONSTRAINT FK_OrderItems_Orders
FOREIGN KEY (OrderID)
REFERENCES Orders(OrderID)

,CONSTRAINT FK_OrderItems_Items
FOREIGN KEY (ItemID)
REFERENCES Items(ItemID)
)

--Problem 6.	University Database

USE master
GO
CREATE DATABASE TEST
GO
USE TEST
CREATE TABLE Subjects(
SubjectID INT NOT NULL
,SubjectName VARCHAR(50) NOT NULL
CONSTRAINT PK_Subjects
PRIMARY KEY (SubjectID)
)

CREATE TABLE Majors(
MajorID INT NOT NULL
,NAME VARCHAR(50)

CONSTRAINT PK_Majors
PRIMARY KEY (MajorID)
)

CREATE TABLE Students(
StudentID INT NOT NULL
,StudentNumber INT 
,StudentName VARCHAR(50) NOT NULL
,MajorID INT NOT NULL

CONSTRAINT PK_Students
PRIMARY KEY (StudentID)

, CONSTRAINT FK_Students_Majors
FOREIGN KEY (MajorID)
REFERENCES Majors(MajorID)
)

CREATE TABLE Agenda(
StudentID INT NOT NULL
,SubjectID INT NOT NULL

CONSTRAINT PK_Agenda
PRIMARY KEY (StudentID,SubjectID)

,CONSTRAINT FK_Agenda_Students
FOREIGN KEY (StudentID)
REFERENCES Students(StudentID)

,CONSTRAINT FK_Agenda_Subjects
FOREIGN KEY (SubjectID)
REFERENCES Subjects(SubjectID)
)
CREATE TABLE Payments(
PaymentID INT NOT NULL
,PaymentDate DATE NOT NULL
,PaymentsAmount DECIMAL(15,2) NOT NULL
,StudentID INT NOT NULL

CONSTRAINT PK_Payments
PRIMARY KEY (PaymentID)

,CONSTRAINT FK_Payments_Students
FOREIGN KEY (StudentID)
REFERENCES Students(StudentID)
)


--Problem 9.	*Peaks in Rila
--Display all peaks for "Rila" mountain. Include:
--�	MountainRange
--�	PeakName
--�	Elevation
--Peaks should be sorted by elevation descending

USE Geography

SELECT m.MountainRange
	, p.PeakName
	, p.Elevation
FROM Mountains AS m
LEFT JOIN Peaks AS p
ON p.MountainId = m.Id
WHERE m.MountainRange = 'Rila'
ORDER BY p.Elevation DESC
