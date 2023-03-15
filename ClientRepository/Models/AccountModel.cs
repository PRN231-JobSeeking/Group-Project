using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Models
{
    public class AccountModel
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(60, MinimumLength = 2)]
        public string Email { get; set; } = null!;
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Password { get; set; } = null!;
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string FirstName { get; set; } = null!;
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string LastName { get; set; } = null!;
        [Required]
        [StringLength(80, MinimumLength = 2)]
        public string Address { get; set; } = null!;
        [DataType(DataType.PhoneNumber)]
        [StringLength(12, MinimumLength = 9)]
        public string Phone { get; set; } = null!;
        public bool IsLockout { get; set; }
        public int RoleId { get; set; }
        public virtual Role? Role { get; set; }
        public virtual ICollection<UserSkill>? UserSkill { get; set; }
        public bool IsDeleted { get; set; }
    }
}
