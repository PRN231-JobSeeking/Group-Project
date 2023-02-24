using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Models
{
    public class Interview : IDeleted
    {
        [ForeignKey(nameof(Application))]
        public int ApplicationId { get; set; }
        [ForeignKey(nameof(Interviewer))]
        public int InterviewerId { get; set; }
        [ForeignKey(nameof(Slot))]
        public int SlotId { get; set; }
        public int Round { get; set; }
        [Column(TypeName ="date")]
        public DateTime Date { get; set; }
        public string? Feedback { get; set; }    
        public double Point { get; set; }

        public virtual Application? Application { get; set; }
        public virtual Account? Interviewer { get; set; }
        public virtual Slot? Slot { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
