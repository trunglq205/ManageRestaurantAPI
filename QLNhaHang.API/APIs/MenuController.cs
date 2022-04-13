using Microsoft.AspNetCore.Mvc;
using QLNhaHang.API.Entities;
using QLNhaHang.API.Exceptions;
using QLNhaHang.API.Helpers;
using QLNhaHang.API.Interfaces;
using QLNhaHang.API.Services;
using QLNhaHang.API.Utils;

namespace QLNhaHang.API.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService menuService;
        public MenuController(IConfiguration configuration)
        {
            menuService = new MenuService(configuration);
        }

        // GET: api/<MenuController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var res = menuService.Get();
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = EntityUtils<Menu>.CatchValidateException(ex, Resource.QLNhaHangResource.ExceptionError);
                return StatusCode(500, response);
            }
        }

        // GET api/<MenuController>/5
        [HttpGet("{menuId}")]
        public IActionResult GetById(string menuId)
        {
            try
            {
                var res = menuService.GetById(menuId);
                return Ok(res);
            }
            catch (QLNhaHangException ex)
            {
                var response = EntityUtils<Menu>.CatchValidateException(ex, ex.Message);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = EntityUtils<Menu>.CatchValidateException(ex, Resource.QLNhaHangResource.ExceptionError);
                return StatusCode(500, response);
            }
        }

        [HttpGet("paging")]
        public IActionResult GetPaging([FromQuery]string? categoryName, [FromQuery]string? keySearch,[FromQuery]Pagination? pagination = null)
        {
            try
            {
                var res = menuService.GetPaging(categoryName, keySearch, pagination);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = EntityUtils<Menu>.CatchValidateException(ex, Resource.QLNhaHangResource.ExceptionError);
                return StatusCode(500, response);
            }
        }

        // POST api/<MenuController>
        [HttpPost]
        public IActionResult Post(Menu menu)
        {
            try
            {
                var res = menuService.Insert(menu);
                return Ok(res);
            }
            catch (QLNhaHangException ex)
            {
                var response = EntityUtils<Menu>.CatchValidateException(ex, ex.Message, menu);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = EntityUtils<Menu>.CatchValidateException(ex, Resource.QLNhaHangResource.ExceptionError, menu);
                return StatusCode(500, response);
            }
        }

        // PUT api/<MenuController>/5
        [HttpPut("{menuId}")]
        public IActionResult Put(string menuId, Menu menu)
        {
            try
            {
                var res = menuService.Update(menuId, menu);
                return Ok(res);
            }
            catch (QLNhaHangException ex)
            {
                var response = EntityUtils<Menu>.CatchValidateException(ex, ex.Message, menu);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = EntityUtils<Menu>.CatchValidateException(ex, Resource.QLNhaHangResource.ExceptionError, menu);
                return StatusCode(500, response);
            }
        }

        // DELETE api/<MenuController>/5
        [HttpDelete("{menuId}")]
        public IActionResult Delete(string menuId)
        {
            try
            {
                menuService.Delete(menuId);
                return Ok();
            }
            catch (QLNhaHangException ex)
            {
                var response = EntityUtils<Menu>.CatchValidateException(ex, ex.Message);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = EntityUtils<Menu>.CatchValidateException(ex, Resource.QLNhaHangResource.ExceptionError);
                return StatusCode(500, response);
            }
        }
    }
}
