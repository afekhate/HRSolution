using HRSolution.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Infrastructure.DTOs.Job
{
    public class JobSpecificationDTO 
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string code { get; set; }
    }
}
