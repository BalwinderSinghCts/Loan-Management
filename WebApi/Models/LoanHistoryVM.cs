using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Entities;

namespace WebApi.Models
{
    public class LoanHistoryVM
    {
        public int Id { get; set; }
        public string LoanNumber { get; set; }
        public string TranscationId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public string EMIAmount { get; set; }
        public DateTime EMIDueDate { get; set; }
        public DateTime? EMIPaidDate { get; set; }
        public bool PaymentStatus { get; set; } = false;
    }
}
