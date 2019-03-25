using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risarc.Enterprise.Repository.Constants
{
    public enum JobStatusTypes
    {
        ReadyToRun = 1,
        RunRequestSent = 2,
        Running = 3,
        StopRequestSent = 4,
        Stopped = 5,
        StoppedWithError = 6,
    }
}
