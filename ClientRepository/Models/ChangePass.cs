using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Models
{
    public class ChangePass
    {
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Password { get; set; } = null!;
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string NewPassword { get; set; } = null!;
        [Required]
        [Compare(nameof(NewPassword))]
        [StringLength(20, MinimumLength = 2)]
        public string ConfirmPassword { get; set; } = null!;
    }
}
