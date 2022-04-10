using QLNhaHang.API.Entities;

namespace QLNhaHang.API.Interfaces
{
    public interface ICategoryService
    {
        /// <summary>
        /// Lấy danh sách danh mục món ăn
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Category> Get();

        /// <summary>
        /// Thêm mới danh mục món ăn
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public Category Insert (Category category);

        /// <summary>
        /// Sửa danh mục món ăn
        /// </summary>
        /// <param name="categoryID"></param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public Category Update (string categoryId, Category category);

        /// <summary>
        /// Xóa danh mục món ăn
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public void Delete (string categoryId);
    }
}
