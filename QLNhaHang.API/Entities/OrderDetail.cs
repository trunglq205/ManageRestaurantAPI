using System;
using System.Collections.Generic;

namespace QLNhaHang.API.Entities
{
    public partial class OrderDetail
    {
        public string? OrderDetailId { get; set; }
        public string? OrderId { get; set; }
        public string? MenuId { get; set; }
        public int? Amount { get; set; }
        public decimal? Price { get; set; }

        public virtual Menu? Menu { get; set; }
        public virtual Order? Order { get; set; }
    }
}
