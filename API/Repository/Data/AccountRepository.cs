using API.Context;
using API.Models;
using API.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace API.Repository.Data
{
    public class AccountRepository : GeneralRepository<MyContext, Account, string>
    {
        private readonly MyContext myContext;

        public AccountRepository(MyContext myContext) : base(myContext)
        {
            this.myContext = myContext;
        }

        public int Login(LoginVM loginVM)
        {
            var cekEmailPhone = myContext.Employees.Where(e => e.Email == loginVM.Email || e.Phone == loginVM.Email).FirstOrDefault();

            if (cekEmailPhone != null)
            {
                var cekPassword = myContext.Accounts.Where(a => a.NIK == cekEmailPhone.NIK).FirstOrDefault();

                if (BCrypt.Net.BCrypt.Verify(loginVM.Password, cekPassword.Password))
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 3;
            }
        }

        /*public IEnumerable<RegisterVM> GetUserRole(LoginVM loginVM)
        {
            var employees = myContext.Employees;
            var accounts = myContext.Accounts;
            var accountRoles = myContext.AccountRoles;
            var roles = myContext.Roles;

            var result = (from e in employees
                          join a in accounts on e.NIK equals a.NIK
                          join ar in accountRoles on a.NIK equals ar.Account_NIK
                          join r in roles on ar.Role_Id equals r.Id
                          select new RegisterVM
                          {
                              RoleName = r.Name
                          }).ToList();

            return result;
        }*/

        public int ForgotPassword(ForgotPasswordVM fp)
        {
            var cekEmail = myContext.Employees.Where(e => e.Email == fp.Email).FirstOrDefault();
            if (cekEmail != null) {
                var cekAccount = myContext.Accounts.Where(a => a.NIK == cekEmail.NIK).FirstOrDefault();
                Random rand = new Random();
                var token = rand.Next(100000, 1000000).ToString("D6");
                cekAccount.OTP = int.Parse(token);

                var time = DateTime.Now.AddMinutes(5);
                cekAccount.ExpiredToken = time;

                cekAccount.IsUsed = false;

                myContext.Entry(cekAccount).State = EntityState.Modified;
                myContext.SaveChanges();

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("milleepark94@gmail.com");
                mail.To.Add(fp.Email);
                mail.Subject = ($"OTP Forgot Password {DateTime.Now.ToString("G")}");
                mail.Body = ($"OTP : <strong> {token} </strong> <br/> Expired at : {time.ToString("G")}");
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("milleepark94@gmail.com", "ansctrwhyn");
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return 1;
            }
            else
            {
                return 2;
            }
        }

        public int ChangePassword(ChangePasswordVM cp)
        {
            var cekEmail = myContext.Employees.Where(e => e.Email == cp.Email).FirstOrDefault();
            if (cekEmail != null)
            {
                var cekAccount = myContext.Accounts.Where(a => a.NIK == cekEmail.NIK).FirstOrDefault();
                if (cp.OTP == cekAccount.OTP) 
                {
                    if (cekAccount.IsUsed == false) 
                    {
                        if (DateTime.Now < cekAccount.ExpiredToken) 
                        {
                            if (cp.NewPassword == cp.ConfirmPassword)
                            {
                                cekAccount.Password = BCrypt.Net.BCrypt.HashPassword(cp.NewPassword);
                                myContext.Entry(cekAccount).State = EntityState.Modified;
                                cekAccount.IsUsed = true;
                                return myContext.SaveChanges();
                            }
                            else
                            {
                                return 6;
                            }
                        }
                        else
                        {
                            return 5;
                        }
                    }
                    else
                    {
                        return 4;
                    }
                }
                else
                {
                    return 3;
                }
            }
            else
            {
                return 2;
            }
        }
    }
}
