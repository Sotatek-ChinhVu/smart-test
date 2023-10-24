using Domain.Models.KensaIrai;
using UseCase.Core.Sync.Core;
using UseCase.MstItem.SearchPostCode;

namespace UseCase.KensaHistory.GetListKensaCmtMst.GetKensaInfDetailByIraiCd
{
    public class GetKensaInfDetailByIraiCdOutputData : IOutputData
    {
        public GetKensaInfDetailByIraiCdOutputData(List<ListKensaInfDetailItemModel> listKensaInfDetails, SearchPostCodeStatus status)
        {
            KensaInfDetails = listKensaInfDetails;
            Status = status;
        }

        public List<ListKensaInfDetailItemModel> KensaInfDetails { get; private set; }
        public SearchPostCodeStatus Status { get; private set; }
    }
}
