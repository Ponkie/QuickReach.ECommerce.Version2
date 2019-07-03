using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuickReach.ECommerce.Domain.Models
{
    [Table("CartItem")]
    public class CartItem :IValidatableObject
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public decimal OldUnitPrice { get; set; }
        [Required]
        public int Quantity { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
