using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace QuickReach.ECommerce.Domain.Models
{
    [Table("Product")]
    public class Product : EntityBase
    {
        public Product()
        {
            this.ProductCategories = new List<ProductCategory>();
        }
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
        public IEnumerable<ProductSupplier> ProductSuppliers { get; set; }
        public IEnumerable<ProductManufacturer> ProductManufacturers { get; set; }

        public void AddCategory(ProductCategory child)
        {
            ((ICollection<ProductCategory>)this.ProductCategories).Add(child);
        }

        public ProductCategory GetCategory(int categoryId)
        {
            return ((ICollection<ProductCategory>)this.ProductCategories)
                    .FirstOrDefault(pc => pc.ProductId == this.ID &&
                               pc.CategoryId == categoryId);
        }

        public void RemoveCategory(int categoryId)
        {
            var child = this.GetCategory(categoryId);

            ((ICollection<ProductCategory>)this.ProductCategories).Remove(child);
        }
    }
}
