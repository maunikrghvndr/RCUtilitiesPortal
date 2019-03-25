using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risarc.Enterprise.Repository.Entities
{
    public class JobLog
    {
        [Key]
        public int JobLogID { get; set; }

        public int JobID { get; set; }

        public string LogDetail { get; set; }

        public DateTime LogDateTime { get; set; }
    }
}
