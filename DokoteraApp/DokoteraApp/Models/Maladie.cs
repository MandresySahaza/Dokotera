using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace DokoteraApp.Models
{
    public class Maladie
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        public Maladie(int id, string nom)
        {
            Id = id;
            Nom = nom;
        }

        public static List<Maladie> getAllMaladieCorrespondantInTrancheAgeByPatient(NpgsqlConnection conn , Patient patient , TrancheAge trancheAge)
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

                string query = "select idMaladie , nomMaladie from view_MaladieParametreWithNom where idtrancheAge = @trancheAgePatient GROUP BY idMaladie , nomMaladie";
                NpgsqlCommand command = new NpgsqlCommand(query, con);
                command.Parameters.AddWithValue("@trancheAgePatient", trancheAge.Id);
                NpgsqlDataReader reader = command.ExecuteReader();

                List<Maladie> results = new List<Maladie>();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string nom = reader.GetString(1);

                    Maladie rep = new Maladie(id, nom);

                    results.Add(rep);
                }
                reader.Close();

                return results;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " passage getAllMaladieCorrespondantInTrancheAgeByPatient");
            }
            finally
            {
                if (isOpen == true) { con.Close(); }
            }
        }



    }
}
