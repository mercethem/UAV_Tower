public class WeatherInfo
{
    private string _description; // Weather description // Hava durumu açıklaması
    private double _temperature; // Temperature (°C) // Sıcaklık (°C)
    private double _feelsLike; // Feels like temperature (°C) // Hissedilen sıcaklık (°C)
    private int _humidity; // Humidity (%) // Nem (%)
    private double _windSpeed; // Wind speed (m/s) // Rüzgar hızı (m/s)
    private int _pressure; // Pressure (hPa) // Basınç (hPa)
    private int _clouds; // Cloud coverage (%) // Bulutluluk (%)
    private int _visibility; // Visibility (meters) // Görüş mesafesi (metre)
    private int _weatherId; // Weather condition ID // Hava durumu kimliği
    private DateTime _sunrise; // Sunrise time // Güneşin doğuş saati
    private DateTime _sunset; // Sunset time // Güneşin batış saati
    private double _lat; // Latitude // Enlem
    private double _lon; // Longitude // Boylam
    private string _city; // City name // Şehir adı
    private DateTime _recordDateTime; // Record time // Kayıt zamanı

    // Constructor for WeatherInfo // WeatherInfo yapıcısı
    public WeatherInfo(string description, double temperature, double feelsLike, int humidity, double windSpeed, int pressure, int clouds, int visibility, int weatherId, DateTime sunrise, DateTime sunset, double lat, double lon, string city, DateTime recordDateTime)
    {
        _description = description;
        _temperature = temperature;
        _feelsLike = feelsLike;
        _humidity = humidity;
        _windSpeed = windSpeed;
        _pressure = pressure;
        _clouds = clouds;
        _visibility = visibility;
        _weatherId = weatherId;
        _sunrise = sunrise;
        _sunset = sunset;
        _lat = lat;
        _lon = lon;
        _city = city;
        _recordDateTime = recordDateTime;
    }

    // Properties // Özellikler
    public string Description => _description; // Weather description // Hava durumu açıklaması
    public double Temperature => _temperature; // Temperature // Sıcaklık
    public double FeelsLike => _feelsLike; // Feels like temperature // Hissedilen sıcaklık
    public int Humidity => _humidity; // Humidity // Nem
    public double WindSpeed => _windSpeed; // Wind speed // Rüzgar hızı
    public int Pressure => _pressure; // Pressure // Basınç
    public int Clouds => _clouds; // Cloud coverage // Bulutluluk
    public int Visibility => _visibility; // Visibility // Görüş mesafesi
    public int WeatherId => _weatherId; // Weather condition ID // Hava durumu kimliği
    public DateTime Sunrise => _sunrise; // Sunrise time // Güneşin doğuş saati
    public DateTime Sunset => _sunset; // Sunset time // Güneşin batış saati
    public double Lat => _lat; // Latitude // Enlem
    public double Lon => _lon; // Longitude // Boylam
    public string City => _city; // City name // Şehir adı
    public DateTime RecordDateTime => _recordDateTime; // Record time // Kayıt zamanı

    // Returns weather information as a string // Hava durumu bilgisini string olarak döndür
    public override string ToString()
    {
        return $"Hava Durumu: {_description}\n" +
               $"Sıcaklık: {_temperature}°C\n" +
               $"Hissedilen Sıcaklık: {_feelsLike}°C\n" +
               $"Nem: {_humidity}%\n" +
               $"Rüzgar Hızı: {_windSpeed} m/s\n" +
               $"Basınç: {_pressure} hPa\n" +
               $"Bulutluluk: {_clouds}%\n" +
               $"Görüş Mesafesi: {_visibility} metre\n" +
               $"Hava Durumu Kimliği: {_weatherId}\n" +
               $"Güneşin Doğuş Saati: {_sunrise}\n" +
               $"Güneşin Batış Saati: {_sunset}\n" +
               $"Enlem: {_lat}\n" +
               $"Boylam: {_lon}\n" +
               $"Kayıt Zamanı: {_recordDateTime}";
    }
}
