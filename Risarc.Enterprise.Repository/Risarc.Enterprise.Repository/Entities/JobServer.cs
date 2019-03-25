using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risarc.Enterprise.Repository.Entities
{
    public class JobServer
    {
        [Key]
        public int JobServerID { get; set; }

        public int JobID { get; set; }

        public int ServerID { get; set; }

        public int MaxInstances { get; set; }

        public int CurrentInstances { get; set; }
    }
}
