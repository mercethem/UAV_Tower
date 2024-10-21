using System;
using MongoDB.Driver;

public class DatabaseService
{
    // Singleton instance of the DatabaseService class.
    // DatabaseService sınıfının tekil örneği.
    private static DatabaseService _instance;

    // Lock object for thread safety.
    // Çoklu iş parçacığı güvenliği için kilit nesnesi.
    private static readonly object _lock = new object();

    // Reference to the MongoDB database.
    // MongoDB veritabanına referans.
    private readonly IMongoDatabase _database;

    // Private constructor to prevent instantiation from outside the class.
    // Sınıf dışından örnek oluşturulmasını engellemek için özel yapıcı.
    private DatabaseService(string connectionString)
    {
        // Create a MongoDB client using the provided connection string.
        // Sağlanan bağlantı dizesini kullanarak bir MongoDB istemcisi oluştur.
        var client = new MongoClient(connectionString);

        // Get the database with the specified name ("mongodbverileri").
        // Belirtilen isimdeki veritabanını al ("mongodbverileri").
        _database = client.GetDatabase("mongodbverileri"); // MongoDB database name // MongoDB veritabanı adı
    }

    // Public method to get the singleton instance of DatabaseService.
    // DatabaseService'in tekil örneğini almak için genel yöntem.
    public static DatabaseService GetInstance(string connectionString)
    {
        // Check if the instance is null.
        // Örnek null mı kontrol et.
        if (_instance == null)
        {
            // Lock to ensure thread safety.
            // Çoklu iş parçacığı güvenliğini sağlamak için kilitle.
            lock (_lock)
            {
                // Double-check if the instance is still null before creating it.
                // Oluşturmadan önce örneğin hala null olup olmadığını kontrol et.
                if (_instance == null)
                {
                    // Create a new instance of DatabaseService.
                    // DatabaseService'in yeni bir örneğini oluştur.
                    _instance = new DatabaseService(connectionString);
                }
            }
        }
        // Return the singleton instance.
        // Tekil örneği döndür.
        return _instance;
    }

    // Method to save weather information to the MongoDB collection.
    // Hava durumu bilgisini MongoDB koleksiyonuna kaydetme yöntemi.
    public void SaveWeatherInfo(WeatherInfo weatherInfo)
    {
        // Get the collection with the specified name ("mongocollect").
        // Belirtilen isimdeki koleksiyonu al ("mongocollect").
        var collection = _database.GetCollection<WeatherInfo>("mongocollect"); // Collection name // Koleksiyon adı

        // Insert the weather information into the collection.
        // Hava durumu bilgisini koleksiyona ekle.
        collection.InsertOne(weatherInfo);
    }
}
