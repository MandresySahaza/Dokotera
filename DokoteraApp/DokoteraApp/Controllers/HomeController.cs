using DokoteraApp.Models;
using Microsoft.AspNetCore.Mvc;
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
            //Patient patient = new Patient(0, "Mandresy", 20);
            //Console.WriteLine("patient : " + patient.InsertPatient(null));

            //PatientParametre patientParametre = new PatientParametre(0, 1, 1, 12);
            //Console.WriteLine("patientParams : " + patientParametre.InsertPatientParametre(null));

            //TrancheAge trancheCorrespondant = TrancheAge.findTrancheOfAnAge(null, 12);
            //Console.WriteLine(trancheCorrespondant.Description);


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