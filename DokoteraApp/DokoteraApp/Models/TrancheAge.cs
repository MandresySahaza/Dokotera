using Npgsql;

namespace DokoteraApp.Models
{
    public class TrancheAge
    {
        public int Id { get; set; }
        public String Description { get; set; }
        public int Min {  get; set; }
        public int Max { get; set; }

        public TrancheAge(int id, string description, int min, int max)
        {
            Id = id;
            Description = description;
            Min = min;
            Max = max;
        }

        public static TrancheAge findTrancheOfAnAge(NpgsqlConnection conn, int age)
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

                string query = "select * from TrancheAge where min <= @age and max >= @age";
                NpgsqlCommand command = new NpgsqlCommand(query, con);
                command.Parameters.AddWithValue("@age", age);
                NpgsqlDataReader reader = command.ExecuteReader();

                TrancheAge results = null;
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    String description = reader.GetString(1);
                    int min = reader.GetInt32(2);
                    int max = reader.GetInt32(3);

                    results = new TrancheAge(id, description , min ,max);
                }
                reader.Close();

                return results;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " passage  findTrancheOfAnAge");
            }
            finally
            {
                if (isOpen == true) { con.Close(); }
            }
        }


    }
}
