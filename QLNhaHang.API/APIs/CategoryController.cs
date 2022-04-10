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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        public CategoryController()
        {
            categoryService = new CategoryService();
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var res = categoryService.Get();
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = EntityUtils<Category>.CatchValidateException(ex, Resource.QLNhaHangResource.ExceptionError);
                return StatusCode(500, response);
            }
        }
        [HttpPost]
        public IActionResult Post (Category category)
        {
            try
            {
                var res = categoryService.Insert(category);
                return Ok(res);
            }
            catch (QLNhaHangException ex)
            {
                var response = EntityUtils<Category>.CatchValidateException(ex, ex.Message, category);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = EntityUtils<Category>.CatchValidateException(ex, Resource.QLNhaHangResource.ExceptionError, category);
                return StatusCode(500, response);
            }
        }
        [HttpPut("{categoryId}")]
        public IActionResult Put (string categoryId, Category category)
        {
            try
            {
                var res = categoryService.Update(categoryId, category);
                return Ok(res);
            }
            catch (QLNhaHangException ex)
            {
                var response = EntityUtils<Category>.CatchValidateException(ex, ex.Message, category);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = EntityUtils<Category>.CatchValidateException(ex, Resource.QLNhaHangResource.ExceptionError, category);
                return StatusCode(500, response);
            }
        }
        [HttpDelete("{categoryId}")]
        public IActionResult Delete(string categoryId)
        {
            try
            {
                categoryService.Delete(categoryId);
                return Ok();
            }
            catch(QLNhaHangException ex)
            {
                var response = EntityUtils<Category>.CatchValidateException(ex, ex.Message);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = EntityUtils<Category>.CatchValidateException(ex, Resource.QLNhaHangResource.ExceptionError);
                return StatusCode(500, response);
            }
        }
    }
}
