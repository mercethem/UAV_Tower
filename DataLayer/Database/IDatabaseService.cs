public interface IDatabaseService
{
    void ConnectionDataBase();
    bool ConnectionTest();
    string GetValue(string query);
    void SetValue(string data);
    void UpdateValue(string data);
    void DeleteValue(string data);
}
