using API.Base;
using API.Models;
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
    public class EmployeesController : BasesController<Employee, EmployeeRepository, string>
    {
        public EmployeeRepository employeeRepository;
        public EmployeesController(EmployeeRepository employeeRepository) : base(employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        [Route("Register")]
        [HttpPost]
        public ActionResult RegisterVM(RegisterVM reg)
        {
            var result = employeeRepository.RegisterVM(reg);

            if (result != 0)
            {
                if (result == 1)
                {
                    return StatusCode(200, new { status = HttpStatusCode.OK, message = "Register berhasil" });
                }
                else if (result == 4)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "REGISTER GAGAL! Email sudah ada" });
                }
                else if (result == 5)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "REGISTER GAGAL! phone sudah ada" });
                }
                else if (result == 6)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "REGISTER GAGAL! Email dan phone sudah ada" });
                }
                else
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "REGISTER GAGAL 1" });
                }
            }
            else
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "REGISTER GAGAL 2" });
            }
        }
        [Route("GetRegisteredData")]
        [HttpGet]
        public ActionResult<RegisterVM> GetRegisteredData()
        {
            var result = employeeRepository.GetRegisteredData();
            if (result != null)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, result, message = "DATA DITEMUKAN" });
            }
            else
            {
                return StatusCode(400, new { status = HttpStatusCode.NotFound, result, message = "DATA TIDAK DITEMUKAN" });
            }
        }

    }
}
