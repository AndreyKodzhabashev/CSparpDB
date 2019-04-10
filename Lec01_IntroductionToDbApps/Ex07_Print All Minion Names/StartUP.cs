namespace Ex07_Print_All_Minion_Names
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
                string cmdText =
                    "Select m.Name from Minions as m";
                connection.Open();
                List<string> allMinions = GetMinions(connection, cmdText);
                int loopMargin = allMinions.Count;

                for (int i = 0; i < loopMargin / 2; i++)
                {
                    Console.WriteLine(allMinions[i]);
                    Console.WriteLine(allMinions[allMinions.Count - 1 - i]);
                }

                if (loopMargin % 2 != 0)
                {
                    Console.WriteLine(allMinions[loopMargin / 2]);
                }
            }
        }

        private static List<string> GetMinions(SqlConnection connection, string cmdText)
        {
            List<string> result = new List<string>();

            using (SqlCommand cmd = new SqlCommand(cmdText, connection))
            {
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
    }
}