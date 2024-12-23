using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace radarApi.FlightDataSaver
{
    public class FlightDataSaverMongoDb // Class for saving flight data to MongoDB
    {
        private readonly IMongoCollection<BsonDocument> _flightDataCollection; // Variable holding the MongoDB collection

        public FlightDataSaverMongoDb(string connectionString, string databaseName, string collectionName)// Constructor method to connect to MongoDB
        {
            // MongoDB'ye bağlan // Connect to MongoDB
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _flightDataCollection = database.GetCollection<BsonDocument>(collectionName);
        }

        public async Task SaveFlightDataAsync(List<FlightData> flightDataList) // Asynchronous method to save flight data to MongoDB
        {
            foreach (var flightData in flightDataList)
            {

                if (flightData.Seen >= 60)
                {
                    continue;
                }

                var flightInfo = new
                {
                    Flight = flightData.Flight.Trim(),
                    Hex_ICAO = flightData.Hex_ICAO,
                    Squawk = flightData.Squawk,
                    Latitude = flightData.Lat,
                    Longitude = flightData.Lon,
                    Altitude = flightData.Altitude,
                    Speed = flightData.Speed,
                    VertRate = flightData.VertRate,
                    Track = flightData.Track,
                    ValidPosition = flightData.ValidPosition,
                    ValidTrack = flightData.ValidTrack,
                    Messages = flightData.Messages,
                    Seen = flightData.Seen,
                    LastUpdated = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                };


                var flightJson = System.Text.Json.JsonSerializer.Serialize(flightInfo, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    WriteIndented = true
                });

                var bsonDocument = BsonDocument.Parse(flightJson);

                try
                {
                    await _flightDataCollection.InsertOneAsync(bsonDocument);
                    Console.WriteLine($"Flight data for {flightData.Hex_ICAO} saved to MongoDB successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving flight data to MongoDB: {ex.Message}");
                }
            }
        }
    }
}
