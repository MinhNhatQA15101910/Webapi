using System.Data.SQLite;

namespace Webapi.Infrastructure.Persistence;

public class ApplicationContext
{
    private static ApplicationContext? _instance;
    private static readonly Lock _lock = new();
    private readonly SQLiteConnection _connection;

    private ApplicationContext()
    {
        string connectionString = "Data Source=nike_clone_2.db;Version=3;";
        _connection = new SQLiteConnection(connectionString);
        _connection.Open();
    }

    public static ApplicationContext GetInstance()
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                _instance ??= new ApplicationContext();
            }
        }

        return _instance;
    }

    public SQLiteConnection GetConnection()
    {
        return _connection;
    }

    public void CloseConnection()
    {
        if (_connection.State != System.Data.ConnectionState.Closed)
        {
            _connection.Close();
        }
    }
}
