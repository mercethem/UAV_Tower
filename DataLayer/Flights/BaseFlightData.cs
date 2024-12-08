namespace radarApi // Namespace tanımlaması // Defines the namespace for the project
{
    public abstract class BaseFlightData : IFlightData  // Uçuş verilerinin temel sınıfı // Base class for flight data
    {
        protected string Hex_ICAO { get; }  // Uçuşun ICAO hex kodu (örneğin uçağın benzersiz ID'si) // The ICAO hex code of the flight (e.g., unique ID of the aircraft)
        protected string Squawk { get; }  // Uçuşun squawk kodu (radarla iletişim için kullanılan kod) // The squawk code of the flight (code used for radar communication)
        protected string Flight { get; }  // Uçuş numarası // Flight number
        protected double Lat { get; }  // Uçağın enlem koordinatı // Latitude of the aircraft
        protected double Lon { get; }  // Uçağın boylam koordinatı // Longitude of the aircraft
        protected int ValidPosition { get; }  // Geçerli konum bilgisi (0 veya 1) // Valid position information (0 or 1)
        protected int Altitude { get; }  // Uçağın irtifası (feet cinsinden) // Altitude of the aircraft (in feet)
        protected int VertRate { get; }  // Uçağın dikey hız bilgisi (feet/dakika) // Vertical rate of the aircraft (in feet per minute)
        protected int Track { get; }  // Uçağın yönü (derece) // Track (direction) of the aircraft (in degrees)
        protected int ValidTrack { get; }  // Geçerli yön bilgisi (0 veya 1) // Valid track information (0 or 1)
        protected int Speed { get; }  // Uçağın hızı (knots cinsinden) // Speed of the aircraft (in knots)
        protected int Messages { get; }  // Uçuşa ait mesaj sayısı // Number of messages related to the flight
        protected int Seen { get; }  // Uçuşun son görüldüğü zaman (saniye olarak) // Last time the flight was seen (in seconds)

        public BaseFlightData(string hex_ICAO, string squawk, string flight, double lat, double lon, int validPosition, int altitude, int vertRate, int track, int validTrack, int speed, int messages, int seen)
        {
            Hex_ICAO = hex_ICAO;  // Constructor: Hex_ICAO değeri ataması // Constructor: Assigns Hex_ICAO value
            Squawk = squawk;  // Constructor: Squawk değeri ataması // Constructor: Assigns Squawk value
            Flight = flight;  // Constructor: Flight numarası ataması // Constructor: Assigns Flight number value
            Lat = lat;  // Constructor: Latitude değeri ataması // Constructor: Assigns Latitude value
            Lon = lon;  // Constructor: Longitude değeri ataması // Constructor: Assigns Longitude value
            ValidPosition = validPosition;  // Constructor: ValidPosition değeri ataması // Constructor: Assigns ValidPosition value
            Altitude = altitude;  // Constructor: Altitude değeri ataması // Constructor: Assigns Altitude value
            VertRate = vertRate;  // Constructor: Vertical rate değeri ataması // Constructor: Assigns Vertical rate value
            Track = track;  // Constructor: Track değeri ataması // Constructor: Assigns Track value
            ValidTrack = validTrack;  // Constructor: ValidTrack değeri ataması // Constructor: Assigns ValidTrack value
            Speed = speed;  // Constructor: Speed değeri ataması // Constructor: Assigns Speed value
            Messages = messages;  // Constructor: Messages değeri ataması // Constructor: Assigns Messages value
            Seen = seen;  // Constructor: Seen değeri ataması // Constructor: Assigns Seen value
        }

        public abstract string ToString();  // Uçuş bilgisini string formatında döndüren abstract metod // Abstract method that returns flight information in string format
        public abstract string ToJson();  // Uçuş bilgisini JSON formatında döndüren abstract metod // Abstract method that returns flight information in JSON format
    }
}
