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
                return StatusCode(400, new { status = HttpStatusCode.NotFound, result, message = "ACCOUNT TIDAK DITEMUKAN" });
            }
            else
            {
                //var re = RedirectToAction("Profile", acc.Email );
                //return RedirectToAction("Profile", new { @email = acc.Email });
                var re = accountRepository.Profile(acc);
                return Ok(re);
            }
        }


    }

}
