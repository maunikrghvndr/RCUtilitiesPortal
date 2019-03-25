using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risarc.Enterprise.Repository.Entities
{
    public class FileImportingJob
    {
        [Key]
        public int FileImportingJobID { get; set; }

        public string IdentifyingPath { get; set; }

        public string IdentifyingFileName { get; set; }

        public int JobID { get; set; }

        public string ImportsFromDirectory { get; set; }
    }
}
