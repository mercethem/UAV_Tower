public abstract class DatabaseService : IDatabaseService
{

    public abstract bool ConnectionTest();

    // Bağlantıyı kurma işlemi (örnek olarak tüm veritabanları için ortak bir işlem)
    public abstract void ConnectionDataBase();

    // Veriyi almak için ortak bir metot (Her veritabanı türü farklı implementasyon sağlar)
    public abstract string GetValue(string query);

    // Veriyi eklemek için ortak bir metot
    public abstract void SetValue(string data);

    // Veriyi güncellemek için ortak bir metot
    public abstract void UpdateValue(string data);

    // Veriyi silmek için ortak bir metot
    public abstract void DeleteValue(string data);
}
