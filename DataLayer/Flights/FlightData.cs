using System.Text.Json.Serialization;  // JSON serileştirme için gerekli kütüphane // Library needed for JSON serialization
using System.Text.Json;  // JSON serileştirme için gerekli kütüphane // Library for JSON handling

namespace radarApi  // Namespace tanımlaması // Defines the namespace for the project
{
    public class FlightData : BaseFlightData  // FlightData, BaseFlightData sınıfını miras alır // FlightData inherits from BaseFlightData
    {
        [JsonPropertyName("hex")]  // JSON serileştirme için property adı belirler // Specifies the JSON property name for Hex_ICAO
        public string Hex_ICAO { get; set; }  // Uçuşun ICAO hex kodu // ICAO hex code of the flight
        [JsonPropertyName("squawk")]  // JSON serileştirme için property adı belirler // Specifies the JSON property name for Squawk
        public string Squawk { get; set; }  // Uçuşun squawk kodu // Squawk code for the flight
        [JsonPropertyName("flight")]  // JSON serileştirme için property adı belirler // Specifies the JSON property name for Flight
        public string Flight { get; set; }  // Uçuş numarası // Flight number
        [JsonPropertyName("lat")]  // JSON serileştirme için property adı belirler // Specifies the JSON property name for Latitude
        public double Lat { get; set; }  // Uçağın enlem koordinatı // Latitude coordinate of the aircraft
        [JsonPropertyName("lon")]  // JSON serileştirme için property adı belirler // Specifies the JSON property name for Longitude
        public double Lon { get; set; }  // Uçağın boylam koordinatı // Longitude coordinate of the aircraft
        [JsonPropertyName("validposition")]  // JSON serileştirme için property adı belirler // Specifies the JSON property name for ValidPosition
        public int ValidPosition { get; set; }  // Geçerli konum bilgisi // Valid position indicator
        [JsonPropertyName("altitude")]  // JSON serileştirme için property adı belirler // Specifies the JSON property name for Altitude
        public int Altitude { get; set; }  // Uçağın irtifası // Altitude of the aircraft
        [JsonPropertyName("vert_rate")]  // JSON serileştirme için property adı belirler // Specifies the JSON property name for Vertical Rate
        public int VertRate { get; set; }  // Uçağın dikey hız bilgisi // Vertical rate of the aircraft
        [JsonPropertyName("track")]  // JSON serileştirme için property adı belirler // Specifies the JSON property name for Track
        public int Track { get; set; }  // Uçağın yönü // Track direction of the aircraft
        [JsonPropertyName("validtrack")]  // JSON serileştirme için property adı belirler // Specifies the JSON property name for ValidTrack
        public int ValidTrack { get; set; }  // Geçerli yön bilgisi // Valid track indicator
        [JsonPropertyName("speed")]  // JSON serileştirme için property adı belirler // Specifies the JSON property name for Speed
        public int Speed { get; set; }  // Uçağın hızı // Speed of the aircraft
        [JsonPropertyName("messages")]  // JSON serileştirme için property adı belirler // Specifies the JSON property name for Messages
        public int Messages { get; set; }  // Uçuşa ait mesaj sayısı // Number of messages related to the flight
        [JsonPropertyName("seen")]  // JSON serileştirme için property adı belirler // Specifies the JSON property name for Seen
        public int Seen { get; set; }  // Uçuşun son görüldüğü zaman // Time when the flight was last seen (in seconds)

        public FlightData(string hex_ICAO, string squawk, string flight, double lat, double lon, int validPosition,
            int altitude, int vertRate, int track, int validTrack, int speed, int messages, int seen)
            : base(hex_ICAO, squawk, flight, lat, lon, validPosition, altitude, vertRate, track, validTrack, speed, messages, seen)
        {
            Hex_ICAO = hex_ICAO;  // Constructor: Hex_ICAO değeri ataması // Assign Hex_ICAO value
            Squawk = squawk;  // Constructor: Squawk değeri ataması // Assign Squawk value
            Flight = flight;  // Constructor: Flight numarası ataması // Assign Flight number
            Lat = lat;  // Constructor: Latitude değeri ataması // Assign Latitude value
            Lon = lon;  // Constructor: Longitude değeri ataması // Assign Longitude value
            ValidPosition = validPosition;  // Constructor: ValidPosition değeri ataması // Assign ValidPosition value
            Altitude = altitude;  // Constructor: Altitude değeri ataması // Assign Altitude value
            VertRate = vertRate;  // Constructor: Vertical rate değeri ataması // Assign Vertical Rate value
            Track = track;  // Constructor: Track değeri ataması // Assign Track value
            ValidTrack = validTrack;  // Constructor: ValidTrack değeri ataması // Assign ValidTrack value
            Speed = speed;  // Constructor: Speed değeri ataması // Assign Speed value
            Messages = messages;  // Constructor: Messages değeri ataması // Assign Messages value
            Seen = seen;  // Constructor: Seen değeri ataması // Assign Seen value
        }

        public override string ToString()  // Uçuş verisini okunabilir bir formatta döndürür // Converts flight data into a human-readable string format
        {
            return $"Flight: {Flight.Trim()}, " +  // Flight bilgisi // Flight information
                   $"Hex: {Hex_ICAO}, " +  // ICAO Hex kodu // ICAO Hex code
                   $"Squawk: {Squawk}, " +  // Squawk kodu // Squawk code
                   $"Latitude: {Lat}, Longitude: {Lon}, " +  // Enlem ve boylam // Latitude and Longitude
                   $"Altitude: {Altitude} ft, Speed: {Speed} knots, " +  // İrtifa ve hız // Altitude and Speed
                   $"Vertical Rate: {VertRate} ft/min, Track: {Track}, " +  // Dikey hız ve yön // Vertical rate and track
                   $"Valid Position: {ValidPosition}, Valid Track: {ValidTrack}, " +  // Geçerli konum ve yön // Valid position and track
                   $"Messages: {Messages}, Seen: {Seen}, Last Updated: {DateTime.Now}";  // Mesaj sayısı ve son görülme zamanı // Message count and last seen time
        }

        public override string ToJson()  // Uçuş bilgisini JSON formatına dönüştürür // Converts flight data to JSON format
        {
            try
            {
                var flightInfo = new  // Uçuş bilgilerini içeren anonim nesne // Anonymous object containing flight information
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
                    LastUpdated = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")  // Uçuş bilgisini ISO formatında JSON'a dönüştürür // Converts flight data to ISO format in JSON
                };

                return System.Text.Json.JsonSerializer.Serialize(flightInfo, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,  // Null değerleri atlar // Ignores null values during serialization
                    WriteIndented = true  // JSON çıktısını okunabilir hale getirir (güzelce hizalar) // Makes the JSON output readable (pretty prints)
                });
            }
            catch (Exception ex)  // JSON serileştirme hatası yakalanır // Catches JSON serialization errors
            {
                Console.WriteLine($"Serialization error: {ex.Message}");  // Hata mesajını yazdırır // Logs the error message
                return string.Empty;  // Boş string döndürür // Returns an empty string if an error occurs
            }
        }

        public void UpdateMissingFields()  // Eksik verileri "Unknown" olarak günceller // Updates missing fields to "Unknown"
        {
            if (string.IsNullOrEmpty(Flight)) Flight = "Unknown";  // Flight boşsa "Unknown" olarak değiştirir // Sets Flight to "Unknown" if empty
            if (string.IsNullOrEmpty(Squawk) || Squawk == "0000") Squawk = "Unknown";  // Squawk boş veya "0000" ise "Unknown" yapar // Sets Squawk to "Unknown" if empty or "0000"
            if (string.IsNullOrEmpty(Hex_ICAO)) Hex_ICAO = "Unknown";  // Hex_ICAO boşsa "Unknown" yapar // Sets Hex_ICAO to "Unknown" if empty
        }
    }
}
