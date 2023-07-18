namespace WebApi.Models
{
    public class Reservesations
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
