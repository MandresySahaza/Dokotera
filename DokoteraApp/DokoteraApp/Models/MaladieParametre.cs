namespace DokoteraApp.Models
{
    public class MaladieParametre
    {
        public int Id { get; set; }
        public int IdMaladie { get; set; }
        public int IdParametre { get; set; }
        public double NiveauMin { get; set; }
        public double NiveauMax { get; set; }
        public int TrancheAge { get; set; }

        public MaladieParametre(int id, int idMaladie, int idParametre, double niveauMin, double niveauMax, int trancheAge)
        {
            Id = id;
            IdMaladie = idMaladie;
            IdParametre = idParametre;
            NiveauMin = niveauMin;
            NiveauMax = niveauMax;
            TrancheAge = trancheAge;
        }

    }

}
