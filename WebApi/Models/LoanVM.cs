namespace WebApi.Models
{
    public class LoanVM
    {
        
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPanNo { get; set; }
        public string CustomerGSTNo { get; set; }

        public string LoanNumber { get; set; }
        public string LoanStatus { get; set; }
        public int LoanType { get; set; }
        public int LoanTerm { get; set; }
        public decimal Amount { get; set; }
        public decimal RateOfinterst { get; set; }
    }
}
