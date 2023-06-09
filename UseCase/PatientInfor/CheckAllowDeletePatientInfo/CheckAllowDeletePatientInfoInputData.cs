using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.CheckAllowDeletePatientInfo
{
    public class CheckAllowDeletePatientInfoInputData : IInputData<CheckAllowDeletePatientInfoOutputData>
    {
        public CheckAllowDeletePatientInfoInputData(int hpId, long ptId)
        {
            HpId = hpId;
            PtId = ptId;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }
    }
}
