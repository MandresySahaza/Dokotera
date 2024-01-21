using Npgsql;
using System.IO;

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


        //tsy misy contre -indication
        /*public List<MedicamentDiag> getMedicamentsDiag() {
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
                        if (patientParametres[i].Niveau < 0)
                        {
                            patientParametres[i].Niveau = patientParametres[i].Niveau * (-1);
                        }

                        List<MedicamentParametre> medicamentParametresIinthisParametre = MedicamentParametre.getMedicamentParametreByIdParametre(con, patientParametres[i].IdParametre, this.Age);
                        List<MedicamentDiag> medicamentTestenaIzayMora = new List<MedicamentDiag>();
                        MedicamentDiag medicamentMora = null;

                        for (int j = 0; j < medicamentParametresIinthisParametre.Count(); j++)
                        {
                            double niveaupatientParametres = patientParametres[i].Niveau;
                                              
                            double apportMedicament = medicamentParametresIinthisParametre[j].Apport;
                            int count = 0;
                            while (niveaupatientParametres != 0)
                            {
                                count++;
                                niveaupatientParametres = niveaupatientParametres - apportMedicament;
                                if (niveaupatientParametres < 0)
                                {
                                    niveaupatientParametres = 0;
                                }
                            }

                            Medicament newmedoc = Medicament.getMedicamentById(con , medicamentParametresIinthisParametre[j].IdMedicament);
                            MedicamentDiag newMedicament = new MedicamentDiag(newmedoc.Id , newmedoc.Nom , count ,newmedoc.Prix*count);
                            medicamentTestenaIzayMora.Add(newMedicament);
                        }
                        if (medicamentTestenaIzayMora.Count != 0)
                        {
                            medicamentMora = medicamentTestenaIzayMora.OrderBy(m => m.Prix).FirstOrDefault();
                            result.Add(medicamentMora);

                            patientParametres[i].Niveau = 0;
                            
                            //manala izay mbola sitran'ilay medicament
                            List<MedicamentParametre> parametreSitranyMedicamentAmzao = MedicamentParametre.getParametreSitranaMedicament(con, medicamentMora.Id);
                            List<PatientParametre> patientParametresVituelle = PatientParametre.getPatientParametreByIdPatient(con, this.Id);
                            for (int j = 0; j < parametreSitranyMedicamentAmzao.Count; j++)
                            {
                                for (int k = 0; k < patientParametresVituelle.Count; k++)
                                {
                                    if (parametreSitranyMedicamentAmzao[j].IdParametre == patientParametres[k].IdParametre && patientParametres[k].Niveau!= 0)
                                    {
                                        if (patientParametres[k].Niveau < 0)
                                        {
                                            patientParametres[k].Niveau = patientParametres[k].Niveau * (-1);
                                        }
                                        patientParametres[k].Niveau = patientParametres[k].Niveau - (parametreSitranyMedicamentAmzao[j].Apport * medicamentMora.Quantite);
                                        if (patientParametres[k].Niveau < 0)
                                        {
                                            patientParametres[k].Niveau = 0;
                                        }
                                    }

                                }
                            }
                        }
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
        }*/


        public List<MedicamentDiag> getMedicamentsDiag()
        {
            NpgsqlConnection con = null;
            try
            {
                con = Connection.ConnectPostgres();
                con.Open();

                List<MedicamentDiag> result = new List<MedicamentDiag>();

                List<PatientParametre> patientParametres = PatientParametre.getPatientParametreByIdPatient(con, this.Id);
                for (int i = 0; i < patientParametres.Count; i++)
                {
                    if (patientParametres[i].Niveau != 0)
                    {
                        if (patientParametres[i].Niveau < 0)
                        {
                            patientParametres[i].Niveau = patientParametres[i].Niveau * (-1);
                        }

                        List<MedicamentParametre> medicamentParametresIinthisParametre = MedicamentParametre.getMedicamentParametreByIdParametre(con, patientParametres[i].IdParametre, this.Age);
                        List<MedicamentDiag> medicamentTestenaIzayMora = new List<MedicamentDiag>();
                        MedicamentDiag medicamentMora = null;

                        for (int j = 0; j < medicamentParametresIinthisParametre.Count(); j++)
                        {
                            double niveaupatientParametres = patientParametres[i].Niveau;

                            double apportMedicament = medicamentParametresIinthisParametre[j].Apport;
                            int count = 0;
                            while (niveaupatientParametres != 0)
                            {
                                count++;
                                niveaupatientParametres = niveaupatientParametres - apportMedicament;
                                if (niveaupatientParametres < 0)
                                {
                                    niveaupatientParametres = 0;
                                }
                            }

                            Medicament newmedoc = Medicament.getMedicamentById(con, medicamentParametresIinthisParametre[j].IdMedicament);
                            MedicamentDiag newMedicament = new MedicamentDiag(newmedoc.Id, newmedoc.Nom, count, newmedoc.Prix * count);
                            medicamentTestenaIzayMora.Add(newMedicament);
                        }
                        if (medicamentTestenaIzayMora.Count != 0)
                        {
                            medicamentMora = medicamentTestenaIzayMora.OrderBy(m => m.Prix).FirstOrDefault();
                            result.Add(medicamentMora);
                            patientParametres[i].Niveau = 0;

                            List<PatientParametre> patientParametresVituelle = PatientParametre.getPatientParametreByIdPatient(con, this.Id);


                            //contreIndication
                            List<ContreIndication> contreIndicationsMedicament = ContreIndication.getContreIndicationByIdMedicament(con, medicamentMora.Id);

                            for (int x = 0; x < contreIndicationsMedicament.Count; x++)
                            {
                                int count = 0;
                                for (int y = 0; y < patientParametresVituelle.Count; y++)
                                {
                                    if (contreIndicationsMedicament[x].IdParametre == patientParametres[y].IdParametre)
                                    {
                                        count++;
                                        patientParametres[y].Niveau = patientParametres[y].Niveau + contreIndicationsMedicament[x].ApportNegatif;
                                    }
                                }
                                if (count == 0)
                                {
                                    PatientParametre nouveauPatientParametre = new PatientParametre(patientParametres.Count + 1, this.Id, contreIndicationsMedicament[x].IdParametre, contreIndicationsMedicament[x].ApportNegatif * medicamentMora.Quantite);
                                    patientParametres.Add(nouveauPatientParametre);
                                }

                                i = 0;
                            }

                            //manala izay mbola sitran'ilay medicament
                            List<MedicamentParametre> parametreSitranyMedicamentAmzao = MedicamentParametre.getParametreSitranaMedicament(con, medicamentMora.Id);
                            for (int j = 0; j < parametreSitranyMedicamentAmzao.Count; j++)
                            {
                                for (int k = 0; k < patientParametresVituelle.Count; k++)
                                {
                                    if (parametreSitranyMedicamentAmzao[j].IdParametre == patientParametres[k].IdParametre && patientParametres[k].Niveau != 0)
                                    {
                                        if (patientParametres[k].Niveau < 0)
                                        {
                                            patientParametres[k].Niveau = patientParametres[k].Niveau * (-1);
                                        }
                                        patientParametres[k].Niveau = patientParametres[k].Niveau - (parametreSitranyMedicamentAmzao[j].Apport * medicamentMora.Quantite);
                                        if (patientParametres[k].Niveau < 0)
                                        {
                                            patientParametres[k].Niveau = 0;
                                        }
                                    }

                                }
                            }
                        }
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
