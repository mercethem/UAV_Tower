using MongoDB.Driver.Core.Configuration;
using Npgsql;

public class PostgreSQLDatabaseService : DatabaseService
{
    private NpgsqlConnection connection;
    private string ConnectionString { get; set; }

    public PostgreSQLDatabaseService(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public override bool ConnectionTest()
    {
        try
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                Console.WriteLine("PostgreSQL connection success.");
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"PostgreSQL connection failed: {ex.Message}");
            return false;
        }
    }

    public override void ConnectionDataBase()
    {

        connection = new NpgsqlConnection(ConnectionString);
        connection.Open();
    }

    public override string GetValue(string query)
    {

        try
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write(reader[i] + "\t");
                            }
                            Console.WriteLine();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading data: {ex.Message}");
            return null;
        }

        return "Veriler başarıyla yazdırıldı";
    }


    public override void SetValue(string data)
    {
        throw new NotImplementedException();
    }

    public override void UpdateValue(string data)
    {
        throw new NotImplementedException();
    }

    public override void DeleteValue(string data)
    {
        throw new NotImplementedException();
    }
}
