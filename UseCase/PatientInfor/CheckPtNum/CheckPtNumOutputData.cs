using Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.CheckPtNum
{
    public class CheckPtNumOutputData : IOutputData
    {
        public CheckPtNumOutputData(long ptNum)
        {
            PtNum = ptNum;
        }

        public long PtNum { get; private set; }
    }
}
