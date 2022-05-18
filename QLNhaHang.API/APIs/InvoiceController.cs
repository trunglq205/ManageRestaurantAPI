using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLNhaHang.API.Entities;
using QLNhaHang.API.Exceptions;
using QLNhaHang.API.Interfaces;
using QLNhaHang.API.Services;
using QLNhaHang.API.Utils;

namespace QLNhaHang.API.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService invoiceService;
        public InvoiceController()
        {
            invoiceService = new InvoiceService();
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var res = invoiceService.Get();
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = EntityUtils<Invoice>.CatchValidateException(ex, Resource.QLNhaHangResource.ExceptionError);
                return StatusCode(500, response);
            }
        }
        [HttpGet("{invoiceId}")]
        public IActionResult GetById(string invoiceId)
        {
            try
            {
                var res = invoiceService.GetById(invoiceId);
                return Ok(res);
            }
            catch (QLNhaHangException ex)
            {
                var response = EntityUtils<Invoice>.CatchValidateException(ex, ex.Message);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = EntityUtils<Invoice>.CatchValidateException(ex, Resource.QLNhaHangResource.ExceptionError);
                return StatusCode(500, response);
            }
        }
        [HttpPost]
        public IActionResult Post(Invoice invoice)
        {
            try
            {
                var res = invoiceService.Insert(invoice);
                return Ok(res);
            }
            catch (QLNhaHangException ex)
            {
                var response = EntityUtils<Invoice>.CatchValidateException(ex, ex.Message, invoice);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = EntityUtils<Invoice>.CatchValidateException(ex, Resource.QLNhaHangResource.ExceptionError, invoice);
                return StatusCode(500, response);
            }
        }
        [HttpPost("export")]
        public async Task<IActionResult> ExportInvoice(Order[] orders)
        {
            await Task.Yield();
            var res = invoiceService.ExportExcel(orders);
            var stream = new MemoryStream();
            res.SaveAs(stream);
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "hoa-don.xlsx");
        }
    }
}
