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
                        order.TotalPrice += menu.Price * chiTiet.Amount;
                        dbContext.OrderDetails.Add(chiTiet);
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
                                orderFind.TotalPrice += menu.Price * orderDetail.Amount;
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
    }
}
