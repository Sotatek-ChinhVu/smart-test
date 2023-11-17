using Domain.Models.CalculationInf;
using Domain.Models.GroupInf;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.MaxMoney;
using Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.CheckPtNum
{
    public class CheckPtNumInputData : IInputData<CheckPtNumOutputData>
    {
        public CheckPtNumInputData(int hpId, long ptNum)
        {
            HpId = hpId;
            PtNum = ptNum;
        }

        public int HpId { get; private set; }
        public long PtNum { get; private set; }
    }
}
