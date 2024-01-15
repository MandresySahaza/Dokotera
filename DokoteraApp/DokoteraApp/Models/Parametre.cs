namespace DokoteraApp.Models
{
    public class Parametre
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        public Parametre(int id, string nom)
        {
            Id = id;
            Nom = nom;
        }
    }
}
