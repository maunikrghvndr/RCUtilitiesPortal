using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risarc.Enterprise.Repository.Entities
{
    public class Job
    {
        [Key]
        public int JobID { get; set; }

        public string JobName { get; set; }

        public string JobDescription { get; set; }

        public string JobParameters { get; set; }

        public int AppID { get; set; }

        public DateTime EnteredAt { get; set; }

        public int? JobProcessID { get; set; }

        public string LastStatusDetail { get; set; }

        public DateTime? LastStatusTime { get; set; }

        public int? JobStatusTypeID { get; set; }

        public bool Active { get; set; }

        public int? JobAsyncTaskID { get; set; }

        public bool ActivateTransaction { get; set; }

        public virtual List<JobLog> JobLogs { get; set; }

        public virtual Application JobApp { get; set; }

        public virtual JobStatusType JobStatusTypes { get; set; }

        public virtual List<JobServer> JobServers { get; set; }

        public int JobCategoryID { get; set; }
    }
}
