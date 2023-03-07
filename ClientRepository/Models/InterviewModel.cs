using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Models
{
    public class InterviewModel
    {
        public int ApplicationId { get; set; }
        public int InterviewerId { get; set; }
        public int SlotId { get; set; }
        public int Round { get; set; }
        public DateTime Date { get; set; }
        public string? Feedback { get; set; }
        public double Point { get; set; }

        public ApplicationModel? Application { get; set; }
        public AccountModel? Interviewer { get; set; }
        public SlotModel? Slot { get; set; }

        public bool IsDeleted { get; set; }

    }
}
