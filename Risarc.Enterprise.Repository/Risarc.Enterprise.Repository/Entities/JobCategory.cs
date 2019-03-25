using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risarc.Enterprise.Repository.Entities
{
    public class JobCategory
    {
        [Key]
        public int JobCategoryID { get; set; }

        public string Description { get; set; }
    }
}
