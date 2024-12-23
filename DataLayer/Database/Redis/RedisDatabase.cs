using StackExchange.Redis;

public class RedisDataBaseService : DatabaseService
{
    private IDatabase database;
    private string ConnectionString { get; set; }

    public RedisDataBaseService(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public override bool ConnectionTest()
    {
        try
        {
            var connection = ConnectionMultiplexer.Connect(ConnectionString);
            database = connection.GetDatabase();
            var pingResult = database.Ping();
            Console.WriteLine("Redis connection success.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Redis connection failed: {ex.Message}");
            return false;
        }
    }

    public override void ConnectionDataBase()
    {
        var connection = ConnectionMultiplexer.Connect(ConnectionString);
        database = connection.GetDatabase();
        Console.WriteLine("Connected to Redis.");
    }

    public override string GetValue(string query)
    {
        try
        {
            var value = database.StringGet(query);
            if (value.HasValue)
            {
                return value.ToString();
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            return $"Error reading data from Redis: {ex.Message}";
        }
    }

    public override void SetValue(string data)
    {
        try
        {
            var key = "sampleKey";
            database.StringSet(key, data);
            Console.WriteLine("Data inserted into Redis.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inserting data into Redis: {ex.Message}");
        }
    }

    public void SetValue(string key, string value)
    {
        try
        {
            database.StringSet(key, value);
            Console.WriteLine($"Data inserted into Redis: Key = {key}, Value = {value}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inserting data into Redis: {ex.Message}");
        }
    }

    public override void UpdateValue(string data)
    {
        try
        {
            var key = "sampleKey";
            database.StringSet(key, data);
            Console.WriteLine("Data updated in Redis.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating data in Redis: {ex.Message}");
        }
    }
    public void UpdateValue(string key, string newValue)
    {
        try
        {
            database.StringSet(key, newValue);
            Console.WriteLine($"Data updated in Redis: Key = {key}, New Value = {newValue}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating data in Redis: {ex.Message}");
        }
    }

    public override void DeleteValue(string key)
    {
        try
        {
            database.KeyDelete(key);
            Console.WriteLine($"Data deleted from Redis: Key = {key}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting data from Redis: {ex.Message}");
        }
    }
}
