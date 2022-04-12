using QLNhaHang.API.Entities;

namespace QLNhaHang.API.Interfaces
{
    public interface IInvoiceService
    {
        public IEnumerable<Invoice> Get();


        public Invoice GetById(string invoiceId);


        public Invoice Insert(Invoice invoice);


        public Invoice Update(string invoiceId, Invoice invoice);


        public void Delete(string invoiceId);
    }
}
