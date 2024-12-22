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
                connection.Open(); // Bağlantıyı açmaya çalış
                Console.WriteLine("PostgreSQL connection success.");
                return true; // Bağlantı başarılıysa true döner
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"PostgreSQL connection failed: {ex.Message}");
            return false; // Bağlantı hatalıysa false döner
        }
    }

    public override void ConnectionDataBase()
    {
        // Veritabanına bağlantıyı burada açabilirsiniz
        connection = new NpgsqlConnection(ConnectionString);
        connection.Open();
    }
    
    public override string GetValue(string query)
    {
        // PostgreSQL'den veri okuma
        try
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        // Verilerin her satırını yazdırıyoruz
                        while (reader.Read())
                        {
                            // Satırdaki her sütunu yazdırıyoruz
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write(reader[i] + "\t");  // Her sütun değerini yazdır
                            }
                            Console.WriteLine(); // Satırın sonunda yeni bir satır
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

        return "Veriler başarıyla yazdırıldı"; // Veri okuma başarılı
    }
    


    public override void SetValue(string data)
    {
        throw new NotImplementedException();
    }

    public override void UpdateValue(string data)
    {
        throw new NotImplementedException ();
    }

    public override void DeleteValue(string data)
    {
        throw new NotImplementedException();
    }

    // İstediğiniz takdirde diğer overload'ları ekleyebilirsiniz.
}
