namespace Covauto.Shared.DTOs
{
    public class LeenAutoReserveringDTO
    {
        public int Id { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime EindDatum { get; set; }
        public string WerknemerId { get; set; }
        public string WerknemerUserName { get; set; }
        public string WerknemerEmail { get; set; }
        public int AutoId { get; set; }
        public AutoDTO Auto { get; set; }
    }
}
