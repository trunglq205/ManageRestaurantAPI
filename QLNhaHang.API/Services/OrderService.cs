using Microsoft.EntityFrameworkCore;
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
            return dbContext.Orders.Include(x=>x.OrderDetails).ThenInclude(x=>x.Menu).ToList();
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
                var lstOrderDetails = dbContext.OrderDetails.Where(x=>x.OrderId == orderId).ToList();
                order.OrderDetails = lstOrderDetails;
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
                var lstOrderDetails = order.OrderDetails;
                order.OrderDetails = null;
                order.Status = Enums.Status.Waiting;
                order.TotalPrice = 0;
                dbContext.Orders.Add(order);
                dbContext.SaveChanges();
                foreach (var chiTiet in lstOrderDetails)
                {
                    if(dbContext.Menus.Any(x=>x.MenuId == chiTiet.MenuId))
                    {
                        chiTiet.OrderDetailId = Guid.NewGuid().ToString();
                        chiTiet.OrderId = order.OrderId;
                        var menu = dbContext.Menus.Find(chiTiet.MenuId);
                        chiTiet.Price = menu.Price * chiTiet.Amount;
                        order.TotalPrice += chiTiet.Price;
                        dbContext.OrderDetails.Add(chiTiet);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        throw new QLNhaHangException(Resource.QLNhaHangResource.MenuNotFound);
                    }
                }
                dbContext.Orders.Update(order);
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
                    var lstOrderDetails = dbContext.OrderDetails.Where(x => x.OrderId == orderFind.OrderId).ToList();
                    orderFind.TotalPrice = 0;
                    if (order.OrderDetails == null || order.OrderDetails.Count == 0)
                    {
                        dbContext.OrderDetails.RemoveRange(lstOrderDetails);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        var lstDeletes = new List<OrderDetail>();
                        foreach (var orderDetail in lstOrderDetails)
                        {
                            if (!order.OrderDetails.Any(x => x.OrderDetailId == orderDetail.OrderDetailId))
                            {
                                lstDeletes.Add(orderDetail);
                            }
                            else
                            {
                                var orderDetailUpdate = order.OrderDetails.FirstOrDefault(x => x.OrderDetailId == orderDetail.OrderDetailId);
                                orderDetail.OrderId = orderFind.OrderId; 
                                orderDetail.Amount = orderDetailUpdate.Amount;
                                dbContext.OrderDetails.Update(orderDetail);
                                var menu = dbContext.Menus.FirstOrDefault(x=>x.MenuId == orderDetail.MenuId);
                                orderDetail.Price = menu.Price * orderDetail.Amount;
                                orderFind.TotalPrice += orderDetail.Price;
                            }
                        }
                        dbContext.OrderDetails.RemoveRange(lstDeletes);
                        dbContext.SaveChanges();
                    }
                    orderFind.TableNumber = order.TableNumber;
                    orderFind.UpdateTime = DateTime.Now;
                    dbContext.Orders.Update(orderFind);
                    dbContext.SaveChanges();
                    trans.Commit();
                    return orderFind;
                }
            }
        }

        public void Delete(string orderId)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                var order = dbContext.Orders.Find(orderId);
                if (order == null)
                {
                    throw new QLNhaHangException(String.Format(Resource.QLNhaHangResource.OrderNotFound));
                }
                else
                {
                    var lstOrderDetails = dbContext.OrderDetails.Where(x => x.OrderId == orderId).ToList();
                    dbContext.OrderDetails.RemoveRange(lstOrderDetails);
                    dbContext.SaveChanges();
                    dbContext.Orders.Remove(order);
                    dbContext.SaveChanges();
                    trans.Commit();
                }
                
            }
        }

        public object GetDashboard(int? month, int? year)
        {
            var lst = new List<decimal?>();
            var today = DateTime.Now.Date;
            var paid = dbContext.Orders.Where(x => x.CreatedTime.Value >= today && x.Status == Enums.Status.Paid).Sum(x=>x.TotalPrice);
            var cancel = dbContext.Orders.Where(x => x.CreatedTime.Value >= today && x.Status == Enums.Status.Cancel).Sum(x => x.TotalPrice);
            var serving = dbContext.Orders.Where(x => x.CreatedTime.Value >= today && x.Status != Enums.Status.Paid && x.Status != Enums.Status.Cancel).Sum(x => x.TotalPrice);
            var lstOrder = dbContext.Orders.Where(x => x.CreatedTime.Value.Year == year && x.CreatedTime.Value.Month == month && x.Status == Enums.Status.Paid).ToList();

            var totalOfMonth = EntityUtils<Order>.GetTotalDayOfMonth(month, year);

            for (int i = 1; i <= totalOfMonth; i++)
            {
                decimal? price = 0;
                foreach (var item in lstOrder)
                {
                    if(item.CreatedTime.Value.Day == i)
                    {
                        price += item.TotalPrice;
                    }
                }
                lst.Add(price);
            }

            return new
            {
                Paid = paid,
                Serving = serving,
                Cancel = cancel,
                ByDate = lst

            };
        }
    }
}
