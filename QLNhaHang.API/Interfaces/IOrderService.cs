using QLNhaHang.API.Entities;

namespace QLNhaHang.API.Interfaces
{
    public interface IOrderService
    {
        public IEnumerable<Order> Get();


        public Order GetById(string orderId);

        public object GetDashboard(int? month, int? year);


        public Order Insert(Order order);


        public Order Update(string orderId, Order order);


        public void Delete(string orderId);
    }
}
