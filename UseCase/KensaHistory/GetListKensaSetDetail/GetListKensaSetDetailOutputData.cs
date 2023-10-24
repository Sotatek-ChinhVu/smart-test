using Domain.Models.KensaSet;
using Domain.Models.KensaSetDetail;
using UseCase.Core.Sync.Core;
using UseCase.MstItem.SearchTenMstItem;

namespace UseCase.KensaHistory.GetListKensaSetDetail
{
    public class GetListKensaSetDetailOutputData : IOutputData
    {
        public GetListKensaSetDetailOutputData(List<KensaSetDetailModel> kensaSetDetails, SearchTenMstItemStatus status)
        {
            KensaSetDetails = kensaSetDetails;
            Status = status;
        }

        public List<KensaSetDetailModel> KensaSetDetails { get; private set; }
        public SearchTenMstItemStatus Status { get; private set; }
    }
}
