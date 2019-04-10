namespace AllExercisesOneSolution
{
    public static class MySqlQuery
    {
        public const string ConnStringToMaster =
            @"Server=localhost\sqlexpress;Database=master;Trusted_Connection=True;";

        public const string ConnStringToMinions =
            @"Server=localhost\sqlexpress;Database=MinionsDB;Trusted_Connection=True;";

        public const string CreateDbMinions = "CREATE DATABASE MinionsDB";

        public const string UseDbMinions = "USE MinionsDB";

        public const string CreateCountries = "CREATE TABLE Countries (Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50))";

        public const string CreateTowns =
            "CREATE TABLE Towns(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50), CountryCode INT FOREIGN KEY REFERENCES Countries(Id))";

        public const string CreateMinions =
            "CREATE TABLE Minions(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(30), Age INT, TownId INT FOREIGN KEY REFERENCES Towns(Id))";

        public const string CreateEvilnessFactors =
            "CREATE TABLE EvilnessFactors(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50))";

        public const string CreateVillains =
            "CREATE TABLE Villains(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50), EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id))";

        public const string CreateMinionsVillains =
            "CREATE TABLE MinionsVillains(MinionId INT FOREIGN KEY REFERENCES Minions(Id), VillainId INT FOREIGN KEY REFERENCES Villains(Id), CONSTRAINT PK_MinionsVillains PRIMARY KEY (MinionId, VillainId))";

        public const string InsertCountries =
            "INSERT INTO Countries([Name]) VALUES('Bulgaria'),('England'),('Cyprus'),('Germany'),('Norway')";

        public const string InsertTowns =
            "INSERT INTO Towns([Name], CountryCode) VALUES('Plovdiv', 1),('Varna', 1),('Burgas', 1),('Sofia', 1),('London', 2),('Southampton', 2),('Bath', 2),('Liverpool', 2),('Berlin', 3),('Frankfurt', 3),('Oslo', 4)";

        public const string InsertMinions =
            "INSERT INTO Minions(Name, Age, TownId) VALUES('Bob', 42, 3),('Kevin', 1, 1),('Bob ', 32, 6),('Simon', 45, 3),('Cathleen', 11, 2),('Carry ', 50, 10),('Becky', 125, 5),('Mars', 21, 1),('Misho', 5, 10),('Zoe', 125, 5),('Json', 21, 1)";

        public const string InsertEvilnessFactors =
            "INSERT INTO EvilnessFactors(Name) VALUES('Super good'),('Good'),('Bad'), ('Evil'),('Super evil')";

        public const string InsertVillains =
            "INSERT INTO Villains(Name, EvilnessFactorId) VALUES('Gru',2),('Victor',1),('Jilly',3),('Miro',4),('Rosen',5),('Dimityr',1),('Dobromir',2)";


        public const string InsertMinionsVillains =
            "INSERT INTO MinionsVillains(MinionId, VillainId) VALUES(4,2),(1,1),(5,7),(3,5),(2,6),(11,5),(8,4),(9,7),(7,1),(1,3),(7,3),(5,3),(4,3),(1,2),(2,1),(2,7)";

        public const string SelectVillainsCount =
            "select v.Name, count(*)from Villains as v join MinionsVillains as mv on mv.VillainId = v.Id Group by v.Name order by count(*) desc";

        public const string SelectAllVillains = "Select * from Villains";
    }
}