using QLNhaHang.API.Entities;
using QLNhaHang.API.Helpers;

namespace QLNhaHang.API.Interfaces
{
    public interface IOrderService
    {
        public PageResult<Order> Get(Pagination? pagination = null);


        public Order GetById(string orderId);

        public object GetDashboard(int? month, int? year);


        public Order Insert(Order order);


        public Order Update(string orderId, Order order);


        public void Delete(string orderId);
    }
}
