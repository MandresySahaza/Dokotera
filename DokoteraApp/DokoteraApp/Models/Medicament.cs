﻿using Npgsql;

namespace DokoteraApp.Models
{
    public class Medicament
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public int AgeMin { get; set; }
        public int AgeMax { get; set; }
        public double Prix { get; set; }

        public Medicament(int id, string nom, int ageMin, int ageMax, double prix)
        {
            Id = id;
            Nom = nom;
            AgeMin = ageMin;
            AgeMax = ageMax;
            Prix = prix;
        }

        public static Medicament getMedicamentById(NpgsqlConnection conn, int idMedicament)
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

                string query = "select * From Medicament where id = @idMedicament";
                NpgsqlCommand command = new NpgsqlCommand(query, con);
                command.Parameters.AddWithValue("@idMedicament", idMedicament);
                NpgsqlDataReader reader = command.ExecuteReader();

                Medicament results = null;

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string nom = reader.GetString(1);
                    int ageMin = reader.GetInt32(2);
                    int ageMax = reader.GetInt32(3);
                    double prix = reader.GetDouble(4);

                    results = new Medicament(id, nom, ageMin, ageMax, prix);
                }
                reader.Close();

                return results;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " passage getMedicament");
            }
            finally
            {
                if (isOpen == true) { con.Close(); }
            }
        }




    }
}
