using Domain.Models.KensaIrai;
using Entity.Tenant;
using UseCase.Core.Sync.Core;
using UseCase.MstItem.SearchPostCode;

namespace UseCase.KensaHistory.GetListKensaInfDetail
{
    public class GetListKensaInfDetailOutputData : IOutputData
    {
        public GetListKensaInfDetailOutputData(List<ListKensaInfDetailModel> listKensaInfDetails, SearchPostCodeStatus status, int totalCount)
        {
            ListKensaInfDetails = listKensaInfDetails;
            Status = status;
            TotalCount = totalCount;
        }

        public List<ListKensaInfDetailModel>  ListKensaInfDetails{ get; private set; }
        public SearchPostCodeStatus Status { get; private set; }
        public int TotalCount { get; private set; }
    }
}
