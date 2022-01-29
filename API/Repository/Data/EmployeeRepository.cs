using API.Context;
using API.Models;
using API.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace API.Repository.Data
{
    public class EmployeeRepository : GeneralRepository<MyContext, Employee, string>
    {
        private readonly MyContext myContext;
        public EmployeeRepository(MyContext myContext) : base(myContext)
        {
            this.myContext = myContext;
        }

        public IEnumerable<UserDataVM> GetUserData()
        {
            var result = myContext.Employees.Include(a => a.Account).ThenInclude(p => p.Profiling).
                ThenInclude(e => e.Education).ThenInclude(u => u.University).
                        Select(x => new UserDataVM
                          {
                              FullName = x.FirstName + " " + x.LastName,
                              Phone = x.Phone,
                              BirthDate = x.BirthDate,
                              Salary = x.Salary,
                              Email = x.Email,
                              Degree = x.Account.Profiling.Education.Degree,
                              GPA = x.Account.Profiling.Education.GPA,
                              UniversityName = x.Account.Profiling.Education.University.Name
                        }).ToList();

            return result;
        }

        public IEnumerable<RegisterVM> GetRegisteredData(){
            var employees = myContext.Employees;
            var accounts = myContext.Accounts;
            var accountRoles = myContext.AccountRoles;
            var roles = myContext.Roles;
            var profilings = myContext.Profilings;
            var educations = myContext.Educations;
            var universities = myContext.Universities;

            var result = (from e in employees
                          join a in accounts on e.NIK equals a.NIK
                          join ar in accountRoles on a.NIK equals ar.Account_NIK
                          join r in roles on ar.Role_Id equals r.Id
                          join p in profilings on a.NIK equals p.NIK
                          join ed in educations on p.Education_Id equals ed.Id
                          join u in universities on ed.University_Id equals u.Id

                          select new RegisterVM
                          {
                              FullName = e.FirstName + " " + e.LastName,
                              Phone = e.Phone,
                              BirthDate = e.BirthDate,
                              Salary = e.Salary,
                              Email = e.Email,
                              Degree = ed.Degree,
                              GPA = ed.GPA,
                              UniversityName = u.Name,
                              RoleName = r.Name
                          }).ToList();

            return result;
        }


        public int Register(RegisterVM registerVM)
        {
            var EmailExist = IsEmailExist(registerVM);
            var PhoneExist = IsPhoneExist(registerVM);
            if (EmailExist == false)
            {
                if (PhoneExist == false)
                {
                    var NIK = GetLastNIK() + 1;
                    var Year = DateTime.Now.Year;
                    registerVM.NIK = Year + "00" + NIK.ToString();

                    var employee = new Employee()
                    {
                        NIK = registerVM.NIK,
                        FirstName = registerVM.FirstName,
                        LastName = registerVM.LastName,
                        Phone = registerVM.Phone,
                        BirthDate = registerVM.BirthDate,
                        Salary = registerVM.Salary,
                        Email = registerVM.Email,
                        Gender = (Models.Gender)registerVM.Gender
                    };

                    myContext.Employees.Add(employee);
                    myContext.SaveChanges();

                    var account = new Account
                    {
                        NIK = employee.NIK,
                        Password = BCrypt.Net.BCrypt.HashPassword(registerVM.Password)
                    };

                    myContext.Accounts.Add(account);
                    myContext.SaveChanges();

                    var accountRole = new AccountRole()
                    {
                        Account_NIK = employee.NIK,
                        Role_Id = 1
                    };
                    myContext.AccountRoles.Add(accountRole);
                    myContext.SaveChanges();

                    var education = new Education
                    {
                        Degree = registerVM.Degree,
                        GPA = registerVM.GPA,
                        University_Id = registerVM.University_Id
                    };

                    myContext.Educations.Add(education);
                    myContext.SaveChanges();

                    var profiling = new Profiling
                    {
                        NIK = account.NIK,
                        Education_Id = education.Id
                    };

                    myContext.Profilings.Add(profiling);
                    myContext.SaveChanges();

                    return 1;
                }
                else
                {
                    return 3; //nomor telepon sudah ada
                }
            }
            else if (EmailExist == true && PhoneExist == true)
            {
                return 4; //email dan nomor telepon sudah ada
            }
            else
            {
                return 2; //email sudah ada
            }
        }

        public int GetLastNIK()
        {
            var lastEmp = myContext.Employees.OrderByDescending(emp => emp.NIK).FirstOrDefault();
            if (lastEmp == null)
            {
                return 0;
            }
            else
            {
                var lastNIK = lastEmp.NIK.Remove(0, 5);
                return int.Parse(lastNIK);
            }
        }

        public bool IsEmailExist(RegisterVM registerVM)
        {
            var CekEmail = myContext.Employees.Where(emp => emp.Email == registerVM.Email).FirstOrDefault();
            if (CekEmail != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsPhoneExist(RegisterVM registerVM)
        {
            var CekPhone = myContext.Employees.Where(emp => emp.Phone == registerVM.Phone).FirstOrDefault();
            if (CekPhone != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*public override int Insert(Employee employee)
        {
            var EmailExist = IsEmailExist(employee);
            var PhoneExist = IsPhoneExist(employee);
            if (EmailExist == false)
            {
                if (PhoneExist == false)
                {
                    var NIK = GetLastNIK() + 1;
                    var Year = DateTime.Now.Year;
                    employee.NIK = Year + "00" + NIK.ToString();

                    myContext.Employees.Add(employee);
                    var result = myContext.SaveChanges();
                    return result;
                }
                else
                {
                    return 3; //nomor telepon sudah ada
                }
            }
            else if (EmailExist == true && PhoneExist == true)
            {
                return 4; //email dan nomor telepon sudah ada
            }
            else
            {
                return 2; //email sudah ada
            }
        }*/
    }
}
