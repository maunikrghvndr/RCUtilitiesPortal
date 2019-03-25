using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risarc.Enterprise.Repository.Helpers
{
    public static class ExceptionExtensions
    {
        public static string ErrorDetail(this Exception ex)
        {
            int num = 0;
            StringBuilder stringBuilder1 = new StringBuilder(string.Format("Base error: {0}. ", ex.Message));
            StringBuilder stringBuilder2 = new StringBuilder(string.Format("Base Stack Trace: {0}. ", ex.StackTrace));
            for (; ex.InnerException != null; ex = ex.InnerException)
            {
                ++num;
                stringBuilder1.AppendFormat("Inner Exception{0}: {1}. ", (object)num.ToString(), (object)ex.InnerException.Message);
                stringBuilder2.AppendFormat("Stack Trace{0}: {1}. ", (object)num.ToString(), (object)ex.InnerException.StackTrace);
            }

            return DateTime.Now.ToString() + " - " + stringBuilder1.ToString() + " ------- " + stringBuilder2.ToString();
        }
    }
}
