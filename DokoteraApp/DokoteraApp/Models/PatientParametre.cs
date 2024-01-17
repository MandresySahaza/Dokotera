using Npgsql;

namespace DokoteraApp.Models
{
    public class PatientParametre
    {
        public int Id { get; set; }
        public int IdPatient { get; set; }
        public int IdParametre { get; set;}
        public double Niveau { get; set; }

        public PatientParametre(int id, int idPatient, int idParametre, double niveau)
        {
            Id = id;
            IdPatient = idPatient;
            IdParametre = idParametre;
            Niveau = niveau;
        }

        public int InsertPatientParametre(NpgsqlConnection conn)
        {
            bool isOpen = false;
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
                NpgsqlCommand command = new NpgsqlCommand($"INSERT INTO PatientParametre(idPatient , idParametre , niveau) VALUES (@idPatient , @idParametre , @niveau) RETURNING id", con);
                command.Parameters.AddWithValue("@idPatient", this.IdPatient);
                command.Parameters.AddWithValue("@idParametre", this.IdParametre);
                command.Parameters.AddWithValue("@niveau", this.Niveau);
                
                NpgsqlDataReader reader = command.ExecuteReader();
                int results = 0;
                while (reader.Read())
                {
                    results = reader.GetInt32(0);
                }

                reader.Close();

                return results;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " passage InsertPatientParametre");
            }
            finally
            {
                if (isOpen == true) { con.Close(); }
            }
        }

        public static List<PatientParametre> getPatientParametreByIdPatient(NpgsqlConnection conn, int idPatient)
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

                string query = "select * from PatientParametre where idPatient = @idPatient and niveau != 0";
                NpgsqlCommand command = new NpgsqlCommand(query, con);
                command.Parameters.AddWithValue("@idPatient", idPatient);
                NpgsqlDataReader reader = command.ExecuteReader();

                List<PatientParametre> results = new List<PatientParametre>();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    int idPat = reader.GetInt32(1);
                    int idParametre = reader.GetInt32(2);
                    double niveau = reader.GetDouble(3);

                    PatientParametre rep = new PatientParametre(id, idPat , idParametre , niveau);

                    results.Add(rep);
                }
                reader.Close();

                return results;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " passage getPatientParametreByIdPatient");
            }
            finally
            {
                if (isOpen == true) { con.Close(); }
            }
        }

        
    }
}
