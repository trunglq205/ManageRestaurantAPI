using QLNhaHang.API.Attribute;
using System;
using System.Collections.Generic;

namespace QLNhaHang.API.Entities
{
    public partial class Category
    {
        public Category()
        {
            Menus = new HashSet<Menu>();
        }

        public string? CategoryId { get; set; } = null!;
        [NotEmpty]
        [PropertyName("Tên danh mục")]
        public string? CategoryName { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }

        public virtual ICollection<Menu>? Menus { get; set; }
    }
}
