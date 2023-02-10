using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Models
{
    public class Application
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey(nameof(Applicant))]
        public int ApplicantId { get; set; }
        [ForeignKey(nameof(Post))] 
        public int PostId { get; set; }
        public bool Status { get; set; }
        public string CV { get; set; } = null!;


        public virtual Account? Applicant { get; set; }
        public virtual Post? Post { get; set; }
    }
}
