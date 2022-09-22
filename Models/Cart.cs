using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuyU.Models
{
    public partial class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? ProductId { get; set; }
        public int? Qty { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; } 
    }
}
