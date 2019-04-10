namespace Ex08_IncreaseMinionAge
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using AllExercisesOneSolution;

    public class StartUp
    {
        public static void Main()
        {
            int[] idCollection = Console.ReadLine()
                .Split()
                .Select(int.Parse)
                .ToArray();

            string cmdText =
                "Select * from Minions as m where id = @Id";
            using (SqlConnection connection = new SqlConnection(MySqlQuery.ConnStringToMinions))
            {
                connection.Open();
                var allMinions = GetMinionNameAndAge(connection, cmdText, idCollection);

                foreach (var minion in allMinions)
                {
                    minion[2] = minion[2] += 1;
                    string name = minion[1];
                    char firstLetter = name[0];
                    char newLetter = Char.ToLower(firstLetter);
                    var nameAsArray = name.ToCharArray();
                    nameAsArray[0] = newLetter;
                    minion[1] = string.Join("", nameAsArray);
                }


                //updatePart
                string cmdUpdate = "update Minions set Name = @Name,Age = @Age where Id = @Id";
                UpdateMinions(connection, cmdUpdate, allMinions);

                var allMinionsAfterUpdate = GetMinionNameAndAge(connection, cmdText, idCollection);
                foreach (var minion in allMinionsAfterUpdate)
                {
                    Console.WriteLine($"{minion[1]} {minion[2]}");
                }
            }
        }

        private static List<dynamic[]> GetMinionNameAndAge(SqlConnection connection, string cmdText,
            int[] IdCollection)
        {
            List<dynamic[]> result = new List<dynamic[]>();

            foreach (var id in IdCollection)
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new dynamic[]
                            {
                                (int) reader[0], (string) reader[1],
                                (int) reader[2]
                            });
                        }
                    }
                }
            }

            return result;
        }

        private static void UpdateMinions(SqlConnection connection, string cmdText, List<dynamic[]> dataList)
        {
            foreach (var item in dataList)
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", item[0]);

                    cmd.Parameters.AddWithValue("@Name", item[1]);

                    cmd.Parameters.AddWithValue("@Age", item[2]);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}