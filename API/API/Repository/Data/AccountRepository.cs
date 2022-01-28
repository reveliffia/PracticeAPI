using API.Context;
using API.Models;
using API.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using bc = BCrypt.Net;

namespace API.Repository.Data
{
    public class AccountRepository : GeneralRepository<MyContext, Account, string>
    {
        private readonly MyContext context;
        public AccountRepository(MyContext myContext) : base(myContext)
        {
            this.context = myContext;
        }


        public int Login(AccountVM acc)
        {
            var result = 0;
            var cekEmailPhone = context.Employees.Where(e => e.Email == acc.Email
            || e.Phone == acc.Phone).FirstOrDefault();
            var pass = context.Accounts.Where(a => a.NIK == cekEmailPhone.NIK).FirstOrDefault();
            var cE = cekEmailPhone.Email.Contains(acc.Email);
            var cP = bc.BCrypt.Verify(acc.Password, pass.Password);
            if (cE == true && cP)
            {
                result = 1;
            }
            else
            {
                result = 0;
            }
            return result;
        }
        public IEnumerable Profile(AccountVM acc)
        {
            var employees = context.Employees;
            var accounts = context.Accounts;
            var profilings = context.Profilings;
            var educations = context.Educations;
            var universities = context.Universities;

            var result = (from emp in employees
                          join ac in accounts on emp.NIK equals ac.NIK
                          join pro in profilings on ac.NIK equals pro.NIK
                          join edu in educations on pro.Education_Id equals edu.Id
                          join univ in universities on edu.University_Id equals univ.Id
                          where acc.Email == emp.Email

                          select new
                          {
                              FullName = string.Concat(emp.FirstName + " " + emp.LastName),
                              Phone = emp.Phone,
                              BirthDate = emp.Birthdate,
                              Salary = emp.Salary,
                              Email = emp.Email,
                              Degree = edu.Degree,
                              GPA = edu.GPA,
                              UnivName = univ.Name
                          }).ToList();
            return result;
        }

        public int ForgotPassword(AccountVM acc)
        {
            var emailNow = (from g in context.Employees where g.Email == acc.Email select g).FirstOrDefault<Employee>();
            if (emailNow != null)
            {
                string numbers = "123456789";
                Random random = new Random();
                string otp = string.Empty;
                for (int i = 0; i < 6; i++)
                {
                    int tempval = random.Next(0, numbers.Length);
                    otp += tempval;
                }
                var accountNow = (from g in context.Accounts where g.NIK == emailNow.NIK select g).FirstOrDefault<Account>();
                string smtpAddress = "smtp.gmail.com";
                int portNumber = 587;
                bool enableSSL = true;
                string emailFromAddress = "aliffiakulsumarwati@gmail.com"; //Sender Email Address  
                string password = "Karawang150798"; //Sender Password  
                string emailToAddress = acc.Email; //Receiver Email Address  
                string subject = "OTP " + DateTime.Now;
                string body = "Hello, This is your OTP " + otp;
                accountNow.OTP = Convert.ToInt32(otp);
                accountNow.ExpiredToken = DateTime.Now.AddMinutes(5);
                accountNow.isUsed = false; //belum dipake otp-nya
                context.Entry(accountNow).State = EntityState.Modified; //insert data di account
                context.SaveChanges();

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(emailFromAddress);
                mail.To.Add(emailToAddress);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }
                return 1;
            }
            else
            {
                return 2; //email not found
            }

        }

        public int ChangePassword(AccountVM acc)
        {
            var emailNow = (from g in context.Employees where g.Email == acc.Email select g).FirstOrDefault<Employee>();
            if (emailNow != null)
            {
                var accountNow = (from g in context.Accounts where g.NIK == emailNow.NIK select g).FirstOrDefault<Account>();
                if (accountNow != null)
                {
                    if (accountNow.OTP == acc.OTP)
                    {
                        if (DateTime.Now < accountNow.ExpiredToken)
                        {
                            if (accountNow.isUsed == false)
                            {
                                if (acc.Password == acc.ConfirmPass)
                                {
                                    accountNow.Password = BCrypt.Net.BCrypt.HashPassword(acc.Password);
                                    accountNow.isUsed = true;
                                    context.Entry(accountNow).State = EntityState.Modified;
                                    context.SaveChanges();
                                    return 1; //berhasil
                                }
                                else
                                {
                                    return 2; //Password & ConfirmPass ga sama
                                }
                            }
                            else
                            {
                                return 3; //OTP udah dipakai
                            }
                        }
                        else
                        {
                            return 4; //OTP expired
                        }
                    }
                    else
                    {
                        return 5; //OTP salah
                    }
                }

            }
            return 0;
        }

    }

    /*  public int ForgotPassword(AccountVM accountVM)
      {
          var emailNow = (from g in context.Employees where g.Email == accountVM.Email select g).FirstOrDefault<Employee>();
          if (emailNow != null)
          {

              string smtpAddress = "smtp.gmail.com";
              int portNumber = 587;
              bool enableSSL = true;
              string emailFromAddress = "aliffiakulsumarwati@gmail.com"; //Sender Email Address
              string password = "Karawang150798"; //Sender Password
              string emailToAddress = accountVM.Email; //Receiver Email Address
              string subject = "Hello";
              string body = "Hello, This is Email sending test using gmail.";

              MailMessage mail = new MailMessage();
              mail.From = new MailAddress(emailFromAddress);
              mail.To.Add(emailToAddress);
              mail.Subject = subject;
              mail.Body = body;
              mail.IsBodyHtml = true;
              //mail.Attachments.Add(new Attachment("D:\TestFile.txt"));//--Uncomment this to send any attachment
              using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
              {
                  smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                  smtp.EnableSsl = enableSSL;
                  smtp.Send(mail);
              }
          }
          return 1;
      }*/

}

    

