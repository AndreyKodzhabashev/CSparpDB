
CREATE TABLE Students
(
Id INT PRIMARY KEY identity
,FirstName nvarchar(30) not null
,MiddleName nvarchar(25)
,LastName nvarchar(30) NOT NULL
,Age int check(age >=5 or age <=100)
,Address nvarchar(50)
,Phone nchar(10)

)

CREATE TABLE Subjects
(
Id INT PRIMARY KEY identity
,Name nvarchar(20) NOT NULL
,Lessons int check(Lessons>0) Not null
)

CREATE TABLE StudentsSubjects
(
Id INT PRIMARY KEY identity
,StudentId int foreign key references Students(Id) not null
,SubjectId int foreign key references Subjects(Id) not null
,Grade decimal(3,2) check(Grade between 2 and 6)
)

CREATE TABLE Exams
(
Id INT PRIMARY KEY identity
,Date datetime
,SubjectId int foreign key references Subjects(Id) not null
)

CREATE TABLE StudentsExams
(
StudentId int foreign key references Students(Id) not null
,ExamId int foreign key references Exams(Id) not null
,Grade decimal(3,2) check(Grade between 2 and 6)

PRIMARY KEY(StudentId,ExamId)
)

CREATE TABLE Teachers
(
Id INT PRIMARY KEY identity
,FirstName nvarchar(20) not null
,LastName nvarchar(20) not null
,Address nvarchar(20) not null
,Phone char(10)
,SubjectId int foreign key references Subjects(Id) not null
)

CREATE TABLE StudentsTeachers
(
StudentId int foreign key references Students(Id) not null
,TeacherId  int foreign key references Teachers(Id) not null

primary key(StudentId,TeacherId)
)

--2 insert
insert into Teachers values
('Ruthanne','Bamb','84948 Mesta Junction','3105500146',6)
,('Gerrard','Lowin','370 Talisman Plaza','3324874824',2)
,('Merrile','Lambdin','81 Dahle Plaza',	'4373065154',5)
,('Bert','Ivie','2 Gateway Circle','4409584510',4)

insert into Subjects values
('Geometry',12)
,('Health',10)
,('Drama',7)
,('Sports',9)

--3 update
--Make all grades 6.00, where the subject id is 1 or 2, if the grade is above or equal to 5.50
update StudentsSubjects
set Grade = 6
where SubjectId in (1,2) and Grade>=5.5

--4 Delete
--Delete all teachers, whose phone number contains ‘72’.
DELETE StudentsTeachers
where TeacherId in (select id from Teachers where Phone like ('%72%'))

DELETE FROM Teachers
where Phone like('%72%')

--5. Teen Students
--Select all students who are teenagers (their age is above or equal to 12). Order them by first name (alphabetically), then by last name (alphabetically). Select their first name, last name and their age.
select FirstName,LastName,Age
from Students
where age >=12
order by FirstName,LastName

--6. Cool Addresses
--Select all full names from students, whose address text contains ‘road’.
--Order them by first name (alphabetically), then by last name (alphabetically), then by address text (alphabetically).
select FirstName + ' '+ ISNULL(MiddleName + ' ',' ') + LastName
		,Address
from Students
where Address like ('%road%')
order by FirstName,LastName,Address

--7. 42 Phones
--Select students with middle names whose phones starts with 42. Select their first name, address and phone number. Order them by first name alphabetically.
select FirstName
		,Address
		,Phone
from Students
where Phone like ('42%') and MiddleName is not null
order by FirstName

--8. Students Teachers
--Select all students and the count of teachers each one has. 
select s.FirstName
		,s.LastName
		,count(st.TeacherId) 
from Students as s 
join StudentsTeachers as st on st.StudentId = s.Id
group by s.Id,FirstName, s.LastName
order by LastName

--9. Subjects with Students
--Select all teachers’ full names and the subjects they teach with the count of lessons in each. Finally select the count of students each teacher has. Order them by students count descending.
select t.FirstName + ' ' + t.LastName
		,s.Name  + '-' + cast(s.Lessons as nvarchar)
		,COUNT(st.StudentId)
from Teachers as t
join Subjects as s on s.Id = t.SubjectId
join StudentsTeachers as st on st.TeacherId = t.Id
group by t.FirstName,t.LastName,s.Name, s.Lessons
order by COUNT(st.StudentId) desc

--10. Students to Go
--Find all students, who have not attended an exam. Select their full name (first name + last name).
--Order the results by full name (ascending).
select FirstName + ' ' + LastName as [Full Name]
from Students as s
left join StudentsExams as se on se.StudentId = s.Id
where se.StudentId is NULL
order by [Full Name] ASC

--11. Busiest Teachers
--Find top 10 teachers with most students they teach. Select their first name, last name and the amount of students they have. Order them by students count (descending), then by first name (ascending), then by last name (ascending).
select top 10 t.FirstName
		,t.LastName
		,count(st.StudentId)
from Teachers as t
join StudentsTeachers as st on st.TeacherId = t.Id
group by t.FirstName, t.LastName
order by count(st.StudentId) desc
		,FirstName asc
		,LastName asc

--12. Top Students
--Find top 10 students, who have highest average grades from the exams.
--Format the grade, two symbols after the decimal point.
--Order them by grade (descending), then by first name (ascending), then by last name (ascending)

select top 10 s.FirstName
		,s.LastName
		,cast(avg(se.Grade) as decimal(3,2)) as GradeSum
from Students as s
left join StudentsExams as se on se.StudentId = s.Id
group by FirstName, LastName
order by avg(se.Grade) desc
		,s.FirstName asc
		,s.LastName asc

--13. Second Highest Grade
--Find the second highest grade per student from all subjects. Sort them by first name (ascending), then by last name (ascending).
select f.FirstName
		,f.LastName
		,f.Grade
from(
select s.FirstName as FirstName
		,s.LastName as LastName
		,ss.Grade as Grade
		,ROW_NUMBER()over(partition by s.id order by ss.Grade desc) as [Rank]
		
from Students as s
join StudentsSubjects as ss on ss.StudentId = s.Id) as f
where f.Rank = 2
order by f.FirstName asc
		,f.LastName asc

--14. Not So In The Studying
--Find all students who don’t have any subjects. Select their full name. The full name is combination of first name, middle name and last name. Order the result by full name
--NOTE: If the middle name is null you have to concatenate the first name and last name separated with single space.
select FirstName + ' '+ ISNULL(MiddleName + ' ','') + LastName as [Full Name]
		
from Students as s
left join StudentsSubjects as ss on ss.StudentId = s.Id
where ss.StudentId is null
order by [Full Name]

--15. Top Student per Teacher
--Find all teachers with their top students. The top student is the person with highest average grade. Select teacher full name (first name + last name), subject name, student full name (first name + last name) and corresponding grade. The grade must be formatted to the second digit after the decimal point.
--Sort the results by subject name (ascending), then by teacher full name (ascending), then by grade (descending)
select t.FirstName + ' ' + t.LastName as  [Teacher Full Name]
		,sb.Name as [Subject Name]
		,s.FirstName + ' '+ ISNULL(s.MiddleName + ' ',' ') + s.LastName as [Student Full Name]
		,avg(ss.Grade) as [Grade]
from Students as s
join StudentsSubjects as ss on ss.SubjectId = s.Id
join  Subjects as sb on sb.Id = ss.StudentId
join Teachers as t on t.SubjectId = ss.SubjectId
join StudentsTeachers as st on st.TeacherId = t.id

group by t.FirstName, t.LastName, sb.Name, s.FirstName,s.MiddleName,s.LastName
order by [Teacher Full Name] asc
		,[Grade] desc


--16. Average Grade per Subject
--Find the average grade for each subject. Select the subject name and the average grade. 
--Sort them by subject id (ascending).

select s.Name
		,avg(ss.Grade) as [AverageGrade]
from Subjects as s
join StudentsSubjects as ss on ss.SubjectId = s.Id
group by s.id,s.Name
order by s.id asc

--17. Exams Information 

SELECT
	t.Q,
	t.[Name],
	COUNT(t.StudentId) AS StudentsCount
FROM (SELECT
	CASE 
		WHEN MONTH(e.Date) BETWEEN 1 AND 3 THEN 'Q1'
		WHEN MONTH(e.Date) BETWEEN 4 AND 6 THEN 'Q2'
		WHEN MONTH(e.Date) BETWEEN 7 AND 9 THEN 'Q3'
		WHEN MONTH(e.Date) BETWEEN 10 AND 12 THEN 'Q4'
		WHEN e.Date IS NULL THEN 'TBA'
	END AS Q,
	s.[Name],
	se.StudentId
FROM Subjects AS s
LEFT JOIN Exams AS e ON e.SubjectId = s.Id
LEFT JOIN StudentsExams AS se ON se.ExamId = e.Id
WHERE se.Grade >=4.00) AS t
GROUP BY t.Q,t.[Name]
ORDER BY t.Q

--18. Exam Grades
--Create a user defined function, named udf_ExamGradesToUpdate(@studentId, @grade), that receives a student id and grade.
go
create function udf_ExamGradesToUpdate(@studentId int, @grade decimal(3,2))
returns nvarchar(max)
as
begin
	
	if	((SELECT s.Id from Students as s where s.Id = @studentId ) is null)
		begin
		 return 'The student with provided id does not exist in the school!'
		end
	if(@grade > 6)
		begin
		return 'Grade cannot be above 6.00!'
		end
	declare @count INT = (select count(*)
	from StudentsExams as s
	where s.StudentId = @studentId and s.Grade between @grade and @grade + 0.5)
	
	declare @studenName nvarchar(max) = (select top 1 s.FirstName from Students as s where Id = @studentId)
	return 'You have to update '+ cast(@count as nvarchar) + ' grades for the student ' + @studenName
end

select dbo.udf_ExamGradesToUpdate( 12,5.50) 

--19. Exclude from school
--Create a user defined stored procedure, named usp_ExcludeFromSchool(@StudentId), that receives a student id and attempts to delete the current student. A student will only be deleted if all of these conditions pass:
--•	If the student doesn’t exist, then it cannot be deleted. Raise an error with the message “This school has no student with the provided id!”
--If all the above conditions pass, delete the student and ALL OF HIS REFERENCES!
Go
Create procedure usp_ExcludeFromSchool(@StudentId INT)
AS 
	if	((SELECT s.Id from Students as s where s.Id = @studentId ) is null)
		begin
		RAISError ('This school has no student with the provided id!',16,1)
		return
		end
		delete from StudentsTeachers
		where StudentId = @StudentId
		delete from StudentsSubjects
		where StudentId = @StudentId
		delete from StudentsExams
		where StudentId = @StudentId
		delete from Students
		where id = @StudentId

EXEC usp_ExcludeFromSchool 1
select count (*) from Students

--20. Deleted Student
--Create a new table “ExcludedStudents” with columns (StudentId, StudentName). Create a trigger, which fires when student is excluded. After excluding the student, insert all of the data into the new table “ExcludedStudents”.

--Note: Submit only your CREATE TRIGGER statement!
create table ExcludedStudents
(
StudentId int
,StudentName nvarchar (max)
)
GO
create trigger trg_Delete
on Students
AFTER DELETE
AS
 insert into ExcludedStudents
 select d.Id, d.FirstName + ' '+ d.LastName from deleted as d

 DELETE FROM StudentsExams
WHERE StudentId = 1

DELETE FROM StudentsTeachers
WHERE StudentId = 1

DELETE FROM StudentsSubjects
WHERE StudentId = 1

DELETE FROM Students
WHERE Id = 1

SELECT * FROM ExcludedStudents
