namespace Covauto.Shared.DTOs
{
    public class LeenAutoRitDTO
    {
        public int WerknemerId { get; set; }
        public int AutoId { get; set; }
        public string VanAdres { get; set; }
        public string NaarAdres { get; set; }
        public DateTime VertrekTijd { get; set; }
        public DateTime AankomstTijd { get; set; }
        public int KilometerstandBegin { get; set; }
        public int KilometerstandEind { get; set; }
    }
}