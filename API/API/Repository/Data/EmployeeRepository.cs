using API.Context;
using API.Models;
using API.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class EmployeeRepository : GeneralRepository<MyContext, Employee, string>
    {
        private readonly MyContext context;
        public EmployeeRepository(MyContext myContext) : base(myContext)
        {
            this.context = myContext;
        }

        private string formatId()
        {
            var idDate = DateTime.Now.ToString("yyyy");
            var lastId = context.Employees.ToList().Count();
            var format = idDate + "00" + lastId;
            while (!format.Equals(Get(format)))
            {
                format = idDate + "00" + (lastId += 1);
                if (Get(format) == null)
                {
                    break;
                }
            }
            return format;
        }
        public int RegisterVM(RegisterVM reg)
        {
            var EmailExist = IsEmailExist(reg);
            var PhoneExist = IsPhoneExist(reg);
            if (EmailExist == false)
            {
                if (PhoneExist == false)
                {
                    var EMP = new Employee()
                    {
                        NIK = formatId(),
                        FirstName = reg.FirstName,
                        LastName = reg.LastName,
                        Phone = reg.Phone,
                        Birthdate = reg.BirthDate,
                        Email = reg.Email,
                        GenderId = (Models.Gender)reg.Gender,
                        Salary = reg.Salary
                    };
                    context.Employees.Add(EMP);

                    var ACC = new Account()
                    {
                        NIK = EMP.NIK,
                        Password = BCrypt.Net.BCrypt.HashPassword(reg.Password)

                    };
                    context.Accounts.Add(ACC);

                    var AR = new AccountRole()
                    {
                        Account_NIK = EMP.NIK,
                        Role_Id = 1
                    };
                    context.AccountRoles.Add(AR);              
                   

                    var EDU = new Education()
                    {
                        Degree = reg.Degree,
                        GPA = reg.GPA,
                        University_Id = reg.University_Id
                    };
                    context.Educations.Add(EDU);

                    context.SaveChanges();
                    var PROF = new Profiling()
                    {
                        NIK = EMP.NIK,
                        Education_Id=EDU.Id
                    };
                    context.Profilings.Add(PROF);
                    return context.SaveChanges();
                }
                else
                {
                    return 5; //phone exist
                }
            }
            else if (EmailExist == true && PhoneExist == true)
            {
                return 6; //email & phone exist
            }
            else
            {
                return 4; //email exist
            }            
        }

        public bool IsEmailExist(RegisterVM reg)
        {
            var cek = context.Employees.Where(s => s.Email == reg.Email).FirstOrDefault<Employee>();
            if (cek != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsPhoneExist(RegisterVM reg)
        {
            var cek = context.Employees.Where(s => s.Phone == reg.Phone).FirstOrDefault<Employee>();
            if (cek != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable GetRegisteredData()
        {
            var employees = context.Employees;
            var accounts = context.Accounts;
            var profilings = context.Profilings;
            var educations = context.Educations;
            var universities = context.Universities;
            var roles = context.Roles;
            var accountroles = context.AccountRoles;

            var result = (from emp in employees
                          join acc in accounts on emp.NIK equals acc.NIK
                          join pro in profilings on acc.NIK equals pro.NIK
                          join edu in educations on pro.Education_Id equals edu.Id
                          join univ in universities on edu.University_Id equals univ.Id
                          join ar in accountroles on acc.NIK equals ar.Account_NIK
                          join role in roles on ar.Role_Id equals role.Id


                          select new
                          {
                              FullName = string.Concat(emp.FirstName + " " + emp.LastName),
                              Phone = emp.Phone,
                              BirthDate = emp.Birthdate,
                              Salary = emp.Salary,
                              Email = emp.Email,
                              Degree = edu.Degree,
                              GPA = edu.GPA,
                              UnivName = univ.Name,
                              Role = role.Name
                          }).ToList();
            return result;
        }      
    }    
}
