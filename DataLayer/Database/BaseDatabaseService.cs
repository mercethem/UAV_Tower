public abstract class DatabaseService : IDatabaseService
{
    public abstract bool ConnectionTest();

    public abstract void ConnectionDataBase();

    public abstract string GetValue(string query);

    public abstract void SetValue(string data);

    public abstract void UpdateValue(string data);

    public abstract void DeleteValue(string data);
}
