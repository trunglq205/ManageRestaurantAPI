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
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;
        public OrderController()
        {
            orderService = new OrderService();
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var res = orderService.Get();
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = EntityUtils<Order>.CatchValidateException(ex, Resource.QLNhaHangResource.ExceptionError);
                return StatusCode(500, response);
            }
        }
        [HttpGet("{orderId}")]
        public IActionResult GetById(string orderId)
        {
            try
            {
                var res = orderService.GetById(orderId);
                return Ok(res);
            }
            catch (QLNhaHangException ex)
            {
                var response = EntityUtils<Order>.CatchValidateException(ex, ex.Message);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = EntityUtils<Order>.CatchValidateException(ex, Resource.QLNhaHangResource.ExceptionError);
                return StatusCode(500, response);
            }
        }

        [HttpGet("dashboard")]
        public IActionResult GetDashboard([FromQuery]int? month, [FromQuery] int? year)
        {
            try
            {
                var res = orderService.GetDashboard(month, year);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = EntityUtils<Order>.CatchValidateException(ex, Resource.QLNhaHangResource.ExceptionError);
                return StatusCode(500, response);
            }
        }


        [HttpPost]
        public IActionResult Post(Order order)
        {
            try
            {
                var res = orderService.Insert(order);
                return Ok(res);
            }
            catch (QLNhaHangException ex)
            {
                var response = EntityUtils<Order>.CatchValidateException(ex, ex.Message, order);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = EntityUtils<Order>.CatchValidateException(ex, Resource.QLNhaHangResource.ExceptionError, order);
                return StatusCode(500, response);
            }
        }
        [HttpPut("{orderId}")]
        public IActionResult Put(string orderId, Order order)
        {
            try
            {
                var res = orderService.Update(orderId, order);
                return Ok(res);
            }
            catch (QLNhaHangException ex)
            {
                var response = EntityUtils<Order>.CatchValidateException(ex, ex.Message, order);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = EntityUtils<Order>.CatchValidateException(ex, Resource.QLNhaHangResource.ExceptionError, order);
                return StatusCode(500, response);
            }
        }
        [HttpDelete("{orderId}")]
        public IActionResult Delete(string orderId)
        {
            try
            {
                orderService.Delete(orderId);
                return Ok();
            }
            catch (QLNhaHangException ex)
            {
                var response = EntityUtils<Order>.CatchValidateException(ex, ex.Message);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = EntityUtils<Order>.CatchValidateException(ex, Resource.QLNhaHangResource.ExceptionError);
                return StatusCode(500, response);
            }
        }
    }
}
