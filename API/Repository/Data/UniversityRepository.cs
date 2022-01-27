using API.Context;
using API.Models;

namespace API.Repository.Data
{
    public class UniversityRepository : GeneralRepository<MyContext, University, int>
    {
       private readonly MyContext myContext;
        public UniversityRepository(MyContext myContext) : base(myContext)
        {
           this.myContext = myContext;
        }
    }
}
