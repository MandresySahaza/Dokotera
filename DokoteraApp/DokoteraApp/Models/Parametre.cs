using Npgsql;

namespace DokoteraApp.Models
{
    public class Parametre
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        public Parametre(int id, string nom)
        {
            Id = id;
            Nom = nom;
        }

        public static List<Parametre> getAllParametre(NpgsqlConnection conn)
        {
            Boolean isOpen = false;
            NpgsqlConnection con = null;
            try
            {
                if (conn == null)
                {
                    con = Connection.ConnectPostgres();
                    con.Open();
                    isOpen = true;
                }
                else
                {
                    con = conn;
                }

                string query = "SELECT * from parametre";
                NpgsqlCommand command = new NpgsqlCommand(query, con);
                NpgsqlDataReader reader = command.ExecuteReader();

                List<Parametre> results = new List<Parametre>();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string nom = reader.GetString(1);
                    
                    Parametre rep = new Parametre(id, nom);

                    results.Add(rep);
                }
                reader.Close();

                return results;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " passage  getAllParametre");
            }
            finally
            {
                if (isOpen == true) { con.Close(); }
            }
        }



    }
}
