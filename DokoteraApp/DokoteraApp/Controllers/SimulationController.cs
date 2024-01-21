using DokoteraApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Npgsql;
using System.Reflection.Metadata;

namespace DokoteraApp.Controllers
{
    public class SimulationController : Controller
    {
        public IActionResult Index()
        {
            List<Parametre> listeparametres = Parametre.getAllParametre(null);

            return View(listeparametres);
        }

        public IActionResult InsertInformationPatient(string nomPatient , int agePatient)
        {
            NpgsqlConnection con = null;
            try
            {
                con = Connection.ConnectPostgres();
                con.Open();

                Patient patient = new Patient(0 , nomPatient , agePatient);
                Patient patientInsert = patient.InsertPatient(con);

           
                List<Parametre> listeparametres = Parametre.getAllParametre(null);
                for (int i = 0; i < listeparametres.Count; i++)
                {
                    double niveau = 0;
                    String niveauString = HttpContext.Request.Form[listeparametres[i].Nom];
                    if (niveauString != null)
                    {
                        niveau = double.Parse(HttpContext.Request.Form[listeparametres[i].Nom]);
                    }
                    
                    PatientParametre patientParametre = new PatientParametre(0, patientInsert.Id, listeparametres[i].Id, niveau);
                    patientParametre.InsertPatientParametre(con);
                }


                string patientJson = JsonConvert.SerializeObject(patientInsert);
                HttpContext.Session.SetString("PatientObject", patientJson);

                return RedirectToAction("Diag", "Simulation");

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index", "Simulation");
            }
            finally
            {
                if (con != null) {
                    con.Close();
                }
                
            }
        }

        public IActionResult Diag()
        {
            try
            {
                string patientJson = HttpContext.Session.GetString("PatientObject");
                Patient storedPatient = JsonConvert.DeserializeObject<Patient>(patientJson);

                Dictionary<Maladie, double> maladiediagsNonParfait = storedPatient.getMaladieOfDiagNonParfait();
                ViewBag.maladiediagsNonParfait = maladiediagsNonParfait;

                List<Maladie> maladiediags = storedPatient.getMaladieOfDiag();
                ViewBag.maladiediags = maladiediags;

                List<MedicamentDiag> repMedicaments = storedPatient.getMedicamentsDiag();
                ViewBag.repMedicaments = repMedicaments;

                return View(storedPatient);

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index", "Simulation");
            }

        }


    }
}
