namespace WebApi.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool Active { get; set; }
        public string? PanNo { get; set; }
        public string? GSTNo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdtateDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdtateBy { get; set; }
        public ICollection<Loan> Loans { get; set; }


    }
}
