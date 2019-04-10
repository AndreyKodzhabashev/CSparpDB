namespace AllExercisesOneSolution
{
    using System.Collections.Generic;
    using System.Data.SqlClient;

    class StartUp
    {
        static void Main()
        {
            using (SqlConnection connectionToMaster = new SqlConnection(MySqlQuery.ConnStringToMaster))
            {
                SqlCommand createDB = new SqlCommand(MySqlQuery.CreateDbMinions, connectionToMaster);
                SqlCommand useDB = new SqlCommand(MySqlQuery.UseDbMinions, connectionToMaster);

                connectionToMaster.Open();

                createDB.ExecuteNonQuery();
                useDB.ExecuteNonQuery();

                connectionToMaster.Close();
            }

            using (SqlConnection connectionToMinions = new SqlConnection(MySqlQuery.ConnStringToMinions))
            {
                List<SqlCommand> commands = new List<SqlCommand>
                {
                    new SqlCommand(MySqlQuery.CreateEvilnessFactors, connectionToMinions),
                    new SqlCommand(MySqlQuery.CreateCountries, connectionToMinions),
                    new SqlCommand(MySqlQuery.CreateTowns, connectionToMinions),
                    new SqlCommand(MySqlQuery.CreateMinions, connectionToMinions),
                    new SqlCommand(MySqlQuery.CreateVillains, connectionToMinions),
                    new SqlCommand(MySqlQuery.CreateMinionsVillains, connectionToMinions)
                };
                connectionToMinions.Open();

                foreach (SqlCommand command in commands)
                {
                    command.ExecuteNonQuery();
                }

                List<string> dBinserts = new List<string>
                {
                    MySqlQuery.InsertCountries,
                    MySqlQuery.InsertEvilnessFactors,
                    MySqlQuery.InsertTowns,
                    MySqlQuery.InsertMinions,
                    MySqlQuery.InsertVillains,
                    MySqlQuery.InsertMinionsVillains
                };

                foreach (string dBinsert in dBinserts)
                {
                    SqlCommand currentCommand = new SqlCommand(dBinsert, connectionToMinions);
                    currentCommand.ExecuteNonQuery();
                }

                connectionToMinions.Close();
            }
        }
    }
}