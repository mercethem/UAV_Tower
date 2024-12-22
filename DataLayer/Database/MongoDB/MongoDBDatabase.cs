using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

public class MongoDatabaseService
{
    private IMongoDatabase database;
    private string ConnectionString { get; set; }

    // MongoDB bağlantı kurulumu
    public MongoDatabaseService(string connectionString, string dbName = "testdb")
    {
        ConnectionString = connectionString;
        var client = new MongoClient(connectionString);
        database = client.GetDatabase(dbName);
    }

    // Veritabanına bağlantı testi
    public bool ConnectionTest()
    {
        try
        {
            var command = new BsonDocument("ping", 1);
            database.RunCommand<BsonDocument>(command);  // MongoDB'ye Ping gönderiyoruz
            Console.WriteLine("MongoDB connection success.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"MongoDB connection failed: {ex.Message}");
            return false;
        }
    }

    // Veritabanından herhangi bir koleksiyondan veri almak
    public List<T> GetValue<T>(string collectionName, FilterDefinition<T> filter = null)
    {
        try
        {
            var collection = database.GetCollection<T>(collectionName);
            filter = filter ?? Builders<T>.Filter.Empty;  // Eğer filtre yoksa tüm veriyi getir

            return collection.Find(filter).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading data from MongoDB: {ex.Message}");
            return null;
        }
    }

    // Veritabanına yeni veri eklemek
    public void SetValue<T>(string collectionName, T data)
    {
        try
        {
            var collection = database.GetCollection<T>(collectionName);
            collection.InsertOne(data);  // Veriyi koleksiyona ekliyoruz
            Console.WriteLine("Data inserted into MongoDB.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inserting data into MongoDB: {ex.Message}");
        }
    }

    // Veritabanındaki mevcut veriyi güncellemek
    public void UpdateValue<T>(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update)
    {
        try
        {
            var collection = database.GetCollection<T>(collectionName);
            var result = collection.UpdateOne(filter, update);  // Filtreye göre güncelleme yapıyoruz
            if (result.MatchedCount > 0)
                Console.WriteLine("Data updated in MongoDB.");
            else
                Console.WriteLine("No matching data found to update.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating data in MongoDB: {ex.Message}");
        }
    }

    // Veritabanından veri silmek
    public void DeleteValue<T>(string collectionName, FilterDefinition<T> filter)
    {
        try
        {
            var collection = database.GetCollection<T>(collectionName);
            var result = collection.DeleteOne(filter);  // Filtreye göre veri silme
            if (result.DeletedCount > 0)
                Console.WriteLine("Data deleted from MongoDB.");
            else
                Console.WriteLine("No matching data found to delete.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting data from MongoDB: {ex.Message}");
        }
    }
}
