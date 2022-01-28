using API.Base;
using API.Models;
using API.Repository;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BasesController<Account, AccountRepository, string>
    {
        public AccountRepository accountRepository;
        public AccountsController(AccountRepository accountRepository) : base(accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        [HttpGet("Login")]
        public ActionResult<AccountVM> Login(AccountVM acc)
        {
            var result = accountRepository.Login(acc);
            if (result != 1)
            {
                return StatusCode(400, new { status = HttpStatusCode.NotFound, 
                    result, message = "ACCOUNT TIDAK DITEMUKAN" });
            }
            else
            {
                //var re = RedirectToAction("Profile", acc.Email );
                //return RedirectToAction("Profile", new { @email = acc.Email });
                var re = accountRepository.Profile(acc);
                return Ok(re);
            }
        }

        [Route("Forgot")]
        [HttpPut("{Forgot}")]
        public ActionResult ForgotPassword(AccountVM acc)
        {
            var result = accountRepository.ForgotPassword(acc);
            if (result !=1)
            {
                return StatusCode(400, new { status = HttpStatusCode.NotFound, result, message = "ACCOUNT TIDAK DITEMUKAN" });
            }
            else
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, result, message = "OTP TERKIRIM" });
            }
        }
        [HttpPut("Change")]
        public ActionResult ChangePassword(AccountVM acc)
        {
            var result = accountRepository.ChangePassword(acc);
            if (result != 0)
            {
                if (result == 1)
                {
                    return StatusCode(200, new { status = HttpStatusCode.OK, message = "PASSWORD DIRUBAH!" });
                }
                else if (result == 2)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "PASSWORD DAN CONFIRM PASSWORD TIDAK SAMA" });
                }
                else if (result == 3)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "OTP SUDAH DIGUNAKAN" });
                }
                else if (result == 4)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "OTP EXPIRED" });
                }
                else
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "OTP SALAH!" });
                }
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.BadRequest, message = "EMAIL TIDAK DITEMUKAN!" });
            }
        }

    }
}
