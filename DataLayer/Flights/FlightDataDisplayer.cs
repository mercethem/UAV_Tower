using System.Collections.Concurrent;

namespace radarApi
{
    public class FlightDataDisplayer  // Uçuş verilerini ekranda görüntüler // Class for displaying flight data on screen
    {
        private readonly IFlightDataHandler flightDataHandler;

        public FlightDataDisplayer(IFlightDataHandler flightDataHandler) // Constructor to initialize FlightDataHandler
        {
            this.flightDataHandler = flightDataHandler;
        }

        public void DisplayUpdatedFlights(List<FlightData> currentData, ConcurrentDictionary<string, (int Count, DateTime LastUpdated)> lastMessageCounts) // Displays updated flights on screen
        {
            // Step 1: Get updated flights
            var updatedFlights = flightDataHandler.GetUpdatedFlights(currentData, lastMessageCounts);

            // Step 2: Display the flights on screen
            flightDataHandler.DisplayFlights(updatedFlights);
        }
    }
}
