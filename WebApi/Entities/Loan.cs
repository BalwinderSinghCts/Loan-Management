using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{
    public class Loan
    {
        public int Id { get; set; }
        public string LoanNumber { get; set; }//000001,000002,000003,000012,0000013
        [Column(TypeName = "decimal(18,2)")]

        public decimal Amount { get; set; }
        public int LoanTerm { get; set; }
        public string Status { get; set; } = "Pending";
        public int LoanType { get; set; }
        public string LoanTypeName { get; set; }
        public DateTime OriginationDate { get; set; }
        public string OriginationAccount { get; set; }
        [Column(TypeName = "decimal(18,2)")]

        public decimal RateOfinterst { get; set; }
        public bool Active { get; set; }
        public int PendingEMI { get; set; }
        [Column(TypeName = "decimal(18,2)")]

        public decimal ProcessingFee { get; set; }
        [Column(TypeName = "decimal(18,2)")]

        public decimal GSTAmount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        public int CustomerId { get; set; }
        public ICollection<LoanDocument> Documents { get; set; }
        public ICollection<LoanHistory> LoanHistory { get; set; }
        public Customer Customer { get; set; }
    }
}
