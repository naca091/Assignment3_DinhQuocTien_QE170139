using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models
{
    public partial class OrderDetail
    {
        [Key]
        public int OrderId { get; set; }
        [Key]
        public int ProductId { get; set; }

        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
