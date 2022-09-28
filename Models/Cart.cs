using BuyU.Data;
using MessagePack;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuyU.Models
{
    public partial class Cart
    {

        [Key("CartId")]
        public int CartId { get; set; }
        public string UserId { get; set; }
        public int? ProductId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } 
    }
}
