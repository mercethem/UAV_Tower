using System.Collections.Concurrent;

namespace radarApi
{
    public class FlightDataHandler : IFlightDataHandler // Manages flight data processing
    {
        public List<FlightData> GetUpdatedFlights(List<FlightData> currentData, ConcurrentDictionary<string, (int Count, DateTime LastUpdated)> lastMessageCounts)
        {
            var updatedFlights = new List<FlightData>(); // List to hold updated flight data

            foreach (var flight in currentData)
            {

                if (flight.Seen < 60)
                {
                    updatedFlights.Add(flight);
                }
            }

            return updatedFlights;
        }

        public void DisplayFlights(List<FlightData> flights)  // Uçuşları ekranda görüntüler // Displays flights on the screen
        {
            Console.Clear();
            Console.WriteLine($"Displaying {flights.Count} flights...\n");
            foreach (var flight in flights)
            {
                Console.WriteLine(flight.ToJson());
                Console.WriteLine();
                Task.Delay(1000).Wait();
            }
        }
    }
}
