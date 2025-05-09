namespace Covauto.Domain.Entities
{
    public class LeenAutoRit
    {
        public int Id { get; set; }
        public string VanAdres { get; set; }
        public string NaarAdres { get; set; }
        public DateTime VertrekTijd { get; set; }
        public DateTime AankomstTijd { get; set; }
        public int KilometerstandBegin { get; set; }
        public int KilometerstandEind { get; set; }
        public int AutoId { get; set; }
        public string WerknemerId { get; set; }
        public Auto Auto { get; set; }
        public AppUser Werknemer { get; set; }
    }
}
