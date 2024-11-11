using Npgsql;

class LoginWithSQL : PostgreSQLDatabaseService
{
    public LoginWithSQL(string connectionString) : base(connectionString)
    {

    }

    public string GetPasswordFromSQLDataBase(string userId)
    {
        // PostgreSQL'e veri ekleme (INSERT)
        try
        {
            string query = "SELECT \"Password\" FROM users WHERE \"UserId\" = '" + userId + "'";
            var connection = new NpgsqlConnection(DataBaseConnectionStringsForLOGIN.connectionStringPostgreDocker);
            connection.Open();
            var command = new NpgsqlCommand(query, connection);

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                return reader[0].ToString();
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inserting data: {ex.Message}");
            return null;
        }
    }
}

