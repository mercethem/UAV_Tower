using System.Collections.Concurrent;

namespace radarApi
{
    public interface IFlightDataHandler  // Interface for handling flight data operations
    {
        List<FlightData> GetUpdatedFlights(
            List<FlightData> currentData,
            ConcurrentDictionary<string, (int Count, DateTime LastUpdated)> lastMessageCounts
        );

        void DisplayFlights(List<FlightData> flights);
    }
}
