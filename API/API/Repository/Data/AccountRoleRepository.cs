using API.Context;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class AccountRoleRepository : GeneralRepository<MyContext, AccountRole, string>
    {
        private readonly MyContext context;
        public AccountRoleRepository(MyContext myContext) : base(myContext)
        {
            this.context = myContext;
        }

        public int SignManager(AccountRole accountRole)
        {
            var cekNIK = context.AccountRoles.Where(e => e.Account_NIK == accountRole.Account_NIK).FirstOrDefault();
            if (cekNIK != null)
            {
                var AR = new AccountRole()
                {
                    Account_NIK = accountRole.Account_NIK,
                    Role_Id = 2
                };
                context.AccountRoles.Add(AR);
            }
            var result = context.SaveChanges();
            return result;
        }
    }
}
