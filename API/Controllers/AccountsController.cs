using API.Base;
using API.Context;
using API.Models;
using API.Models.ViewModel;
using API.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    public class AccountsController : BasesController<Account, AccountRepository, string>
    {
        private readonly AccountRepository accountRepository;
        public IConfiguration _configuration;
        public MyContext myContext;

        public AccountsController(AccountRepository accountRepository, IConfiguration configuration, MyContext myContext) : base(accountRepository)
        {
            this.accountRepository = accountRepository;
            this._configuration = configuration;
            this.myContext = myContext;
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
                    /*var getUserData = accountRepository.GetUserRole(loginVM);

                    var role = "";
                    foreach (var data in getUserData)
                    {
                       role = data.RoleName;
                    }*/

                    var getUserData = myContext.Employees.Where(e => e.Email == loginVM.Email || e.Phone == loginVM.Email).FirstOrDefault();
                    var getRole = myContext.Roles.Where(r => r.AccountRoles.Any(ar => ar.Account.NIK == getUserData.NIK)).ToList();

                    var claims = new List<Claim>
                    {
                        new Claim("Email", loginVM.Email) //payload
                    };

                    foreach (var item in getRole)
                    {
                        claims.Add(new Claim("roles", item.Name));
                    }

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //Header
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10), //set expired times
                        signingCredentials: signIn);
                    var idToken = new JwtSecurityTokenHandler().WriteToken(token); //generate token
                    claims.Add(new Claim("TokenSecurity", idToken.ToString()));

                    return StatusCode(200, new { status = HttpStatusCode.OK, idToken, result, message = "Berhasil Login" });
                }
            }
            else
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "Gagal Login" });
            }
        }

        [Authorize]
        [HttpGet("TestJWT")]
        public ActionResult TestJWT()
        {
            return Ok("Test JWT Berhasil");
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
