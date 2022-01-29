using API.Base;
using API.Models;
using API.Repository.Data;

namespace API.Controllers
{
    public class RolesController : BasesController<Role, RoleRepository, int>
    {
        private readonly RoleRepository roleRepository;
        public RolesController(RoleRepository roleRepository) : base(roleRepository)
        {
            this.roleRepository = roleRepository;
        }
    }
}
