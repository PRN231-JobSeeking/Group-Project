using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Amount { get; set; }

        public CategoryModel Category { get; set; }
        public LocationModel Location { get; set; }
        public LevelModel Level { get; set; }
        public ICollection<PostSkillModel>? SkillRequired { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
