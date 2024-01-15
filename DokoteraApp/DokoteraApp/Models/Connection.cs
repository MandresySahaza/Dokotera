using Npgsql;

namespace DokoteraApp.Models
{
    public class Connection
    {
        public static NpgsqlConnection ConnectPostgres()
        {
            string ConnString = "Server=localhost;Username=postgres;Password=root;Database=dokotera";
            NpgsqlConnection Conn;
            Conn = new NpgsqlConnection(ConnString);


            return Conn;
        }
    }
}
