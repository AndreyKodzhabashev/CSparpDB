namespace Ex02_Villain_Names
{
    using System;
    using System.Data.SqlClient;
    using AllExercisesOneSolution;

    public class StartUp
    {
        public static void Main()
        {
            //in my opinion the result is wrong.
            // i print all results, sort desc.
            using (SqlConnection connection = new SqlConnection(MySqlQuery.ConnStringToMinions))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(MySqlQuery.SelectVillainsCount, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader[0]} - {reader[1]}");
                        }
                    }
                }
            }
        }
    }
}