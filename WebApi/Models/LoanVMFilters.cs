namespace WebApi.Models
{
    public class LoanVMFilters
    {
        public string LoanNumber { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public int PageSize { get; set; } = 5;
        public int PageNo { get; set; } = 1;
    }
}
