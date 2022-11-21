using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.DeletePatient
{
    public class DeletePatientInfoInputData : IInputData<DeletePatientInfoOutputData>
    {
        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int UserId { get; private set; }

        public DeletePatientInfoInputData(int hpId, long ptId, int userId)
        {
            HpId = hpId;
            PtId = ptId;
            UserId = userId;
        }
    }
}

