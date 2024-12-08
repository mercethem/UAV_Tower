using System.Collections.Concurrent;

namespace radarApi
{
    public class FlightDataUpdater  // Uçuş verilerini günceller // Updates flight data
    {
        public void UpdateMessageCounts(List<FlightData> currentData, ConcurrentDictionary<string, (int Count, DateTime LastUpdated)> lastMessageCounts)
        {
            foreach (var flight in currentData)  // Her uçuşu günceller // Iterates through each flight to update
            {
                var flightKey = flight.Hex_ICAO;  // Uçuşun benzersiz ICAO kodu // Unique ICAO code for the flight
                var currentMessageCount = flight.Messages;  // Uçuşun mevcut mesaj sayısı // The current message count for the flight

                // Sadece mesaj sayısı değişen uçuşları güncelliyoruz // Only updates flights where the message count has changed
                if (lastMessageCounts.ContainsKey(flightKey))  // Eğer uçuş zaten var ise // If the flight already exists in the dictionary
                {
                    if (lastMessageCounts[flightKey].Count != currentMessageCount)  // Eğer mesaj sayısı değişmişse // If the message count has changed
                    {
                        lastMessageCounts[flightKey] = (currentMessageCount, DateTime.Now);  // Mesaj sayısı değişmişse günceller // Updates with the new message count and timestamp
                    }
                }
                else  // Eğer uçuş daha önce eklenmemişse // If the flight is not in the dictionary yet
                {
                    lastMessageCounts[flightKey] = (currentMessageCount, DateTime.Now);  // İlk kez görüldüğünde ekler // Adds the flight with the message count and timestamp
                }
            }
        }
    }
}
