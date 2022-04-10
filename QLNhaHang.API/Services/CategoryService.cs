using QLNhaHang.API.Entities;
using QLNhaHang.API.Exceptions;
using QLNhaHang.API.Interfaces;
using QLNhaHang.API.Utils;

namespace QLNhaHang.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly QLNhaHangContext dbContext;
        public CategoryService()
        {
            dbContext = new QLNhaHangContext();
        }

        public IEnumerable<Category> Get()
        {
            return dbContext.Categories.ToList();
        }
        public Category Insert(Category category)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                EntityUtils<Category>.ValidateData(category);
                category.CategoryId = Guid.NewGuid().ToString();
                category.CreatedTime = DateTime.Now;
                dbContext.Categories.Add(category);
                dbContext.SaveChanges();
                trans.Commit();
                return category;
            }
        }

        public Category Update(string categoryId, Category category)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                var categoryFind = dbContext.Categories.Find(categoryId);
                if (categoryFind == null)
                {
                    throw new QLNhaHangException(String.Format(Resource.QLNhaHangResource.CategoryNotFound));
                }
                else
                {
                    categoryFind.CategoryName = category.CategoryName;
                    categoryFind.UpdatedTime = DateTime.Now;
                    dbContext.Update(categoryFind);
                    dbContext.SaveChanges();
                    trans.Commit();
                    return categoryFind;
                }
            }
        }
        public void Delete(string categoryId)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                var category = dbContext.Categories.Find(categoryId);
                if(category == null)
                {
                    throw new QLNhaHangException(String.Format(Resource.QLNhaHangResource.CategoryNotFound));
                }
                else
                {
                    dbContext.Categories.Remove(category);
                    dbContext.SaveChanges();
                    trans.Commit();
                }
            }
        }
    }
}
