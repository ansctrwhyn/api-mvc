using API.Context;
using API.Models;
using API.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace API.Repository
{
    public class OldEmployeeRepository : OldIEmployeeRepository
    {
        private readonly MyContext context;

        public OldEmployeeRepository(MyContext context)
        {
            this.context = context;
        }

        public int Delete(string NIK)
        {
            var entity = context.Employees.Find(NIK);
            if (entity == null)
            {
                return 0;
            }
            context.Remove(entity);
            var result = context.SaveChanges();
            return result;
        }

        /*public int Delete(Employee employee) // baru
        {
            var entity = context.Employees.Find(employee.NIK);
            context.Remove(entity);
            var result = context.SaveChanges();
            return result;
        }*/

        public IEnumerable<Employee> Get()
        {
            return context.Employees.ToList();
        }

        public Employee Get(string NIK)
        {
            //var result = context.Employees.Where(emp => emp.NIK == NIK).Single();
            var result = context.Employees.Find(NIK);
            return result;
        }

        public int GetLastNIK()
        {
            var lastEmp = context.Employees.OrderByDescending(emp => emp.NIK).FirstOrDefault();
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

        public bool IsEmailExist(Employee employee)
        {
            var CekEmail = context.Employees.Where(emp => emp.Email == employee.Email).FirstOrDefault();
            if (CekEmail != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsPhoneExist(Employee employee)
        {
            var CekPhone = context.Employees.Where(emp => emp.Phone == employee.Phone).FirstOrDefault();
            if (CekPhone != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int Insert(Employee employee)
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

                    context.Employees.Add(employee);
                    var result = context.SaveChanges();
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
        }

        public int Update(Employee employee)
        {
            var entity = context.Employees.Find(employee.NIK);
            if (entity == null)
            {
                return 0;
            }
            context.Remove(entity);
            context.Entry(employee).State = EntityState.Modified;
            var result = context.SaveChanges();
            return result;           
        }
    }
}
