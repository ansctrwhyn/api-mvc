using API.Context;
using API.Models;
using API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OldEmployeesController : ControllerBase
    {
        private OldEmployeeRepository employeeRepository;

        public OldEmployeesController(OldEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [HttpPost]
        public ActionResult Post(Employee employee)
        {
            var result = employeeRepository.Insert(employee);
            if (result != 0)
            {
                if (result == 2)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "Email sudah digunakan!" });
                }
                else if (result == 3)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "Nomor Telepon sudah digunakan!" });
                }
                else if(result == 4)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "Email dan Nomor Telepon sudah digunakan!" });
                }
                else
                {
                    return StatusCode(200, new { status = HttpStatusCode.OK, result, message = "Data Berhasil ditambahkan" });
                }
            }
            else
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "Gagal ditambahkan" });
            }
        }

        [HttpGet]
        public ActionResult Get()
        {
            var result = employeeRepository.Get();
            if (result.Count() > 0)
            {
                return StatusCode(200,new { status = HttpStatusCode.OK, result, message = "Data berhasil ditampilkan"});
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, result, message = "Data tidak ditemukan" });
            }
        }

        //[Route("getnik")]
        [HttpGet("{NIK}")]
        public ActionResult Get(Employee employee)
        {
            var result = employeeRepository.Get(employee.NIK);
            if (result != null)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, result, message = "Data berhasil ditampilkan" });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, result, message = "Data tidak ditemukan" });
            }
        }

        [HttpPut]
        public ActionResult Put(Employee employee)
        {
            var result = employeeRepository.Update(employee);
            if (result != 0)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, result, message = "Data berhasil diubah" });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, result, message = "Data tidak ditemukan" });
            }
        }

        //[HttpDelete("{NIK}")]
        /*[Route("deletenik")]
        public ActionResult Delete(string NIK)
        {
            var result = employeeRepository.Delete(NIK);
            if (result != 0)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, result, message = "Data berhasil dihapus" });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, result, message = "Data tidak ditemukan" });
            }
        }*/

        [HttpDelete]
        public ActionResult Delete(Employee employee) //baru
        {

            var result = employeeRepository.Delete(employee.NIK);
            if (result != 0)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, result, message = "Data berhasil dihapus" });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, result, message = "Data tidak ditemukan" });
            }
        }
    }
}
