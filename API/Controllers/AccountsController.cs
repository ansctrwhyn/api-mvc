using API.Base;
using API.Models;
using API.Models.ViewModel;
using API.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    public class AccountsController : BasesController<Account, AccountRepository, string>
    {
        private readonly AccountRepository accountRepository;
        public AccountsController(AccountRepository accountRepository) : base(accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        [Route("login")]
        [HttpPost]
        public ActionResult Login(LoginVM loginVM)
        {
            var result = accountRepository.Login(loginVM);
            if (result != 0)
            {
                if (result == 2)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "Password Salah!" });
                }
                else if (result == 3)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "Akun tidak ditemukan!" });
                }
                else
                {
                    return StatusCode(200, new { status = HttpStatusCode.OK, result, message = "Berhasil Login" });
                    //return RedirectToAction("Profile");
                    /*var result2 = accountRepository.GetProfile(loginVM);
                    return StatusCode(200, new { status = HttpStatusCode.OK, result2, message = "Data berhasil ditampilkan" });*/
                }
            }
            else
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "Gagal Login" });
            }
        }

        /*public ActionResult Profile(LoginVM loginVM)
        {
            var result = accountRepository.GetProfile(loginVM);
            if (result != null)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, result, message = "Data berhasil ditampilkan" });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, result, message = "Data tidak ditemukan" });
            }
        }*/

        [Route("forgotpassword")]
        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordVM fp)
        {
            var result = accountRepository.ForgotPassword(fp);
            if (result > 0)
            {
                if (result == 2)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "Akun tidak ditemukan!" });
                }
                else
                {
                    return StatusCode(200, new { status = HttpStatusCode.OK, result, message = "OTP berhasil dikirim. Cek Email Anda!" });
                }
            }
            else
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "Gagal" });
            }
        }

        [Route("changepassword")]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordVM cp)
        {
            var result = accountRepository.ChangePassword(cp);
            if (result != 0)
            {
                if (result == 2)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "Akun tidak ditemukan!" });
                }
                else if (result == 3)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "OTP Invalid!" });
                }
                else if (result == 4)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "OTP sudah digunakan!" });
                }
                else if (result == 5)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "OTP Expired!" });
                }
                else if (result == 6)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "Password tidak sama!" });
                }
                else
                {
                    return StatusCode(200, new { status = HttpStatusCode.OK, result, message = "Change Password Berhasil" });
                }
            }
            else
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "Gagal" });
            }
        }
    }
}
