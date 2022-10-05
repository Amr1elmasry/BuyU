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
        public string UserId { get; set; }

        [DefaultValue(1)]
        public int  Qty { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } 
        public ICollection<Product> Products { get; set; }
    }
}
