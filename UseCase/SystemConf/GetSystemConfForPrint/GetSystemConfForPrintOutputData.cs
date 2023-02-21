using System.Collections;
using UseCase.Core.Sync.Core;

namespace UseCase.SystemConf.GetSystemConfForPrint
{
    public class GetSystemConfForPrintOutputData : IOutputData
    {
        public GetSystemConfForPrintOutputData(Hashtable values, GetSystemConfForPrintStatus status)
        {
            Values = values;
            Status = status;
        }

        public Hashtable Values { get; private set; }

        public GetSystemConfForPrintStatus Status { get; private set; }
    }
}
