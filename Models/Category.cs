using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BuyU.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        public int CatId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Photo { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
