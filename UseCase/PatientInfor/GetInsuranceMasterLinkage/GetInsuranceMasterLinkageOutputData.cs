using Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.GetInsuranceMasterLinkage
{
    public class GetInsuranceMasterLinkageOutputData : IOutputData
    {
        public GetInsuranceMasterLinkageOutputData(List<DefHokenNoModel> defHokenNoModels, GetInsuranceMasterLinkageStatus status)
        {
            DefHokenNoModels = defHokenNoModels;
            Status = status;
        }

        public List<DefHokenNoModel> DefHokenNoModels { get; private set; }

        public GetInsuranceMasterLinkageStatus Status { get; private set; }
    }
}
