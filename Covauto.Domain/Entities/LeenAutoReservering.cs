namespace Covauto.Domain.Entities
{
    public class LeenAutoReservering
    {
        public int Id { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime EindDatum { get; set; }
        public string WerknemerId { get; set; }
        public AppUser Werknemer { get; set; }
        public int AutoId { get; set; }
        public Auto Auto { get; set; }
        public bool RitVoltooid { get; set; }
    }
}
