using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Models
{
    public class Account
    {
        public int Id { get; set; } 
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public bool IsLockout { get; set; }
        public int RoleId { get; set; }
        public virtual Role? Role { get; set; }
        public virtual ICollection<UserSkill>? UserSkill { get; set; }
    }
}
