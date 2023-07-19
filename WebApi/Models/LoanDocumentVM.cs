using WebApi.Entities;

namespace WebApi.Models
{
    public class LoanDocumentVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int LoanNumber { get; set; }
    }
}
