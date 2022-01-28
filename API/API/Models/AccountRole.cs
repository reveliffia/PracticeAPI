using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("TB_M_AccountRole")]
    public class AccountRole
    {
        public virtual Account Account { get; set; }
        public virtual Role Role { get; set; }

        [ForeignKey("Account")]
        public string Account_NIK { get; set; }

        [ForeignKey("Role")]
        public int Role_Id { get; set; }
    }
}
