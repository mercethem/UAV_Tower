using System.Collections.Concurrent;

namespace radarApi
{
    public interface IFlightDataHandler  // Uçuş verisi işlemleri için bir arayüz // Interface for handling flight data operations
    {
        // Güncellenmiş uçuş verilerini alır // Method to get updated flight data
        List<FlightData> GetUpdatedFlights(
            List<FlightData> currentData,  // Mevcut uçuş verileri // List of current flight data
            ConcurrentDictionary<string, (int Count, DateTime LastUpdated)> lastMessageCounts  // Son mesaj sayısı ve güncelleme zamanı // Dictionary of last message count and last updated time
        );

        // Uçuş verilerini ekranda görüntüler // Method to display flight data on screen
        void DisplayFlights(List<FlightData> flights);  // Uçuş verileri listesini alır ve görüntüler // Takes a list of flight data and displays it
    }
}
