using API.Context;
using API.Models;
using API.Models.ViewModel;
using System.Linq;

namespace API.Repository.Data
{
    public class AccountRoleRepository : GeneralRepository<MyContext, AccountRole, int>
    {
        private readonly MyContext myContext;

        public AccountRoleRepository(MyContext myContext) : base(myContext)
        {
            this.myContext = myContext;
        }

        public int SignManager(SignManager sm)
        {
            var getEmployee = myContext.Employees.Where(e => e.NIK == sm.NIK).FirstOrDefault();
            if (getEmployee != null)
            {
                var accountRole = new AccountRole()
                {
                    Account_NIK = getEmployee.NIK,
                    Role_Id = 2
                };

                myContext.AccountRoles.Add(accountRole);
                return myContext.SaveChanges();

            }
            else
            {
                return 0;
            }
        }
    }
}
