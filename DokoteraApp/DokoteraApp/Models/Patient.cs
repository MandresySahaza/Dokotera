using Npgsql;

namespace DokoteraApp.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public int Age { get; set; }

        public Patient(int id, string nom, int age)
        {
            Id = id;
            Nom = nom;
            Age = age;
        }

        public int InsertPatient(NpgsqlConnection conn)
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
                NpgsqlCommand command = new NpgsqlCommand($"INSERT INTO Patient(nom ,age) VALUES (@nom ,@age) RETURNING id", con);
                command.Parameters.AddWithValue("@nom", this.Nom);
                command.Parameters.AddWithValue("@age", this.Age);

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
                throw new Exception(e.Message + " passage InsertPatient");
            }
            finally
            {
                if (isOpen == true) { con.Close(); }
            }
        }




    }
}
