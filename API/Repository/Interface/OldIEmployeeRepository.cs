using API.Models;
using System.Collections.Generic;

namespace API.Repository.Interface
{
    public interface OldIEmployeeRepository
    {
        IEnumerable<Employee> Get();
        Employee Get(string NIK);
        int Insert(Employee employee);
        int Update(Employee employee);
        int Delete(string NIK);
    }
}
