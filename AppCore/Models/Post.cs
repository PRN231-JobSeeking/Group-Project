using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Models
{
    public class Post : IDeleted
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(Location))]
        public int LocationId { get; set; }
        [ForeignKey(nameof(Level))]
        public int LevelId { get; set; }
        public bool Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Amount { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Location? Location { get; set; }
        public virtual Level? Level { get; set; }
        public virtual ICollection<PostSkillRequired>? SkillRequired { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
