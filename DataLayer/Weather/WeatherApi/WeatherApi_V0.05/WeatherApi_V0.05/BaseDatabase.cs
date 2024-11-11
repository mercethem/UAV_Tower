using System;
using System.Data.SqlClient;


public abstract class BaseDatabase : IDatabase
{
    protected readonly string connectionString; // Veritabanı bağlantı dizesi.

    // Veritabanı bağlantı dizesini alarak sınıfı başlatan özel yapıcı.
    protected BaseDatabase(string connectionString)
    {
        this.connectionString = connectionString;
    }

    // WeatherInfo nesnesini veritabanına kaydeden metod. Abstrakt metot olarak tanımlanacak.
    public abstract void SaveWeatherInfo(WeatherInfo weatherInfo);

    // Bağlantıyı almak için genel bir metot.
    protected SqlConnection GetConnection()
    {
        return new SqlConnection(this.connectionString);
    }
}
