using System.Collections.Concurrent;

namespace radarApi
{
    public class FlightDataHandler : IFlightDataHandler  // Uçuş verileriyle ilgili işlemleri yönetir // Manages flight data processing
    {
        public List<FlightData> GetUpdatedFlights(List<FlightData> currentData, ConcurrentDictionary<string, (int Count, DateTime LastUpdated)> lastMessageCounts)
        {
            var updatedFlights = new List<FlightData>();  // Güncellenmiş uçuş verilerini tutacak liste // List to hold updated flight data

            foreach (var flight in currentData)  // Her bir uçuş verisini inceler // Iterates through each flight data
            {
                // Filtreleme logic, sadece Seen değerini kontrol ediyoruz // Filtering logic, checking only the Seen value
                if (flight.Seen < 60)  // Eğer uçuş son 60 saniye içinde görüldüyse, güncel kabul edilir // If the flight was seen within the last 60 seconds, it's considered updated
                {
                    updatedFlights.Add(flight);  // Güncellenmiş uçuşları listeye ekler // Adds updated flights to the list
                }
            }

            return updatedFlights;  // Güncellenmiş uçuşları döndürür // Returns the updated flights
        }

        public void DisplayFlights(List<FlightData> flights)  // Uçuşları ekranda görüntüler // Displays flights on the screen
        {
            Console.Clear();  // Konsolu temizler // Clears the console
            Console.WriteLine($"Displaying {flights.Count} flights...\n");  // Gösterilecek uçuş sayısını yazdırır // Prints the number of flights to be displayed
            foreach (var flight in flights)  // Her uçuşu ekrana yazdırır // Iterates through each flight and displays it
            {
                Console.WriteLine(flight.ToJson());  // Her uçuşu JSON formatında ekrana yazdırır // Displays each flight in JSON format
                                                     //Console.WriteLine(flight.ToString());  // Ya da okunabilir formatta yazdırabilir // Or it could display in a readable format
                Console.WriteLine();  // Satır boşluk bırakır // Adds a blank line
                Task.Delay(1000).Wait();  // 1 saniye bekler // Waits for 1 second
            }
        }
    }
}
