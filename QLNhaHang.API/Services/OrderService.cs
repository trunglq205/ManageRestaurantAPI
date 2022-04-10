using QLNhaHang.API.Entities;
using QLNhaHang.API.Exceptions;
using QLNhaHang.API.Interfaces;
using QLNhaHang.API.Utils;

namespace QLNhaHang.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly QLNhaHangContext dbContext;
        public OrderService()
        {
            dbContext = new QLNhaHangContext();
        }

        public IEnumerable<Order> Get()
        {
            return dbContext.Orders.ToList();
        }

        public Order GetById(string orderId)
        {
            var order = dbContext.Orders.Find(orderId);
            if (order == null)
            {
                throw new QLNhaHangException(String.Format(Resource.QLNhaHangResource.OrderNotFound));
            }
            else
            {
                return order;
            }
        }

        public Order Insert(Order order)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                EntityUtils<Order>.ValidateData(order);
                order.OrderId = Guid.NewGuid().ToString();
                order.CreatedTime = DateTime.Now;
                var lstChiTietOrder = order.OrderDetails;
                order.OrderDetails = null;
                order.Status = Enums.Status.Waiting;
                order.TotalPrice = 0;
                dbContext.Orders.Add(order);
                dbContext.SaveChanges();
                foreach (var chiTiet in lstChiTietOrder)
                {
                    if(dbContext.Menus.Any(x=>x.MenuId == chiTiet.MenuId))
                    {
                        chiTiet.OrderDetailId = Guid.NewGuid().ToString();
                        chiTiet.OrderId = order.OrderId;
                        var menu = dbContext.Menus.Find(chiTiet.MenuId);
                        order.TotalPrice += menu.Price * chiTiet.Amount;
                        dbContext.OrderDetails.Add(chiTiet);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        throw new QLNhaHangException(Resource.QLNhaHangResource.MenuNotFound);
                    }
                }
                dbContext.Update(order);
                dbContext.SaveChanges();
                trans.Commit();
                return order;
            }
        }

        public Order Update(string orderId, Order order)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                EntityUtils<Order>.ValidateData(order);
                var orderFind = dbContext.Orders.Find(orderId);
                if (orderFind == null)
                {
                    throw new QLNhaHangException(String.Format(Resource.QLNhaHangResource.OrderNotFound));
                }
                else
                {
                    orderFind.TableNumber = order.TableNumber;
                    orderFind.UpdateTime = DateTime.Now;
                    dbContext.Update(orderFind);
                    dbContext.SaveChanges();
                    trans.Commit();
                    return orderFind;
                }
            }
        }

        public void Delete(string orderId)
        {
            throw new NotImplementedException();
        }
    }
}
