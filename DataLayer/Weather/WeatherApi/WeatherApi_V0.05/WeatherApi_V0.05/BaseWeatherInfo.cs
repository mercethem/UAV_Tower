public abstract class BaseWeatherInfo : IWeatherInfo
{
    // Ortak alanlar
    protected string description; // Hava durumu açıklaması
    protected double temperature; // Hava sıcaklığı
    protected double feelsLike; // Hissedilen sıcaklık
    protected int humidity; // Nem oranı
    protected double windSpeed; // Rüzgar hızı
    protected int pressure; // Hava basıncı
    protected int clouds; // Bulut oranı
    protected int visibility; // Görüş mesafesi
    protected int weatherId; // Hava durumu ID'si
    protected DateTime sunrise; // Gündoğumu zamanı
    protected DateTime sunset; // Günbatımı zamanı
    protected double lat; // Enlem
    protected double lon; // Boylam
    protected string city; // Şehir adı
    protected DateTime recordDateTime; // Kayıt zamanı

    // Constructor 
    public BaseWeatherInfo(string description, double temperature, double feelsLike, int humidity, double windSpeed, int pressure, int clouds, int visibility, int weatherId, DateTime sunrise, DateTime sunset, double lat, double lon, string city, DateTime recordDateTime)
    {
        //Gelen parametreler ile alanları başlatır.
        this.description = description;
        this.temperature = temperature;
        this.feelsLike = feelsLike;
        this.humidity = humidity;
        this.windSpeed = windSpeed;
        this.pressure = pressure;
        this.clouds = clouds;
        this.visibility = visibility;
        this.weatherId = weatherId;
        this.sunrise = sunrise;
        this.sunset = sunset;
        this.lat = lat;
        this.lon = lon;
        this.city = city;
        this.recordDateTime = recordDateTime;
    }
    internal string Description => this.description;
    internal double Temperature => this.temperature;
    internal double FeelsLike => this.feelsLike;
    internal int Humidity => this.humidity;
    internal double WindSpeed => this.windSpeed;
    internal int Pressure => this.pressure;
    internal int Clouds => this.clouds;
    internal int Visibility => this.visibility;
    internal int WeatherId => this.weatherId;
    internal DateTime Sunrise => this.sunrise;
    internal DateTime Sunset => this.sunset;
    internal double Lat => this.lat;
    internal double Lon => this.lon;
    internal string City => this.city;
    internal DateTime RecordDateTime => this.recordDateTime;

    public abstract string GetWeatherDetails(); // Abstract metod tanımı

}
