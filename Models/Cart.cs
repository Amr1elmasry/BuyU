using MessagePack;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuyU.Models
{
    public partial class Cart
    {

        [Key("CartId")]
        public int CartId { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public ICollection<Product>? Products { get; set; }
        public List<CartProduct>? CartProduct { get; set; }
    }
}
