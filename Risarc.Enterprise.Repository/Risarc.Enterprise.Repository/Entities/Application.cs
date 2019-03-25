using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risarc.Enterprise.Repository.Entities
{
    public class Application
    {
        [Key]
        public int AppID { get; set; }

        public string AppName { get; set; }

        public string AppPath { get; set; }

        public bool IsConsoleApp { get; set; }

        public string AssemblyPath { get; set; }

        public string ClassName { get; set; }

        public DateTime EnteredAt { get; set; }
    }
}
