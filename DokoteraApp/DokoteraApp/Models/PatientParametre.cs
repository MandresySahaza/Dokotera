using Npgsql;

namespace DokoteraApp.Models
{
    public class PatientParametre
    {
        public int Id { get; set; }
        public int IdPatient { get; set; }
        public int IdParametre { get; set;}
        public int Niveau { get; set; }

        public PatientParametre(int id, int idPatient, int idParametre, int niveau)
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


    }
}
