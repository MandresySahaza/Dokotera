namespace DokoteraApp.Models
{
    public class MedicamentDiag
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public double Quantite { get; set; }
        public double Prix {  get; set; }

        public MedicamentDiag(int id, string nom, double quantite, double prix)
        {
            Id = id;
            Nom = nom;
            Quantite = quantite;
            Prix = prix;
        }

    }
}
