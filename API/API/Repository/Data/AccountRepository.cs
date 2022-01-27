using API.Context;
using API.Models;
using API.ViewModel;
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

        public int ForgotPassword(AccountVM accountVM)
        {
            var emailNow = (from g in context.Employees where g.Email == accountVM.Email select g).FirstOrDefault<Employee>();
            if (emailNow != null)
            {

                string smtpAddress = "smtp.gmail.com";
                int portNumber = 587;
                bool enableSSL = true;
                string emailFromAddress = "aliffiakulsumarwati@gmail.com"; //Sender Email Address
                string password = "Karawang150798; //Sender Password
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
        }


        /*  ViewState["msgotp"] = otp;
          string msg = "your otp from abc.com is " + otp;
          bool f = SendOTP("from@from.com", "to@to.com", "Subjected to OTP", msg);
          if (f)
          { response.write("otp sent successfully"); }
          else { response.write("otp not sent"); }*/
    }

    
}
