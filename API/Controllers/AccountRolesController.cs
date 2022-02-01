using API.Base;
using API.Context;
using API.Models;
using API.Models.ViewModel;
using API.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    public class AccountRolesController : BasesController<AccountRole, AccountRoleRepository, int>
    {
        private readonly AccountRoleRepository accountRoleRepository;


        public AccountRolesController(AccountRoleRepository accountRoleRepository, MyContext myContext) : base(accountRoleRepository)
        {
            this.accountRoleRepository = accountRoleRepository;
        }

        [Authorize(Roles = "Director")]
        [Route("signmanager")]
        [HttpPost("SignManager")]
        public ActionResult SignManager(SignManager sm)
        {
            var result = accountRoleRepository.SignManager(sm);
            if (result != 0)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, result, message = "Berhasil" });
            }
            else
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "Data tidak ditemukan" });
            }
        }
    }
}
