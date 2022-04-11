using QLNhaHang.API.Attribute;
using QLNhaHang.API.Enums;
using System;
using System.Collections.Generic;

namespace QLNhaHang.API.Entities
{
    public partial class Order
    {
        public Order()
        {
            InvoiceDetails = new HashSet<InvoiceDetail>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string? OrderId { get; set; }
        [NotEmpty]
        [PropertyName("Số bàn")]
        public int? TableNumber { get; set; }
        public decimal? TotalPrice { get; set; }
        public Status? Status { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdateTime { get; set; }

        public virtual ICollection<InvoiceDetail>? InvoiceDetails { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }

        public string? StatusName
        {
            get
            {
                switch (Status)
                {
                    case Enums.Status.Waiting:
                        return Resource.QLNhaHangResource.StatusWaiting;
                    case Enums.Status.Processing:
                        return Resource.QLNhaHangResource.StatusProcessing;
                    case Enums.Status.Completed:
                        return Resource.QLNhaHangResource.StatusComplete;
                    case Enums.Status.Cancel:
                        return Resource.QLNhaHangResource.StatusCancel;
                    case Enums.Status.Paid:
                        return Resource.QLNhaHangResource.StatusPaid;
                    default: return null;
                }
            }
        }
    }
}
