using Npgsql; // Npgsql kütüphanesini kullanmak için import eder // Imports the Npgsql library for PostgreSQL database operations
using NpgsqlTypes; // Npgsql türlerini kullanmak için import eder // Imports the types used by Npgsql
using System.Text.Json.Serialization; // JSON serileştirme işlemleri için gerekli kütüphaneyi import eder // Imports the library for JSON serialization
using System.Text.Json; // JSON işlemleri için kütüphaneyi import eder // Imports the library for handling JSON

namespace radarApi.FlightDataSaver // Namespace tanımlaması // Defines the namespace for the project
{
    public class FlightDataSaverPostgreSql // PostgreSQL veritabanına uçuş verilerini kaydeden sınıf // Class for saving flight data to PostgreSQL database
    {
        private readonly string connectionString; // Veritabanı bağlantı dizesini tutar // Holds the database connection string

        public FlightDataSaverPostgreSql(string connectionString) // Yapıcı metod, bağlantı dizesini alır // Constructor method that takes the connection string
        {
            this.connectionString = connectionString; // Veritabanı bağlantı dizesini kaydeder // Saves the connection string
        }

        public async Task SaveFlightDataAsync(List<FlightData> flightDataList) // Uçuş verilerini PostgreSQL veritabanına kaydetmek için asenkron metod // Asynchronous method to save flight data to PostgreSQL database
        {
            using (var connection = new NpgsqlConnection(connectionString)) // Veritabanı bağlantısı oluşturur // Creates a new connection to the database
            {
                await connection.OpenAsync();  // Veritabanına bağlan // Opens the connection to the database asynchronously

                foreach (var flightData in flightDataList) // Uçuş verileri listesindeki her bir uçuş için işlem yapılır // Loops through each flight data in the list
                {
                    // Eğer Seen değeri 60 veya daha büyükse, bu uçuşu kaydetme // If Seen value is 60 or greater, skip saving this flight
                    if (flightData.Seen >= 60) // Seen değeri 60 veya daha büyükse atla // If Seen value is greater than or equal to 60, skip this flight
                    {
                        continue;  // Bu uçuşu atla // Skip this flight
                    }

                    // Manuel sıralama ile veriyi hazırlayalım // Prepare the data manually for insertion
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

                    // JSON verisini eklemek için SQL komutunu hazırlayalım // Prepare the SQL command to insert the JSON data
                    var command = new NpgsqlCommand("INSERT INTO flights (data) VALUES (@flightJson)", connection); // SQL komutunu oluşturur // Creates the SQL command
                    command.Parameters.AddWithValue("@flightJson", NpgsqlDbType.Jsonb, flightJson);  // JSONB olarak kaydet // Adds the flightJson as a parameter of type JSONB

                    try
                    {
                        await command.ExecuteNonQueryAsync();  // Veritabanına kaydetme işlemi // Executes the command to insert the data into the database asynchronously
                        Console.WriteLine("Flight data inserted successfully."); // Başarı mesajı // Prints a success message
                    }
                    catch (Exception ex) // Hata oluşursa // If an error occurs
                    {
                        Console.WriteLine($"Error saving flight data to database: {ex.Message}"); // Hata mesajını yazdırır // Prints the error message
                    }
                }
            }
        }
    }
}
