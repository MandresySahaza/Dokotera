using Npgsql;

namespace DokoteraApp.Models
{
    public class MedicamentParametre
    {
        public int Id { get; set; }
        public int IdMedicament { get; set; }
        public int IdParametre { get; set; }
        public double Apport { get; set; }

        public MedicamentParametre(int id, int idMedicament, int idParametre, double apport)
        {
            Id = id;
            IdMedicament = idMedicament;
            IdParametre = idParametre;
            Apport = apport;
        }

        public static List<MedicamentParametre> getMedicamentParametreByIdParametre(NpgsqlConnection conn, int idParametre ,int age)
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

                string query = "select * from view_MedicamentParametreWithTrancheAge where idParametre = @idParametre and @age >= ageMin and @age <= ageMax";
                NpgsqlCommand command = new NpgsqlCommand(query, con);
                command.Parameters.AddWithValue("@idParametre", idParametre);
                command.Parameters.AddWithValue("@age", age);
                NpgsqlDataReader reader = command.ExecuteReader();

                List<MedicamentParametre> results = new List<MedicamentParametre>();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    int idMedicament = reader.GetInt32(1);
                    int idParams = reader.GetInt32(2);
                    double apport = reader.GetDouble(3);

                    MedicamentParametre rep = new MedicamentParametre(id, idMedicament, idParams, apport);

                    results.Add(rep);
                }
                reader.Close();

                return results;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " passage getMedicamentParametreByIdParametre");
            }
            finally
            {
                if (isOpen == true) { con.Close(); }
            }
        }

        public static List<Parametre> getParametreSitranaMedicament(NpgsqlConnection conn, int idMedicament)
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

                string query = "select * from view_MedicamentParametreWithTrancheAge where idMedicament = @idMedicament";
                NpgsqlCommand command = new NpgsqlCommand(query, con);
                command.Parameters.AddWithValue("@idMedicament", idMedicament);
                NpgsqlDataReader reader = command.ExecuteReader();

                List<Parametre> results = new List<Parametre>();

                while (reader.Read())
                {
                    int id = reader.GetInt32(2);
                    String nom = reader.GetString(4);

                    Parametre rep = new Parametre(id, nom);

                    results.Add(rep);
                }
                reader.Close();

                return results;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " passage getParametreSitranaMedicament");
            }
            finally
            {
                if (isOpen == true) { con.Close(); }
            }
        }


    }
}
