using QuickReach.ECommerce.Domain.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Customer : EntityBase
{
    public Customer()
    {
        Carts = new List<Cart>();
    }
    [Required]
    public string CardNumber { get; set; }
    [Required]
    public string SecurityNumber { get; set; }
    [Required]
    [RegularExpression(@"(0[1-9]|1[0-2])\/[0-9]{2}", ErrorMessage = "Expiration should match a valid MM/YY value")]
    public string Expiration { get; set; }
    [Required]
    public string CardHolderName { get; set; }
    public int CardType { get; set; }
    [Required]
    public string Street { get; set; }
    [Required]
    public string City { get; set; }
    [Required]
    public string State { get; set; }
    [Required]
    public string Country { get; set; }
    [Required]
    public string ZipCode { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }

    public IEnumerable<Cart> Carts { get; set; }
}