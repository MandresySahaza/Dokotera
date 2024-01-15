namespace DokoteraApp.Models
{
    public class Maladie
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        public Maladie(int id, string nom)
        {
            Id = id;
            Nom = nom;
        }


    }
}
