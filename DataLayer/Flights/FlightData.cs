using System.Text.Json.Serialization;  
using System.Text.Json;  

namespace radarApi  
{
    public class FlightData : BaseFlightData  // FlightData, BaseFlightData sınıfını miras alır 
    {
        [JsonPropertyName("hex")]  
        public string Hex_ICAO { get; set; }
        [JsonPropertyName("squawk")]  
        public string Squawk { get; set; }
        [JsonPropertyName("flight")]  
        public string Flight { get; set; }
        [JsonPropertyName("lat")]  
        public double Lat { get; set; }
        [JsonPropertyName("lon")]  
        public double Lon { get; set; }  
        [JsonPropertyName("validposition")]  
        public int ValidPosition { get; set; }  
        [JsonPropertyName("altitude")]  
        public int Altitude { get; set; }  
        [JsonPropertyName("vert_rate")]  
        public int VertRate { get; set; }  
        [JsonPropertyName("track")]  
        public int Track { get; set; }  
        [JsonPropertyName("validtrack")]
        public int ValidTrack { get; set; }
        [JsonPropertyName("speed")]  
        public int Speed { get; set; }  
        [JsonPropertyName("messages")]  
        public int Messages { get; set; }  
        [JsonPropertyName("seen")]  
        public int Seen { get; set; }  

        public FlightData(string hex_ICAO, string squawk, string flight, double lat, double lon, int validPosition,
            int altitude, int vertRate, int track, int validTrack, int speed, int messages, int seen)
            : base(hex_ICAO, squawk, flight, lat, lon, validPosition, altitude, vertRate, track, validTrack, speed, messages, seen)
        {
            Hex_ICAO = hex_ICAO;  
            Squawk = squawk;  
            Flight = flight;  
            Lat = lat;  
            Lon = lon;  
            ValidPosition = validPosition;  
            Altitude = altitude;  
            VertRate = vertRate;  
            Track = track;  
            ValidTrack = validTrack;  
            Speed = speed;  
            Messages = messages;  
            Seen = seen;  
        }

        public override string ToString() // Converts flight data into a human-readable string format
        {
            return $"Flight: {Flight.Trim()}, " +  
                   $"Hex: {Hex_ICAO}, " +  
                   $"Squawk: {Squawk}, " + 
                   $"Latitude: {Lat}, Longitude: {Lon}, " +  
                   $"Altitude: {Altitude} ft, Speed: {Speed} knots, " + 
                   $"Vertical Rate: {VertRate} ft/min, Track: {Track}, " +  
                   $"Valid Position: {ValidPosition}, Valid Track: {ValidTrack}, " +  
                   $"Messages: {Messages}, Seen: {Seen}, Last Updated: {DateTime.Now}";  
        }

        public override string ToJson()  // Uçuş bilgisini JSON formatına dönüştürür // Converts flight data to JSON format
        {
            try
            {
                var flightInfo = new  
                {
                    Flight = this.Flight.Trim(),
                    Hex_ICAO = this.Hex_ICAO,
                    Squawk = this.Squawk,
                    Latitude = this.Lat,
                    Longitude = this.Lon,
                    Altitude = this.Altitude,
                    Speed = this.Speed,
                    VertRate = this.VertRate,
                    Track = this.Track,
                    ValidPosition = this.ValidPosition,
                    ValidTrack = this.ValidTrack,
                    Messages = this.Messages,
                    Seen = this.Seen,
                    LastUpdated = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") 
                };

                return System.Text.Json.JsonSerializer.Serialize(flightInfo, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,  
                    WriteIndented = true  
                });
            }
            catch (Exception ex)  
            {
                Console.WriteLine($"Serialization error: {ex.Message}"); 
                return string.Empty;  
            }
        }

        public void UpdateMissingFields()  // Eksik verileri "Unknown" olarak günceller // Updates missing fields to "Unknown"
        {
            if (string.IsNullOrEmpty(Flight)) Flight = "Unknown";  
            if (string.IsNullOrEmpty(Squawk) || Squawk == "0000") Squawk = "Unknown";
            if (string.IsNullOrEmpty(Hex_ICAO)) Hex_ICAO = "Unknown";  
        }
    }
}
