using API.Context;
using API.Models;
using API.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

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
            var cekEmailPhone = myContext.Employees.Where(e => e.Email == loginVM.EmailPhone || e.Phone == loginVM.EmailPhone).FirstOrDefault();

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

        /*public IEnumerable<UserDataVM> GetProfile(LoginVM loginVM)
        {
            var result = myContext.Employees.Include(a => a.Account).ThenInclude(p => p.Profiling).ThenInclude(e => e.Education).ThenInclude(u => u.University).
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
                        }).Where(x => x.Email == loginVM.EmailPhone || x.Phone == loginVM.EmailPhone);

            return result;
        }*/

        public int ForgotPassword(ForgotPasswordVM fp)
        {
            var cekEmail = myContext.Employees.Where(e => e.Email == fp.Email).FirstOrDefault();
            if (cekEmail != null) {
                var cekAccount = myContext.Accounts.Where(a => a.NIK == cekEmail.NIK).FirstOrDefault();
                Random rand = new Random();
                var token = rand.Next(0, 1000000).ToString("D6");
                cekAccount.OTP = int.Parse(token);

                var time = DateTime.Now.AddMinutes(5);
                cekAccount.ExpiredToken = time;

                cekAccount.IsUsed = false;

                myContext.Entry(cekAccount).State = EntityState.Modified;
                myContext.SaveChanges();

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("milleepark94@gmail.com");
                mail.To.Add(fp.Email);
                mail.Subject = "OTP Forgot Password";
                mail.Body = ($"OTP : <strong> {token} </strong> <br/> Expired at : {time.ToString("G")}");
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("milleepark94@gmail.com", "xxxxxxxxxxx");
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return 1;
            }
            else
            {
                return 2;
            }
        }

        /*public int ChangePassword(ChangePasswordVM cp)
        {
            return 0;
        }*/
    }
}
