using System;


public interface IJsonParser
{  
    
    // Bir Json yanıtını ve şehir adını alarak WeatherInfo nesnesi döndürür.
    WeatherInfo ParseWeather(string jsonResponse, string city);
}
