using DokoteraApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;

namespace DokoteraApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Patient patient = new Patient(1, "mandresy", 12);
            //Console.WriteLine("patient : " + patient.InsertPatient(null).Age);

            //PatientParametre patientParametre = new PatientParametre(0, 1, 1, 12);
            //Console.WriteLine("patientParams : " + patientParametre.InsertPatientParametre(null));

            //TrancheAge trancheCorrespondant = TrancheAge.findTrancheOfAnAge(null, 12);
            //Console.WriteLine(trancheCorrespondant.Description);

            //List<Maladie> maladies = Maladie.getAllMaladieCorrespondantInTrancheAgeByPatient(null , patient);
            //foreach (Maladie maladie in maladies)
            //{
            //    Console.WriteLine(maladie.Nom);
            //}

            //List<MaladieParametre> maladieParametres = MaladieParametre.getParametreMaladieByIdMaladie(null, 2);
            //foreach (MaladieParametre maladieParametre in maladieParametres)
            //{
            //Console.WriteLine(maladieParametre.IdParametre);
            //}

            //List<PatientParametre> patientParametres = PatientParametre.getPatientParametreByIdPatient(null , patient.Id);
            //foreach (PatientParametre patientParametre in patientParametres)
            //{
            //    Console.WriteLine(patientParametre.IdParametre +" " + patientParametre.Niveau);
            //}

            //-----------------------getMaladie Parfait
            //List<Maladie> maladiediags = patient.getMaladieOfDiag();
            //foreach (Maladie maladiediag in maladiediags) {
            //    Console.WriteLine(maladiediag.Nom);
            //}

            //-----------------------getMaladie tsy Parfait
            //Dictionary<Maladie, double> maladiediags = patient.getMaladieOfDiagNonParfait();
            //foreach ((Maladie cle , double valeur) in maladiediags) {
            //    Console.WriteLine(cle.Nom + " " + valeur);
            //}

            //List<MedicamentParametre> medicamentParametres = MedicamentParametre.getMedicamentParametreByIdParametre(null, 1, 18);
            //Console.WriteLine(medicamentParametres.Count());


            //List<ContreIndication> medicamentParametres = ContreIndication.getContreIndicationByIdMedicament(null, 1);
            //for (int i = 0; i < medicamentParametres.Count; i++)
            //{
            //    Console.WriteLine(medicamentParametres[i].IdParametre);
            //}


            //List<Parametre> listeParams = MedicamentParametre.getParametreSitranaMedicament(null , 2);
            //for (int i = 0; i < listeParams.Count(); i++)
            //{
            //    Console.WriteLine(listeParams[i].Nom);
            //}



            List<MedicamentDiag> repMedicament = patient.getMedicamentsDiag();
            for (int i = 0; i < repMedicament.Count(); i++)
            {
                Console.WriteLine(repMedicament[i].Nom + " " + repMedicament[i].Prix + " " + repMedicament[i].Quantite);
            }


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}