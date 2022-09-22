using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BuyU.Models
{
    public partial class Brand
    {
        public Brand()
        {
            Products = new HashSet<Product>();
        }

        public int BrandId { get; set; }
        
        [Required , MaxLength(50)]
        public string BrandName { get; set; }
        
        [MaxLength(500)]
        public string? Description { get; set; }
        public string? Photo { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
