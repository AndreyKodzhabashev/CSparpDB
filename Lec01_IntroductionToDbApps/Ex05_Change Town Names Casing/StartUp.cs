namespace Ex05_Change_Town_Names_Casing
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using AllExercisesOneSolution;

    public class StartUp
    {
        public static void Main()
        {
            Console.Write("Choose the Country name which Cities to go UPPER: ");
            string input = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(MySqlQuery.ConnStringToMinions))
            {
                connection.Open();
                int citiesAffected;
                int countryId = GetCountryId(connection, input);

                string cmdText = $@"update Towns set Name = UPPER(Name)	where CountryCode = @countryCode";
                using (SqlCommand cmd = new SqlCommand(cmdText, connection))
                {
                    cmd.Parameters.AddWithValue("@countryCode", countryId);

                    try
                    {
                        citiesAffected = cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        throw;
                    }
                }

                if (citiesAffected == 0)
                {
                    Console.WriteLine("No town names were affected.");
                }
                else
                {
                    string[] affectedCities = GetAffectedCities(connection, countryId);
                    PrintAffectedCities(affectedCities);
                }

                connection.Close();
            }
        }

        private static void PrintAffectedCities(string[] affectedCities)
        {
            Console.WriteLine($"{affectedCities.Length} town names were affected.");
            Console.WriteLine(string.Join(", ", affectedCities));
        }


        private static int GetCountryId(SqlConnection connection, string input)
        {
            int result;
            string cmdText = $"select Id from Countries where Name = @countryName";
            using (SqlCommand command = new SqlCommand(cmdText, connection))
            {
                command.Parameters.AddWithValue("@countryName", input);
                result = Convert.ToInt32(command.ExecuteScalar());
            }

            return result;
        }

        private static string[] GetAffectedCities(SqlConnection connection, int countryCode)
        {
            List<string> cities = new List<string>();
            string cmdTxt = $@"select Name from Towns where CountryCode = @countryCode";
            using (SqlCommand cmd = new SqlCommand(cmdTxt, connection))
            {
                cmd.Parameters.AddWithValue("@countryCode", countryCode);
                SqlDataReader reader = cmd.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        cities.Add((string)reader.GetValue(0));
                    }
                }
            }

            return cities.ToArray();
        }
    }
}