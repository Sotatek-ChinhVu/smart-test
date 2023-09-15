using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetKensaStdMst
{
    public class GetKensaStdMstOutputData : IOutputData
    {
        public GetKensaStdMstOutputData(List<KensaStdMstModel> kensaStdMsts, GetKensaStdMstIStatus status)
        {
            KensaStdMsts = kensaStdMsts;
            Status = status;
        }

        public List<KensaStdMstModel> KensaStdMsts { get; private set; }

        public GetKensaStdMstIStatus Status { get; private set; }
    }
}
