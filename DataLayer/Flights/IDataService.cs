namespace radarApi
{
    public interface IDataService<T>  // Interface: Generic veri servisi tanımı // Interface: Defines a generic data service
    {
        Task<List<T>> GetDataAsync();  // Async metod: List<T> türünde veri almak için // Async method to fetch data of type List<T>
    }
}
