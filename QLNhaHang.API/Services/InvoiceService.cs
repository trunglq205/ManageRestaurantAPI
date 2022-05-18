using OfficeOpenXml;
using OfficeOpenXml.Style;
using QLNhaHang.API.Entities;
using QLNhaHang.API.Exceptions;
using QLNhaHang.API.Interfaces;
using QLNhaHang.API.Utils;
using System.Globalization;

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

        public ExcelPackage ExportExcel(Order[] orders)
        {
            var package = new ExcelPackage();
            // khởi tạo worksheet hóa đơn
            var worksheet = package.Workbook.Worksheets.Add("Invoice");
            worksheet.Cells["C1:H1"].Merge = true;
            worksheet.Cells["C2:H2"].Merge = true;
            worksheet.Cells["C3:H3"].Merge = true;
            worksheet.Cells["C4:H4"].Merge = true;
            worksheet.Cells["C1:H1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["C1"].Value = "Cukcuk Restaurant";
            worksheet.Cells["C1"].Style.Font.Size = 20;
            worksheet.Cells["C1"].Style.Font.Bold = true;
            worksheet.Cells["C2"].Value = "Địa chỉ: số 08, đường Đa Phúc, Sóc Sơn - Hà Nội";
            worksheet.Cells["C3"].Value = "SĐT liên hệ: 0961339500";
            worksheet.Cells["C4"].Value = "Thời gian: " + DateTime.Now.ToString("HH:mm dd/MM/yyyy");

            worksheet.Cells["C6"].Value = "STT";
            worksheet.Cells["D6"].Value = "Tên món";
            worksheet.Cells["E6"].Value = "Đơn vị tính";
            worksheet.Cells["F6"].Value = "Số lượng";
            worksheet.Cells["G6"].Value = "Đơn giá";
            worksheet.Cells["H6"].Value = "Thành tiền";
            var th = worksheet.Cells[1, 1, 1, 6];
            th.Style.Font.Bold = true;
            var numRows = 6;
            decimal? tt = 0;
            var count = 0;
            foreach (var order in orders)
            {
                var list = order.OrderDetails.ToList();
                numRows += list.Count;
                for (int i = 0; i < list.Count(); i++)
                {
                    // thứ tự 
                    worksheet.Cells[i + 7 + count, 3].Value = (i + 1 + count).ToString();
                    worksheet.Cells[i + 7 + count, 4].Value = list[i].Menu.MenuName;
                    worksheet.Cells[i + 7 + count, 5].Value = list[i].Menu.Unit;
                    worksheet.Cells[i + 7 + count, 6].Value = list[i].Amount;
                    var donGia = string.Format(new CultureInfo("vi-VN"), "{0:#,##0}", list[i].Menu.Price);
                    worksheet.Cells[i + 7 + count, 7].Value = donGia;
                    worksheet.Cells[i + 7 + count, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    var thanhTien = string.Format(new CultureInfo("vi-VN"), "{0:#,##0}", list[i].Amount * list[i].Menu.Price);
                    worksheet.Cells[i + 7 + count, 8].Value = thanhTien;
                    worksheet.Cells[i + 7 + count, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    tt += list[i].Amount * list[i].Menu.Price;
                }
                count += list.Count;
            }
            var modelRows = numRows+1;
            string modelRange = "C6:H" + modelRows.ToString();
            var modelTable = worksheet.Cells[modelRange];
            var tongTien = worksheet.Cells["C" + modelRows.ToString() + ":G" + modelRows.ToString()];
            tongTien.Merge = true;
            tongTien.Value = "Tổng tiền(VNĐ):";
            var tong = string.Format(new CultureInfo("vi-VN"), "{0:#,##0}", tt);
            worksheet.Cells["H" + modelRows.ToString()].Value = tong;
            worksheet.Cells["H" + modelRows.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            string kyTen = "F" + (modelRows + 2).ToString() + ":H" + (modelRows + 2).ToString();
            worksheet.Cells[kyTen].Merge = true;
            worksheet.Cells[kyTen].Value = "Người lập hóa đơn";
            worksheet.Cells[kyTen].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            string ky = "F" + (modelRows + 3).ToString() + ":H" + (modelRows + 3).ToString();
            worksheet.Cells[ky].Merge = true;
            worksheet.Cells[ky].Value = "(Ký, ghi rõ họ tên)";
            worksheet.Cells[ky].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells.AutoFitColumns();
            return package;
        }

        public Invoice Update(string invoiceId, Invoice invoice)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                var invoiceFind = dbContext.Invoices.Find(invoiceId);
                if (invoiceFind == null)
                {
                    throw new QLNhaHangException(String.Format(Resource.QLNhaHangResource.InvoiceNotFound));
                }
                else
                {
                    invoiceFind.Status = invoice.Status;
                    invoiceFind.UpdatedTime = DateTime.Now;
                    dbContext.Invoices.Update(invoiceFind);
                    dbContext.SaveChanges();
                    return invoiceFind;
                }
            }
        }

        public void Delete(string invoiceId)
        {
            throw new NotImplementedException();
        }
    }
}
