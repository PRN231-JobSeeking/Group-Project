using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Models
{
    public class UserSkill
    {
        [ForeignKey(nameof(Account))]
        public int AccountId { get; set; }
        [ForeignKey(nameof(Skill))]
        public int SkillId { get; set; }
        public virtual Skill? Skill { get; set; }
        public virtual Account? Account { get; set; }
    }
}
