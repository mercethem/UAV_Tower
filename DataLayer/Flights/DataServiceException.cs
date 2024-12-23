namespace radarApi
{
    public class DataServiceException : Exception // Data service exception class
    {
        public DataServiceException(string message) : base(message) { }  // Constructor that takes the error message and passes it to the base Exception class
    }
}
