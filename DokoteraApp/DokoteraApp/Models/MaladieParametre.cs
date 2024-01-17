using Npgsql;

namespace DokoteraApp.Models
{
    public class MaladieParametre
    {
        public int Id { get; set; }
        public int IdMaladie { get; set; }
        public int IdParametre { get; set; }
        public double NiveauMin { get; set; }
        public double NiveauMax { get; set; }
        public int TrancheAge { get; set; }

        public MaladieParametre(int id, int idMaladie, int idParametre, double niveauMin, double niveauMax, int trancheAge)
        {
            Id = id;
            IdMaladie = idMaladie;
            IdParametre = idParametre;
            NiveauMin = niveauMin;
            NiveauMax = niveauMax;
            TrancheAge = trancheAge;
        }

        public static List<MaladieParametre> getParametreMaladieInMyTrancheAgeByIdMaladie(NpgsqlConnection conn, int idMaladie , int idTrancheAge)
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

                string query = "select * from MaladieParametre where idMaladie= @idMaladie and idtrancheAge = @idtrancheAge";
                NpgsqlCommand command = new NpgsqlCommand(query, con);
                command.Parameters.AddWithValue("@idMaladie", idMaladie);
                command.Parameters.AddWithValue("@idtrancheAge", idTrancheAge);
                NpgsqlDataReader reader = command.ExecuteReader();

                List<MaladieParametre> results = new List<MaladieParametre>();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    int idMal = reader.GetInt32(1);
                    int idParametre = reader.GetInt32(2);
                    double niveauMin = reader.GetDouble(3);
                    double niveauMax = reader.GetDouble(4);
                    int idtrancheage = reader.GetInt32(5);

                    MaladieParametre rep = new MaladieParametre(id, idMal , idParametre ,niveauMin , niveauMax , idtrancheage);

                    results.Add(rep);
                }
                reader.Close();

                return results;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " passage getParametreMaladieInMyTrancheAgeByIdMaladie");
            }
            finally
            {
                if (isOpen == true) { con.Close(); }
            }
        }

    }

}
