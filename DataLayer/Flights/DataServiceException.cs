namespace radarApi // Namespace tanımlaması // Defines the namespace for the project
{
    public class DataServiceException : Exception  // Veri servisi hata sınıfı // Data service exception class
    {
        public DataServiceException(string message) : base(message) { }  // Hata mesajını alıp Exception'a ileten constructor // Constructor that takes the error message and passes it to the base Exception class
    }
}
