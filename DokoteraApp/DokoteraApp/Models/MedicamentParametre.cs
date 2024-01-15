namespace DokoteraApp.Models
{
    public class MedicamentParametre
    {
        public int Id { get; set; }
        public int IdMedicament { get; set; }
        public int IdParametre { get; set; }
        public double Apport { get; set; }

        public MedicamentParametre(int id, int idMedicament, int idParametre, double apport)
        {
            Id = id;
            IdMedicament = idMedicament;
            IdParametre = idParametre;
            Apport = apport;
        }
    }
}
