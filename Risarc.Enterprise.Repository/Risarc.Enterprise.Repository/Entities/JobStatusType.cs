using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risarc.Enterprise.Repository.Entities
{
    public class JobStatusType
    {
        [Key]
        public int JobStatusTypeID { get; set; }

        public string JobStatus { get; set; }

        public string JobStatusDesc { get; set; }
    }
}
