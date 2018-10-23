using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace HappyShare.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemID { get; set; }
        public string CartID { get; set; }

        public int ProductID { get; set; }

        public SharedItem SharedItem { get; set; }
    }
}
