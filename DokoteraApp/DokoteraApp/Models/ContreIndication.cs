using Npgsql;

namespace DokoteraApp.Models
{
    public class ContreIndication
    {
        public int Id { get; set; }
        public int IdMedicament { get; set; }
        public int IdParametre { get; set; }
        public double ApportNegatif { get; set; }

        public ContreIndication(int id, int idMedicament, int idParametre, double apportNegatif)
        {
            Id = id;
            IdMedicament = idMedicament;
            IdParametre = idParametre;
            ApportNegatif = apportNegatif;
        }

        public static List<ContreIndication> getContreIndicationByIdMedicament(NpgsqlConnection conn, int idMedicament)
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

                string query = "select * from ContreIndication where idMedicament = @idMedicament";
                NpgsqlCommand command = new NpgsqlCommand(query, con);
                command.Parameters.AddWithValue("@idMedicament", idMedicament);
                NpgsqlDataReader reader = command.ExecuteReader();

                List<ContreIndication> results = new List<ContreIndication>();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    int idmedicament = reader.GetInt32(1);
                    int idparametre = reader.GetInt32(2);
                    double apportneg = reader.GetDouble(3);

                    ContreIndication rep = new ContreIndication(id, idmedicament, idparametre, apportneg);

                    results.Add(rep);
                }
                reader.Close();

                return results;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " passage getContreIndicationByIdMedicament");
            }
            finally
            {
                if (isOpen == true) { con.Close(); }
            }
        }


    }
}
