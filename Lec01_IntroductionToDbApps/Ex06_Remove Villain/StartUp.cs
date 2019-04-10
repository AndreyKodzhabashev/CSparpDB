namespace Ex06_Remove_Villain
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using AllExercisesOneSolution;

    public class StartUp
    {
        public static void Main()
        {
            using (SqlConnection connection = new SqlConnection(MySqlQuery.ConnStringToMinions))
            {
                Console.Write("Insert digit as ID for the Villain to be deleted: ");
                Console.WriteLine("Available Villains are: ");
                connection.Open();
                var availableVillains = GetAvailableVillains(connection);
                foreach (var availableVillain in availableVillains)
                {
                    Console.WriteLine($"ID-> {availableVillain.Key} and Name: {availableVillain.Value}");
                }

                Console.Write("Choose an ID: ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int villainId) == false)
                {
                    Console.Write("Restart the program and Chose a digit!");
                    return;
                }

                if (IdIsNotAvailable(villainId, availableVillains))
                {
                    Console.WriteLine("No such villain was found.");
                    Console.WriteLine("Restart the program and chose a valid ID!");
                    Console.WriteLine("I gave you a list. How is even possible to make a mistake? :) ");
                    return;
                }

                //List<string> freedMinions = GetFreedMinions(connection, villainId);
                int countFreedMinions = DeleteReferenceVillainMinion(connection, villainId);
                string deletedVillain = DeleteVillain(connection, villainId);

                Console.WriteLine($"{deletedVillain} was deleted.");
                Console.WriteLine($"{countFreedMinions} minions were released.");
            }
        }

        private static string DeleteVillain(SqlConnection connection, int villainId)
        {
            string villainName = string.Empty;
            string getVillainName = "select Name from Villains where Id = @villainId";


            using (SqlCommand cmd = new SqlCommand(getVillainName, connection))
            {
                cmd.Parameters.AddWithValue("@villainId", villainId);
                villainName = (string)cmd.ExecuteScalar();
                cmd.Dispose();
            }

            string cmdText = $@"delete from MinionsVillains	where VillainId = @villainId";
            using (SqlCommand cmd = new SqlCommand(cmdText, connection))
            {
                cmd.Parameters.AddWithValue("@villainId", villainId);
                cmd.ExecuteNonQuery();
            }

            return villainName;
        }

        private static bool IdIsNotAvailable(int villainId, List<KeyValuePair<int, string>> availableVillains)
        {
            foreach (var availableVillain in availableVillains)
            {
                if (availableVillain.Key == villainId)
                {
                    return false;
                }
            }

            return true;
        }

        private static List<KeyValuePair<int, string>> GetAvailableVillains(SqlConnection connection)
        {
            List<KeyValuePair<int, string>> result = new List<KeyValuePair<int, string>>();
            string cmdText = $"select Id,Name from Villains";

            using (SqlCommand cmd = new SqlCommand(cmdText, connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new KeyValuePair<int, string>((int) reader[0], (string)reader[1]));
                    }
                }
            }

            return result;
        }

        private static List<string> GetFreedMinions(SqlConnection connection, int villainId)
        {
            List<string> result = new List<string>();
            string cmdText =
                "Select m.Name from Minions as m join MinionsVillains as mv on m.Id = mv.MinionsId and mv.VillainId = @villainId";
            using (SqlCommand cmd = new SqlCommand(cmdText, connection))
            {
                cmd.Parameters.AddWithValue("@villainId", villainId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add((string)reader[0]);
                    }
                }
            }

            return result;
        }

        private static int DeleteReferenceVillainMinion(SqlConnection connection, int villainId)
        {
            string cmdText = "delete from MinionsVillains where VillainId = @villainId";
            int affectedRows = 0;
            using (SqlCommand cmd = new SqlCommand(cmdText, connection))
            {
                cmd.Parameters.AddWithValue("@villainId", villainId);
                affectedRows = cmd.ExecuteNonQuery();
            }

            return affectedRows;
        }
    }
}