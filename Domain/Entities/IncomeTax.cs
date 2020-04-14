using TaxCalculator.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace TaxCalculator.Domain.Entities
{
    public class IncomeTax
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid doubleNumber")]
        public double IncomeAmount { get; set; }

        [Required]
        public string PostalCode { get; set; }

        public double TaxAmount { get; set; }
        public string TaxCalculationType { get; set; }

        public DateTime? CreatedDate { get; set; }

    }
}
