using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risarc.Enterprise.Repository.Entities
{
    public class Server
    {
        [Key]
        public int ServerID { get; set; }

        public string ServerName { get; set; }

        public string ServerIP { get; set; }
    }
}
