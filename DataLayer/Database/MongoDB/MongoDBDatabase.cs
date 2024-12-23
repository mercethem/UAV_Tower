using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

public class MongoDatabaseService
{
    private IMongoDatabase database;
    private string ConnectionString { get; set; }


    public MongoDatabaseService(string connectionString, string dbName = "testdb")
    {
        ConnectionString = connectionString;
        var client = new MongoClient(connectionString);
        database = client.GetDatabase(dbName);
    }


    public bool ConnectionTest()
    {
        try
        {
            var command = new BsonDocument("ping", 1);
            database.RunCommand<BsonDocument>(command);
            Console.WriteLine("MongoDB connection success.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"MongoDB connection failed: {ex.Message}");
            return false;
        }
    }


    public List<T> GetValue<T>(string collectionName, FilterDefinition<T> filter = null)
    {
        try
        {
            var collection = database.GetCollection<T>(collectionName);
            filter = filter ?? Builders<T>.Filter.Empty;

            return collection.Find(filter).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading data from MongoDB: {ex.Message}");
            return null;
        }
    }


    public void SetValue<T>(string collectionName, T data)
    {
        try
        {
            var collection = database.GetCollection<T>(collectionName);
            collection.InsertOne(data);
            Console.WriteLine("Data inserted into MongoDB.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inserting data into MongoDB: {ex.Message}");
        }
    }


    public void UpdateValue<T>(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update)
    {
        try
        {
            var collection = database.GetCollection<T>(collectionName);
            var result = collection.UpdateOne(filter, update);
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


    public void DeleteValue<T>(string collectionName, FilterDefinition<T> filter)
    {
        try
        {
            var collection = database.GetCollection<T>(collectionName);
            var result = collection.DeleteOne(filter);
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
