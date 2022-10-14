using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.SaveInsuranceMasterLinkage
{
    public class SaveInsuranceMasterLinkageOutputData : IOutputData
    {
        public SaveInsuranceMasterLinkageOutputData(SaveInsuranceMasterLinkageStatus status)
        {
            Status = status;
        }

        public SaveInsuranceMasterLinkageStatus Status { get; private set; }
    }
}
