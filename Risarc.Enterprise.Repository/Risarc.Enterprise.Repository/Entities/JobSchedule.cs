using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risarc.Enterprise.Repository.Entities
{
    public class JobSchedule
    {
        [Key]
        public int JobScheduleID { get; set; }

        public int JobID { get; set; }

        public int IntervalInMilliseconds { get; set; }

        public string RunDays { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? NextStartDateTime { get; set; }
    }
}
