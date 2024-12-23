namespace radarApi
{
    public interface IDataService<T>
    {
        Task<List<T>> GetDataAsync(); // Async method to fetch data of type List<T>
    }
}
