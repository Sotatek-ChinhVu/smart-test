using Domain.Models.HokenMst;
using UseCase.Core.Sync.Core;

namespace UseCase.HokenMst.GetDetail
{
    public class GetDetailHokenMstOutputData : IOutputData
    {
        public HokenMasterModel? Data { get; private set; }
        public GetDetailHokenMstStatus Status { get; private set; }

        public GetDetailHokenMstOutputData(HokenMasterModel? data, GetDetailHokenMstStatus status)
        {
            Data = data;
            Status = status;
        }
    }
}
