namespace QLNhaHang.API.Helpers
{
    public class PageResult<T>
    {
        public Pagination Pagination { get; set; }
        public IEnumerable<T> Data { get; set; }
        public PageResult() { }
        public PageResult(Pagination pagination, IEnumerable<T> data)
        {
            Pagination = pagination;
            Data = data;
        }
        public static IQueryable<T> ToPageResult(Pagination pagination, IQueryable<T> query)
        {
            pagination.PageNumber = pagination.PageNumber < 1? 1 : pagination.PageNumber;
            query = query.Skip(pagination.PageSize*(pagination.PageNumber-1))
                         .Take(pagination.PageSize)
                         .AsQueryable();
            return query;
        }
    }
}
