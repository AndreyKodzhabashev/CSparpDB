namespace Ex03_Minion_Names
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using AllExercisesOneSolution;

    public class StartUp
    {
        private static string num;

        static SqlConnection connection = new SqlConnection(MySqlQuery.ConnStringToMinions);

        public static void Main()
        {
            using (connection)
            {
                Console.WriteLine("Choose the ID for Villain to see his available minions. ");

                connection.Open();

                using (SqlCommand command = new SqlCommand(MySqlQuery.SelectAllVillains, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("Available Villains are: ");
                        while (reader.Read())
                        {
                            Console.WriteLine($" ID -> {reader[0]}; Name - {reader[1]}");
                        }
                    }
                }

                Console.Write("Input your choice:");
                num = Console.ReadLine();

                if (num.ToLower().Equals("end"))
                {
                    return;
                }

                if (int.TryParse(num, out int test) == false)
                {
                    Console.WriteLine("RESTART program AND Input a valid digit for ID: ");
                }

                SqlCommand command2 = new SqlCommand($@"select Name from Villains where Id = {num}", connection);
                var result = command2.ExecuteScalar();

                if (result == null)
                {
                    Console.WriteLine($"No villain with ID {num} exists in the database.");
                    return;
                }


                using (SqlCommand command = new SqlCommand(
                    $"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,m.Name,m.Age " +
                    "FROM MinionsVillains AS mv " +
                    "JOIN Minions As m ON mv.MinionId = m.Id " +
                    $"WHERE mv.VillainId = {num} " +
                    "ORDER BY m.Name", connection))


                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine($"Villain: {result}");
                    List<Min> testw = new List<Min>();
                    while (reader.Read())
                    {
                        testw.Add(new Min
                            {Name = (string)reader[1], Age = (int) reader[2]});
                    }

                    if (testw.Count == 0)
                    {
                        Console.WriteLine("(no minions)");
                    }
                    else
                    {
                        foreach (var minion in testw)
                        {
                            Console.WriteLine($"{minion.Num}. {minion.Name} {minion.Age}");
                        }
                    }
                }
            }
        }
    }
}