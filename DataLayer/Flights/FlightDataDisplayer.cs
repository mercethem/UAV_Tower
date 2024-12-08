using System.Collections.Concurrent;  // Concurrent koleksiyonları için gerekli kütüphane // Required for Concurrent collections

namespace radarApi  // Namespace tanımlaması // Defines the namespace for the project
{
    public class FlightDataDisplayer  // Uçuş verilerini ekranda görüntüler // Class for displaying flight data on screen
    {
        private readonly IFlightDataHandler _flightDataHandler;  // Uçuş verisi işlemlerini yöneten handler // Handler that manages flight data operations

        public FlightDataDisplayer(IFlightDataHandler flightDataHandler)  // Constructor, FlightDataHandler'ı başlatır // Constructor to initialize FlightDataHandler
        {
            _flightDataHandler = flightDataHandler;  // Handler'ı başlatır // Initializes the flight data handler
        }

        public void DisplayUpdatedFlights(List<FlightData> currentData, ConcurrentDictionary<string, (int Count, DateTime LastUpdated)> lastMessageCounts)  // Güncellenmiş uçuşları ekranda görüntüler // Displays updated flights on screen
        {
            // 1. Güncellenmiş uçuşları al // Step 1: Get updated flights
            var updatedFlights = _flightDataHandler.GetUpdatedFlights(currentData, lastMessageCounts);  // Güncellenmiş uçuşları alır // Fetch updated flight data

            // 2. Ekranda görüntüle // Step 2: Display the flights on screen
            _flightDataHandler.DisplayFlights(updatedFlights);  // Ekranda uçuşları görüntüler // Displays the flights on the screen
        }
    }
}
