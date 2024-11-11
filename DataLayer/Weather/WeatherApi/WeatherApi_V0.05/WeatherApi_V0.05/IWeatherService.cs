using System;


public interface IWeatherClass
{   
    // Verilen şehir adı için hava durumu bilgilerini asenkron olarak alır.

    Task<WeatherInfo> GetWeatherAsync(string city);

}