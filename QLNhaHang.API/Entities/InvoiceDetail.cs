using System;
using System.Collections.Generic;

namespace QLNhaHang.API.Entities
{
    public partial class InvoiceDetail
    {
        public string? InvoiceDetailId { get; set; }
        public string? InvoiceId { get; set; }
        public string? OrderId { get; set; }

        public virtual Invoice? Invoice { get; set; }
        public virtual Order? Order { get; set; }
    }
}
