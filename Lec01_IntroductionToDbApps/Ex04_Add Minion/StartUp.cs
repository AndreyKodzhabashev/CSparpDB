namespace Ex04_Add_Minion
{
    using System;
    using System.Data.SqlClient;
    using System.Linq;
    using AllExercisesOneSolution;

    public class StartUp
    {
        public static void Main()
        {
            int townId = 0;
            int villainId = 0;
            int minionId = 0;
            using (SqlConnection connection = new SqlConnection(MySqlQuery.ConnStringToMinions))
            {
                connection.Open();
                var inputMinion = ReadAndSplitInput();
                var minionData = ReadMinion(inputMinion);

                var inputVillain = ReadAndSplitInput();
                string villainName = ReadVillain(inputVillain);

                string cmdTextGetTownName = $"select Name from Towns where Name = @args";
                string cmdTextGetTownId = $"select Id from Towns where Name = @args";
                if (ItemNotExists(connection, cmdTextGetTownName, minionData[2]))
                {
                    string cmdTextTown = $"insert into Towns(Name) values(@args)";
                    InsertItem(connection, cmdTextTown, minionData[2]);
                    Console.WriteLine($"Town {minionData[2]} was added to the database.");
                }

                townId = GetId(connection, cmdTextGetTownId, minionData[2]);

                string commandVillain = $"select Name from Villains where Name = @args";
                string cmdVillain = $"select Id from Villains where Name = @args";
                if (ItemNotExists(connection, commandVillain, villainName))
                {
                    string cmdTextInsertVillain = $"insert into Villains(Name,EvilnessFactorId) values (@args,4)";
                    InsertItem(connection, cmdTextInsertVillain, villainName);
                    Console.WriteLine($"Villain {villainName} was added to the database.");
                }
                else
                    villainId = GetId(connection, cmdVillain, villainName);

                InsertMinion(connection, minionData, townId);

                string cmdTextGetLastMinionId = "select top 1 Id from Minions where Name = @args order by id desc";
                minionId = GetId(connection, cmdTextGetLastMinionId, minionData[0]);

                InsertMinionVillain(connection, villainId, minionId);

                Console.WriteLine($"Successfully added {minionData[0]} to be minion of {villainName}.");
            }
        }

        static string[] ReadAndSplitInput()
        {
            string[] inputSplit = Console.ReadLine()
                .Split(new char[] {':', ' ',}, StringSplitOptions.RemoveEmptyEntries).ToArray();
            return inputSplit;
        }

        static dynamic[] ReadMinion(string[] inputMinion)
        {
            string name = inputMinion[1];
            int age = int.Parse(inputMinion[2]);
            string town = inputMinion[3];
            return new dynamic[] {name, age, town};
        }

        static string ReadVillain(string[] inputVillain)
        {
            return inputVillain[1];
        }

        private static int GetId(SqlConnection connection, string cmdText, string args)
        {
            int result = 0;
            using (SqlCommand cmd = new SqlCommand(cmdText, connection))
            {
                cmd.Parameters.AddWithValue("@args", args);
                result = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return result;
        }

        private static bool ItemNotExists(SqlConnection connection, string cmdText, string args)
        {
            string result;
            using (SqlCommand cmd = new SqlCommand(cmdText, connection))
            {
                cmd.Parameters.AddWithValue("@args", args);
                try
                {
                    result = (string)cmd.ExecuteScalar();
                }
                catch (Exception e)
                {
                    return true;
                }
            }

            return false;
        }

        private static void InsertItem(SqlConnection connection, string cmdText, string args)
        {
            using (SqlCommand cmd = new SqlCommand(cmdText, connection))
            {
                cmd.Parameters.AddWithValue("@args", args);
                cmd.ExecuteNonQuery();
            }
        }

        private static void InsertMinion(SqlConnection connection, dynamic[] minionData, int townId)
        {
            string insertIntoMinions =
                $"INSERT INTO Minions (Name, Age, TownId) VALUES ('{minionData[0]}', {minionData[1]}, {townId})";
            int result;
            using (SqlCommand command = new SqlCommand(insertIntoMinions, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private static void InsertMinionVillain(SqlConnection connection, int villainId, int minionId)
        {
            string insertIntoMinionsVillains =
                $"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId,@villainId)";
            using (SqlCommand cmd = new SqlCommand(insertIntoMinionsVillains, connection))
            {
                cmd.Parameters.AddWithValue("@villainId", villainId);
                cmd.Parameters.AddWithValue("@minionId", minionId);

                cmd.ExecuteNonQuery();
            }
        }
    }
}