using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{
    public class LoanHistory
    {
        public int Id { get; set; }
        public string LoanNumber { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal EMIAmount { get; set; }
        public DateTime EMIDueDate { get; set; }
        public DateTime? EMIPaidDate { get; set; }
        public bool PaymentStatus { get; set; }=false;
        public Loan Loan { get; set; }

    }
}
