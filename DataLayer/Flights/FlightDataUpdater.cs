using System.Collections.Concurrent;

namespace radarApi
{
    public class FlightDataUpdater  // Updates flight data
    {
        public void UpdateMessageCounts(List<FlightData> currentData, ConcurrentDictionary<string, (int Count, DateTime LastUpdated)> lastMessageCounts)
        {
            foreach (var flight in currentData)
            {
                var flightKey = flight.Hex_ICAO;
                var currentMessageCount = flight.Messages;


                if (lastMessageCounts.ContainsKey(flightKey))
                {
                    if (lastMessageCounts[flightKey].Count != currentMessageCount)
                    {
                        lastMessageCounts[flightKey] = (currentMessageCount, DateTime.Now);
                    }
                }
                else
                {
                    lastMessageCounts[flightKey] = (currentMessageCount, DateTime.Now);
                }
            }
        }
    }
}
