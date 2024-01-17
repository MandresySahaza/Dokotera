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





    }
}
