using System.Data.SqlClient;

public class DatabaseService : BaseDatabase
{
    
    private static readonly object lockObj = new object(); // Thread güvenliğini sağlamak için kullanılan nesne.

    // Veritabanı bağlantı dizesini alarak DatabaseService nesnesini başlatan özel yapıcı.
    public DatabaseService(string connectionString) : base(connectionString) { }

   

    // WeatherInfo nesnesini veritabanına kaydeden metod.
    public override void SaveWeatherInfo(WeatherInfo weatherInfo)
    {
        // Veritabanı bağlantısını aç
        using (SqlConnection connection = GetConnection())
        {
            connection.Open(); // Bağlantı aç

            // Verileri kaydetmek için SQL sorgusu
            string query = @"INSERT INTO WeatherData 
                             (Description, Temperature, FeelsLike, Humidity, WindSpeed, Pressure, Clouds, Visibility, WeatherId, Sunrise, Sunset, Lat, Lon, City, RecordDateTime) 
                             VALUES 
                             (@Description, @Temperature, @FeelsLike, @Humidity, @WindSpeed, @Pressure, @Clouds, @Visibility, @WeatherId, @Sunrise, @Sunset, @Lat, @Lon, @City, @RecordDateTime)";

            // SQL komutunu oluştur.
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // Parametreleri komuta ekle.
                command.Parameters.AddWithValue("@Description", weatherInfo.Description);
                command.Parameters.AddWithValue("@Temperature", weatherInfo.Temperature);
                command.Parameters.AddWithValue("@FeelsLike", weatherInfo.FeelsLike);
                command.Parameters.AddWithValue("@Humidity", weatherInfo.Humidity);
                command.Parameters.AddWithValue("@WindSpeed", weatherInfo.WindSpeed);
                command.Parameters.AddWithValue("@Pressure", weatherInfo.Pressure);
                command.Parameters.AddWithValue("@Clouds", weatherInfo.Clouds);
                command.Parameters.AddWithValue("@Visibility", weatherInfo.Visibility);
                command.Parameters.AddWithValue("@WeatherId", weatherInfo.WeatherId);
                command.Parameters.AddWithValue("@Sunrise", weatherInfo.Sunrise);
                command.Parameters.AddWithValue("@Sunset", weatherInfo.Sunset);
                command.Parameters.AddWithValue("@Lat", weatherInfo.Lat);
                command.Parameters.AddWithValue("@Lon", weatherInfo.Lon);
                command.Parameters.AddWithValue("@City", weatherInfo.City);
                command.Parameters.AddWithValue("@RecordDateTime", weatherInfo.RecordDateTime);

                // Komutu çalıştır
                command.ExecuteNonQuery();
            }
        }
    }
}
