namespace radarApi 
{
    public abstract class BaseFlightData : IFlightData  // Base class for flight data
    {
        protected string Hex_ICAO { get; }
        protected string Squawk { get; }
        protected string Flight { get; }
        protected double Lat { get; }
        protected double Lon { get; }
        protected int ValidPosition { get; }
        protected int Altitude { get; }
        protected int VertRate { get; }
        protected int Track { get; }
        protected int ValidTrack { get; }
        protected int Speed { get; }
        protected int Messages { get; }
        protected int Seen { get; }

        public BaseFlightData(string hex_ICAO, string squawk, string flight, double lat, double lon, int validPosition, int altitude, int vertRate, int track, int validTrack, int speed, int messages, int seen)
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

        public abstract string ToString();  // Abstract method that returns flight information in string format
        public abstract string ToJson();  // Abstract method that returns flight information in JSON format
    }
}
