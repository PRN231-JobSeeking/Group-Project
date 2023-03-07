using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Models
{
    public class ApplicationModel
    {
        public int Id { get; set; }
        public int ApplicantId { get; set; }
        public int PostId { get; set; }
        public bool? Status { get; set; }
        public string CV { get; set; }
        public bool IsDeleted { get; set; }
    }
}
