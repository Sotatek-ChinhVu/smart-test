using Domain.Models.KensaSet;
using UseCase.Core.Sync.Core;
using UseCase.MstItem.SearchTenMstItem;

namespace UseCase.KensaHistory.GetListKensaSet
{
    public class GetListKensaSetOutputData : IOutputData
    {
        public GetListKensaSetOutputData(List<KensaSetModel> kensaSets, SearchTenMstItemStatus status)
        {
            KensaSets = kensaSets;
            Status = status;
        }

        public List<KensaSetModel> KensaSets { get; private set; }
        public SearchTenMstItemStatus Status { get; private set; }
    }
}
