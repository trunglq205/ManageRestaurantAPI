using QLNhaHang.API.Attribute;
using QLNhaHang.API.Enums;
using System;
using System.Collections.Generic;

namespace QLNhaHang.API.Entities
{
    public partial class Invoice
    {
        public Invoice()
        {
            InvoiceDetails = new HashSet<InvoiceDetail>();
        }
        public string? InvoiceId { get; set; } = null!;
        [NotEmpty]
        [PropertyName("Người lập")]
        public string? UserId { get; set; }
        public decimal? TotalPrice { get; set; }
        public Status? Status { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }

        public virtual Account? User { get; set; }
        public virtual ICollection<InvoiceDetail>? InvoiceDetails { get; set; }
    }
}
