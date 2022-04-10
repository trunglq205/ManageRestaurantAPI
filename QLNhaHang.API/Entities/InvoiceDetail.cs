using System;
using System.Collections.Generic;

namespace QLNhaHang.API.Entities
{
    public partial class InvoiceDetail
    {
        public string InvoiceDetailId { get; set; } = null!;
        public string InvoiceId { get; set; } = null!;
        public string OrderId { get; set; } = null!;

        public virtual Invoice InvoiceDetailNavigation { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;
    }
}
