using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Models
{
    public class InterviewFeedbackModel
    {
        public int ApplicationId { get; set; }
        public int InterviewerId { get; set; }

        public int SlotId { get; set; }
        public int Round { get; set; }
        public string? Feedback { get; set; }
        public double Point { get; set; } = 0;
    }
}
