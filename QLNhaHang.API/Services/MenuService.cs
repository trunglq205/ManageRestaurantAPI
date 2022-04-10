using QLNhaHang.API.Entities;
using QLNhaHang.API.Exceptions;
using QLNhaHang.API.Interfaces;
using QLNhaHang.API.Utils;

namespace QLNhaHang.API.Services
{
    public class MenuService : IMenuService
    {
        private readonly QLNhaHangContext dbContext;
        public MenuService()
        {
            dbContext = new QLNhaHangContext();
        }
        

        public IEnumerable<Menu> Get()
        {
            return dbContext.Menus.ToList();
        }

        public Menu GetById(string menuId)
        {
            var menu = dbContext.Menus.Find(menuId);
            if(menu == null)
            {
                throw new QLNhaHangException(String.Format(Resource.QLNhaHangResource.MenuNotFound));
            }
            else
            {
                return menu;
            }
        }

        public Menu Insert(Menu menu)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                EntityUtils<Menu>.ValidateData(menu);
                menu.MenuId = Guid.NewGuid().ToString();
                menu.CreatedTime = DateTime.Now;
                dbContext.Menus.Add(menu);
                dbContext.SaveChanges();
                trans.Commit();
                return menu;
            }
        }

        public Menu Update(string menuId, Menu menu)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                EntityUtils<Menu>.ValidateData(menu);
                var menuFind = dbContext.Menus.Find(menuId);
                if(menuFind == null)
                {
                    throw new QLNhaHangException(String.Format(Resource.QLNhaHangResource.MenuNotFound));
                }
                else
                {
                    menuFind.MenuName = menu.MenuName;
                    menuFind.Price = menu.Price;
                    menuFind.CategoryId = menu.CategoryId;
                    menuFind.Image = menu.Image;
                    menuFind.Description = menu.Description;
                    menuFind.UpdateTime = DateTime.Now;
                    dbContext.Update(menuFind);
                    dbContext.SaveChanges();
                    trans.Commit();
                    return menuFind;
                }
            }
        }

        public void Delete(string menuId)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                var menu = dbContext.Menus.Find(menuId);
                if (menu == null)
                {
                    throw new QLNhaHangException(String.Format(Resource.QLNhaHangResource.MenuNotFound));
                }
                else
                {
                    dbContext.Menus.Remove(menu);
                    dbContext.SaveChanges();
                    trans.Commit();
                }
            }
        }
    }
}
