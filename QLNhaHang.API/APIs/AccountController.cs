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
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        public AccountController()
        {
            accountService = new AccountService();
        }


        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var res = accountService.Get();
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = EntityUtils<Account>.CatchValidateException(ex, Resource.QLNhaHangResource.ExceptionError);
                return StatusCode(500, response);
            }
        }

        [HttpPost("login")]
        public IActionResult Login(Account user)
        {
            try
            {
                var res = accountService.Login(user);
                return Ok(res);
            }
            catch (QLNhaHangException ex)
            {
                var response = EntityUtils<Account>.CatchValidateException(ex, ex.Message, user);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = EntityUtils<Account>.CatchValidateException(ex, Resource.QLNhaHangResource.ExceptionError, user);
                return StatusCode(500, response);
            }
        }

        [HttpPost]
        public IActionResult Post(Account user)
        {
            try
            {
                var res = accountService.Insert(user);
                return Ok(res);
            }
            catch (QLNhaHangException ex)
            {
                var response = EntityUtils<Account>.CatchValidateException(ex, ex.Message, user);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = EntityUtils<Account>.CatchValidateException(ex, Resource.QLNhaHangResource.ExceptionError, user);
                return StatusCode(500, response);
            }
        }

        [HttpPut("{userId}")]
        public IActionResult Put(string userId, Account user)
        {
            try
            {
                var res = accountService.Update(userId, user);
                return Ok(res);
            }
            catch (QLNhaHangException ex)
            {
                var response = EntityUtils<Account>.CatchValidateException(ex, ex.Message, user);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = EntityUtils<Account>.CatchValidateException(ex, Resource.QLNhaHangResource.ExceptionError, user);
                return StatusCode(500, response);
            }
        }
    }
}
