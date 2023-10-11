using Domain.Models.KensaCmtMst.cs;
using UseCase.Core.Sync.Core;
using UseCase.MstItem.SearchTenMstItem;

namespace UseCase.KensaHistory.GetListKensaCmtMst
{
    public class GetListKensaCmtMstOutputData : IOutputData
    {

        public GetListKensaCmtMstOutputData(List<KensaCmtMstModel> kensaCmtMsts, SearchTenMstItemStatus status)
        {
            KensaCmtMsts = kensaCmtMsts;
            Status = status;
        }

        public List<KensaCmtMstModel> KensaCmtMsts { get; private set; }
        public SearchTenMstItemStatus Status { get; private set; }
    }
}
