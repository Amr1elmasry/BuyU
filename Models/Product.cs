using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuyU.Models
{
    public partial class Product
    {
        public int ProductId { get; set; }

        [Required, MaxLength(50)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
        [Required]
        public int? Price { get; set; }
        public string? Photo { get; set; }
        public string? Color { get; set; }

        [Required , Display(Name ="BrandName") ]
        public int BrandId { get; set; }
        public int? Quantity { get; set; }
        public int? DiscountId { get; set; }

        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; } 
    }
}
