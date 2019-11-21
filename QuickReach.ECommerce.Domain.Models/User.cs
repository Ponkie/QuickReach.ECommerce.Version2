using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickReach.ECommerce.Domain.Models
{
    [Table("User")]
    public class User : EntityBase
    {
        [Required]
        [MaxLength(255)]
        public string Username { get; set; }
        [Required]
        [MaxLength(255)]
        public string Password{ get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Type { get; set; }
        public bool IsActive { get; set; }
    }
}
