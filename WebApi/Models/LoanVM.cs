using WebApi.Entities;

namespace WebApi.Models
{
    public class LoanVM
    {
        public LoanVM()
        {
            this.LoanHistory = new List<LoanHistoryVM>();
        }
        public int LoanId { get; set; }
        public int CustomerId { get; set; }
        public int CustomerEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPanNo { get; set; }
        public string CustomerGSTNo { get; set; }
        public string LoanNumber { get; set; }
        public string LoanStatus { get; set; }
        public int LoanType { get; set; }
        public string LoanTypeName { get; set; }
        public int LoanTerm { get; set; }
        public decimal Amount { get; set; }
        public decimal RateOfinterst { get; set; }
        public int UserId { get; set; }
        public List<LoanHistoryVM> LoanHistory { get; set; }
        public List<LoanDocumentVM> LoanDocuments { get; set; }
    }
}
