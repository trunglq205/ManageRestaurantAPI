using QLNhaHang.API.Attribute;
using QLNhaHang.API.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QLNhaHang.API.Entities
{
    public partial class Account
    {
        public Account()
        {
            Invoices = new HashSet<Invoice>();
        }

        public string? UserId { get; set; }
        [NotEmpty]
        [PropertyName("Tên tài khoản")]
        public string? UserName { get; set; }
        [NotEmpty]
        [PropertyName("Mật khẩu")]
        public string? Password { get; set; }
        [NotEmpty]
        [PropertyName("Họ tên")]
        public string? FullName { get; set; }
        [NotEmpty]
        [PropertyName("Chức vụ")]
        public int? Position { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public Status? Status { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }

        public virtual ICollection<Invoice>? Invoices { get; set; }


        public string? StatusName
        {
            get
            {
                switch (Status)
                {
                    case Enums.Status.Active:
                        return Resource.QLNhaHangResource.StatusActive;
                    case Enums.Status.Disable:
                        return Resource.QLNhaHangResource.StatusDisable;
                    default: return null;
                }
            }
        }
    }
}
