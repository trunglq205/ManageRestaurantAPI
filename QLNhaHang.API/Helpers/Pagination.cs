namespace QLNhaHang.API.Helpers
{
    public class Pagination
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalCount { get; set; }
        public int TotalPage
        {
            get
            {
                if (this.PageSize == 0) return 0;
                var total = this.TotalCount / this.PageSize;
                if (this.TotalCount % this.PageSize > 0) total += 1;
                return total;
            }
        }
        public Pagination()
        {
            PageNumber = 1;
            PageSize = 1;
        }

    }
}
