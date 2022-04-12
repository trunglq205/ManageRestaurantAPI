using QLNhaHang.API.Entities;
using QLNhaHang.API.Exceptions;
using QLNhaHang.API.Interfaces;
using QLNhaHang.API.Utils;

namespace QLNhaHang.API.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly QLNhaHangContext dbContext;
        public InvoiceService()
        {
            dbContext = new QLNhaHangContext();
        }

        public IEnumerable<Invoice> Get()
        {
            return dbContext.Invoices.ToList();
        }

        public Invoice GetById(string invoiceId)
        {
            var invoice = dbContext.Invoices.Find(invoiceId);
            if (invoice == null)
            {
                throw new QLNhaHangException(String.Format(Resource.QLNhaHangResource.InvoiceNotFound));
            }
            else
            {
                var lstInvoiceDetails = dbContext.InvoiceDetails.Where(x => x.InvoiceId == invoiceId).ToList();
                invoice.InvoiceDetails = lstInvoiceDetails;
                return invoice;
            }
        }

        public Invoice Insert(Invoice invoice)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                EntityUtils<Invoice>.ValidateData(invoice);
                invoice.InvoiceId = Guid.NewGuid().ToString();
                invoice.CreatedTime = DateTime.Now;
                var lstInvoiceDetails = invoice.InvoiceDetails.ToList();
                invoice.InvoiceDetails = null;
                invoice.Status = Enums.Status.Unpaid;
                invoice.TotalPrice = 0;
                dbContext.Invoices.Add(invoice);
                dbContext.SaveChanges();
                foreach (var chiTiet in lstInvoiceDetails)
                {
                    if (dbContext.Orders.Any(x => x.OrderId == chiTiet.OrderId))
                    {
                        chiTiet.InvoiceDetailId = Guid.NewGuid().ToString();
                        chiTiet.InvoiceId = invoice.InvoiceId;
                        var order = dbContext.Orders.Find(chiTiet.OrderId);
                        invoice.TotalPrice += order.TotalPrice;
                        dbContext.InvoiceDetails.Add(chiTiet);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        throw new QLNhaHangException(Resource.QLNhaHangResource.OrderNotFound);
                    }
                }
                dbContext.Invoices.Update(invoice);
                dbContext.SaveChanges();
                trans.Commit();
                return invoice;
            }
        }

        public Invoice Update(string invoiceId, Invoice invoice)
        {
            throw new NotImplementedException();
        }

        public void Delete(string invoiceId)
        {
            throw new NotImplementedException();
        }
    }
}
