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

        public Patient InsertPatient(NpgsqlConnection conn)
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
                NpgsqlCommand command = new NpgsqlCommand($"INSERT INTO Patient(nom ,age) VALUES (@nom ,@age) RETURNING id , nom , age", con);
                command.Parameters.AddWithValue("@nom", this.Nom);
                command.Parameters.AddWithValue("@age", this.Age);
                
                //command.ExecuteNonQuery();

                NpgsqlDataReader reader = command.ExecuteReader();
                Patient results = null;
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string nom = reader.GetString(1);
                    int age = reader.GetInt32(2);
                    results = new Patient(id, nom , age);
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

        
        //raha parfait
        public List<Maladie> getMaladieOfDiag()
        {
            NpgsqlConnection con = null;

            try
            {
                con = Connection.ConnectPostgres();
                con.Open();

                TrancheAge trancheAge = TrancheAge.findTrancheOfAnAge(con, this.Age);

                List<Maladie> results = new List<Maladie>();

                List<Maladie> maladieInTranches = Maladie.getAllMaladieCorrespondantInTrancheAgeByPatient(con, this, trancheAge);
                foreach (Maladie maladieInTranche in maladieInTranches)
                {
                    int counter = 0;
                    List<MaladieParametre> maladieParametresInThisMaladies = MaladieParametre.getParametreMaladieInMyTrancheAgeByIdMaladie(con, maladieInTranche.Id, trancheAge.Id);
                    foreach (MaladieParametre maladieParametresInThisMaladie in maladieParametresInThisMaladies)
                    {
                        List<PatientParametre> patientParametres = PatientParametre.getPatientParametreByIdPatient(con, this.Id);
                        foreach (PatientParametre patientParametre in patientParametres)
                        {
                            if (maladieParametresInThisMaladie.IdParametre == patientParametre.IdParametre)
                            {
                                if (patientParametre.Niveau > maladieParametresInThisMaladie.NiveauMin && patientParametre.Niveau < maladieParametresInThisMaladie.NiveauMax)
                                {
                                    counter++;
                                }
                            }
                            
                        }
                    }
                    Console.WriteLine("counter"+ counter);
                    if (counter == maladieParametresInThisMaladies.Count)
                    {
                        results.Add(maladieInTranche);
                    }
                }
                return results;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " passage getMaladieOfDiag");
            }
            finally
            {
                con.Close();
            }
        }


        //raha saisina daholo (tsy parfait)
        public Dictionary<Maladie , double> getMaladieOfDiagNonParfait()
        {
            NpgsqlConnection con = null;

            try
            {
                con = Connection.ConnectPostgres();
                con.Open();

                TrancheAge trancheAge = TrancheAge.findTrancheOfAnAge(con, this.Age);

                Dictionary<Maladie, double> results = new Dictionary<Maladie, double>();

                List<Maladie> maladieInTranches = Maladie.getAllMaladieCorrespondantInTrancheAgeByPatient(con, this ,trancheAge);
                foreach (Maladie maladieInTranche in maladieInTranches)
                {
                    int counter = 0;
                    List<MaladieParametre> maladieParametresInThisMaladies = MaladieParametre.getParametreMaladieInMyTrancheAgeByIdMaladie(con, maladieInTranche.Id , trancheAge.Id);
                    foreach (MaladieParametre maladieParametresInThisMaladie in maladieParametresInThisMaladies)
                    {
                        List<PatientParametre> patientParametres = PatientParametre.getPatientParametreByIdPatient(con, this.Id);
                        foreach (PatientParametre patientParametre in patientParametres)
                        {
                            if (maladieParametresInThisMaladie.IdParametre == patientParametre.IdParametre)
                            {
                                if (patientParametre.Niveau > maladieParametresInThisMaladie.NiveauMin && patientParametre.Niveau < maladieParametresInThisMaladie.NiveauMax)
                                {
                                    counter++;
                                }
                            }

                        }
                    }
                    double pourcentage = (counter  *100) / maladieParametresInThisMaladies.Count();
                    results.Add(maladieInTranche, pourcentage);
                }
                return results;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " passage getMaladieOfDiagNonParfait");
            }
            finally
            {
                con.Close();
            }
        }


        public List<MedicamentDiag> getMedicamentsDiag() {
            NpgsqlConnection con = null;

            try
            {
                con = Connection.ConnectPostgres();
                con.Open();

                List<MedicamentDiag> result = new List<MedicamentDiag>();

                List<PatientParametre> patientParametres = PatientParametre.getPatientParametreByIdPatient(con, this.Id);
                for (int i = 0; i < patientParametres.Count; i++) {
                    if (patientParametres[i].Niveau != 0)
                    {
                        
                    }

                }

                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " passage getMedicamentsDiag");
            }
            finally
            {
                con.Close();
            }
        }
    
    }
}
