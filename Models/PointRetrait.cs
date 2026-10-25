namespace PaniersFermiers.Models
{
    public class PointRetrait
    {
        public int Id { get; set; }
        public string Nom { get; set; } = "";
        public string Detail { get; set; } = "";     
        public string Icone { get; set; } = "";
        public string Jour { get; set; } = "";      
        public string Horaires { get; set; } = "";   // "16h30 - 19h00"
        public string Periode { get; set; } = "";    // "Jeudi après-midi"
        public List<Creneau> Creneaux { get; set; } = new List<Creneau>();

        public string JourHoraires
        {
            get { return Jour + " " + Horaires; }
        }
    }
}