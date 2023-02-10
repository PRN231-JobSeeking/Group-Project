using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Models
{
    public class PostSkillRequired
    {
        [ForeignKey(nameof(Post))]
        public int PostId { get; set; }
        [ForeignKey(nameof(Skill))]
        public int SkillId { get; set; }
        public virtual Skill? Skill { get; set; }
        public virtual Post? Post { get; set; }
    }
}
