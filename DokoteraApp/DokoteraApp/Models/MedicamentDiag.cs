namespace DokoteraApp.Models
{
    public class MedicamentDiag
    {
        public int id { get; set; }
        public string nom { get; set; }
        public double quantite { get; set; }
        public double prix {  get; set; }

        public MedicamentDiag(int id, string nom, double quantite, double prix)
        {
            this.id = id;
            this.nom = nom;
            this.quantite = quantite;
            this.prix = prix;
        }

    }
}
