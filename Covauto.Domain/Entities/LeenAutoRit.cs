namespace Covauto.Domain.Entities
{
    public class LeenAutoRit
    {
        public int Id { get; set; }
        public int WerknemerId { get; set; }
        public string VanAdres { get; set; }
        public string NaarAdres { get; set; }
        public DateTime VertrekTijd { get; set; }
        public int KilometerstandBegin { get; set; }
        public int KilometerstandEind { get; set; }
    }
}