using Domain.Models.KensaIrai;
using Entity.Tenant;
using UseCase.Core.Sync.Core;
using UseCase.MstItem.SearchPostCode;

namespace UseCase.KensaHistory.GetListKensaInfDetail
{
    public class GetListKensaInfDetailOutputData : IOutputData
    {
        public GetListKensaInfDetailOutputData(ListKensaInfDetailModel listKensaInfDetails, SearchPostCodeStatus status)
        {
            ListKensaInfDetails = listKensaInfDetails;
            Status = status;
        }

        public ListKensaInfDetailModel ListKensaInfDetails { get; private set; }
        public SearchPostCodeStatus Status { get; private set; }
    }
}
