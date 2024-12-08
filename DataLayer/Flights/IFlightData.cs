namespace radarApi
{
    public interface IFlightData  // Uçuş verisi arayüzü // Interface for flight data
    {
        string ToString();  // Uçuş verisini string formatında döndüren metod // Method to return flight data as a string
        string ToJson();  // Uçuş verisini JSON formatında döndüren metod // Method to return flight data as a JSON string
    }
}
