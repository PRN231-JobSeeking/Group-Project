using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Models
{
    public class UserSkill
    {
        public int AccountId { get; set; }
        public int SkillId { get; set; }
        public virtual Skill? Skill { get; set; }
        public virtual AccountModel? Account { get; set; }
        public bool IsDeleted { get; set; }
    }
}
