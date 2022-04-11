using QLNhaHang.API.Entities;
using QLNhaHang.API.Helpers;

namespace QLNhaHang.API.Interfaces
{
    public interface IMenuService
    {
        public IEnumerable<Menu> Get();


        public Menu GetById(string menuId);

        public PageResult<Menu> GetPaging(string? keySearch, Pagination? pagination = null);
        public Menu Insert(Menu menu);

        
        public Menu Update(string menuId, Menu menu);

        
        public void Delete(string menuId);
    }
}
