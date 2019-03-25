using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risarc.Enterprise.Repository.Entities
{
    public class DependantJob
    {
        [Key]
        public int ID { get; set; }

        public int ParentJobID { get; set; }

        public int DependantJobID { get; set; }

        public string RunTimeParams { get; set; }

        public bool ParentRan { get; set; }

        public int? DependancyGroup { get; set; }

        public int? DependancyOrder { get; set; }
    }
}
