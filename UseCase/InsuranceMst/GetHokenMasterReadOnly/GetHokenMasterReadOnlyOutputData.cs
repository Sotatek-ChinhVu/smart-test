using Domain.Models.InsuranceMst;
using UseCase.Core.Sync.Core;

namespace UseCase.InsuranceMst.GetHokenMasterReadOnly
{
    public class GetHokenMasterReadOnlyOutputData : IOutputData
    {
        public GetHokenMasterReadOnlyOutputData(GetHokenMasterReadOnlyStatus status, HokenMstModel hokenMaster)
        {
            Status = status;
            HokenMaster = hokenMaster;
        }

        public GetHokenMasterReadOnlyStatus Status { get; private set; }

        public HokenMstModel HokenMaster { get; private set; }
    }
}
