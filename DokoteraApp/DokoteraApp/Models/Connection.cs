using Npgsql;

namespace DokoteraApp.Models
{
    public class Connection
    {
        public static NpgsqlConnection ConnectPostgres()
        {
            string ConnString = "Server=localhost;Username=postgres;Password=root;Database=dokotera;Port=5432";
            NpgsqlConnection Conn;
            Conn = new NpgsqlConnection(ConnString);


            return Conn;
        }
    }
}
