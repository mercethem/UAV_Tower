using MongoDB.Bson; // MongoDB.Bson kütüphanesini kullanmak için import eder // Imports MongoDB.Bson library
using MongoDB.Driver; // MongoDB'ye bağlanmak için MongoDB.Driver kütüphanesini import eder // Imports MongoDB.Driver library
using System.Text.Json.Serialization; // JSON serileştirme işlemleri için gerekli kütüphaneyi import eder // Imports library for JSON serialization
using System.Text.Json; // JSON işlemleri için kütüphaneyi import eder // Imports library for handling JSON

namespace radarApi.FlightDataSaver // Namespace tanımlaması // Defines the namespace for the project
{
    public class FlightDataSaverMongoDb // MongoDB'ye uçuş verilerini kaydeden sınıf // Class for saving flight data to MongoDB
    {
        private readonly IMongoCollection<BsonDocument> _flightDataCollection; // MongoDB koleksiyonunu tutan değişken // Variable holding the MongoDB collection

        public FlightDataSaverMongoDb(string connectionString, string databaseName, string collectionName) // Yapıcı metod, MongoDB'ye bağlanmak için gerekli parametreleri alır // Constructor method to connect to MongoDB
        {
            // MongoDB'ye bağlan // Connect to MongoDB
            var client = new MongoClient(connectionString); // MongoClient nesnesi, connectionString ile MongoDB'ye bağlanır // MongoClient instance connects to MongoDB using the connection string
            var database = client.GetDatabase(databaseName); // MongoDB veritabanını seçer // Selects the database in MongoDB
            _flightDataCollection = database.GetCollection<BsonDocument>(collectionName); // Veritabanındaki koleksiyonu alır // Gets the collection from the database
        }

        public async Task SaveFlightDataAsync(List<FlightData> flightDataList) // Uçuş verilerini MongoDB'ye kaydetmek için asenkron metod // Asynchronous method to save flight data to MongoDB
        {
            foreach (var flightData in flightDataList) // Uçuş verileri listesindeki her bir uçuş için işlem yapılır // Loops through each flight data in the list
            {
                // Eğer Seen değeri 60 veya daha büyükse, bu uçuşu kaydetme // If Seen value is 60 or greater, skip saving this flight
                if (flightData.Seen >= 60) // Seen değeri 60 veya daha büyükse atla // If Seen value is greater than or equal to 60, skip this flight
                {
                    continue;  // Bu uçuşu atla // Skip this flight
                }

                // Uçuş bilgilerini JSON formatına çevir // Convert flight data to JSON format
                var flightInfo = new
                {
                    Flight = flightData.Flight.Trim(), // Flight kodunu alır, boşlukları temizler // Gets the flight code and trims any whitespace
                    Hex_ICAO = flightData.Hex_ICAO, // Hex_ICAO kodunu alır // Gets the Hex_ICAO code
                    Squawk = flightData.Squawk, // Squawk kodunu alır // Gets the Squawk code
                    Latitude = flightData.Lat, // Enlem bilgisini alır // Gets the latitude
                    Longitude = flightData.Lon, // Boylam bilgisini alır // Gets the longitude
                    Altitude = flightData.Altitude, // Yükseklik bilgisini alır // Gets the altitude
                    Speed = flightData.Speed, // Hız bilgisini alır // Gets the speed
                    VertRate = flightData.VertRate, // Yükseklik değişim oranını alır // Gets the vertical rate
                    Track = flightData.Track, // İzleme bilgisini alır // Gets the track
                    ValidPosition = flightData.ValidPosition, // Geçerli pozisyon bilgisini alır // Gets the valid position information
                    ValidTrack = flightData.ValidTrack, // Geçerli izleme bilgisini alır // Gets the valid track information
                    Messages = flightData.Messages, // Mesaj bilgilerini alır // Gets the messages
                    Seen = flightData.Seen, // Uçuş verisinin görüldüğü süreyi alır // Gets the time the flight data has been seen
                    LastUpdated = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") // UTC Zamanı formatında son güncellenme zamanını alır // Gets the last updated time in UTC format
                };

                // JSON serileştirme // JSON serialization
                var flightJson = System.Text.Json.JsonSerializer.Serialize(flightInfo, new JsonSerializerOptions // flightInfo nesnesini JSON formatına çevirir // Serializes the flightInfo object to JSON format
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,  // Null değerlerini atla // Ignores null values
                    WriteIndented = true  // JSON'u okunabilir hale getir // Makes the JSON readable (pretty print)
                });

                // JSON verisini BSON formatına dönüştür // Converts the JSON data to BSON format
                var bsonDocument = BsonDocument.Parse(flightJson); // JSON'u BSON formatına dönüştürür // Converts the JSON string to BSON format

                try
                {
                    // MongoDB'ye kaydet // Save to MongoDB
                    await _flightDataCollection.InsertOneAsync(bsonDocument); // BSON verisini MongoDB'ye asenkron olarak ekler // Asynchronously inserts the BSON document into MongoDB
                    Console.WriteLine($"Flight data for {flightData.Hex_ICAO} saved to MongoDB successfully."); // Kaydedilen uçuş verisinin bilgisi yazdırılır // Prints a success message for the saved flight data
                }
                catch (Exception ex) // Hata oluşursa // If an error occurs
                {
                    Console.WriteLine($"Error saving flight data to MongoDB: {ex.Message}"); // Hata mesajını yazdırır // Prints the error message
                }
            }
        }
    }
}
