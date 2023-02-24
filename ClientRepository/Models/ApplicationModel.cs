﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public bool Status { get; set; }
        public string CV { get; set; } = null!;


        public AccountModel? Applicant { get; set; }
        public PostModel? Post { get; set; }
    }
}
