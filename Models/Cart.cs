using MessagePack;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuyU.Models
{
    public partial class Cart
    {


        public int CartId { get; set; }
        public int UserId { get; set; }
        public int? ProductId { get; set; }

        [Required]
        public int? Qty { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; } 
    }
}
