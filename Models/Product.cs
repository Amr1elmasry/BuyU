using System;
using System.Collections.Generic;

namespace BuyU.Models
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Price { get; set; }
        public string? Photo { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public string? Season { get; set; }
        public int CatId { get; set; }
        public int? Quantity { get; set; }
        public int? SellerId { get; set; }
        public int? DiscountId { get; set; }

        public virtual Category Cat { get; set; } = null!;
    }
}
