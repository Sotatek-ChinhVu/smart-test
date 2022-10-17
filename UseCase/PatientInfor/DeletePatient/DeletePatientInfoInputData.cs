using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.DeletePatient
{
    public class DeletePatientInfoInputData : IInputData<DeletePatientInfoOutputData>
    {
        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public DeletePatientInfoInputData(int hpId, long ptId)
        {
            HpId = hpId;
            PtId = ptId;
        }
    }
}

