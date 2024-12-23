using Npgsql;
using NpgsqlTypes;
using System.Text.Json.Serialization;
using System.Text.Json;
namespace radarApi.FlightDataSaver
{
    public class FlightDataSaverPostgreSql// Class for saving flight data to PostgreSQL database
    {
        private readonly string connectionString;

        public FlightDataSaverPostgreSql(string connectionString) // Constructor method that takes the connection string
        {
            this.connectionString = connectionString;
        }

        public async Task SaveFlightDataAsync(List<FlightData> flightDataList)// Asynchronous method to save flight data to PostgreSQL database
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();

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


                    var command = new NpgsqlCommand("INSERT INTO flights (data) VALUES (@flightJson)", connection);
                    command.Parameters.AddWithValue("@flightJson", NpgsqlDbType.Jsonb, flightJson);

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        Console.WriteLine("Flight data inserted successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error saving flight data to database: {ex.Message}");
                    }
                }
            }
        }
    }
}
