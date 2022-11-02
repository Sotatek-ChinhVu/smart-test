using UseCase.Core.Sync.Core;
using static Helper.Constants.DefHokenNoConst;

namespace UseCase.PatientInfor.SaveInsuranceMasterLinkage
{
    public class SaveInsuranceMasterLinkageOutputData : IOutputData
    {
        public SaveInsuranceMasterLinkageOutputData(ValidationStatus status)
        {
            Status = status;
        }

        public ValidationStatus Status { get; private set; }
    }
}
