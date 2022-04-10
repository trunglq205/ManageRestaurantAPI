using System;
using System.Collections.Generic;

namespace QLNhaHang.API.Entities
{
    public partial class Invoice
    {
        public string InvoiceId { get; set; } = null!;
        public string? UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public int Status { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }

        public virtual Account? User { get; set; }
        public virtual InvoiceDetail InvoiceDetail { get; set; } = null!;
    }
}
