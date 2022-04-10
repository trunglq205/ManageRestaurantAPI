using QLNhaHang.API.Entities;

namespace QLNhaHang.API.Interfaces
{
    public interface IMenuService
    {
        public IEnumerable<Menu> Get();


        public Menu GetById(string menuId);

        public Menu Insert(Menu menu);

        
        public Menu Update(string menuId, Menu menu);

        
        public void Delete(string menuId);
    }
}
