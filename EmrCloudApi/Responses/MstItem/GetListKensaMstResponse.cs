using Domain.Models.KensaIrai;
using Domain.Models.MstItem;
using Entity.Tenant;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetListKensaMstResponse
    {
        public GetListKensaMstResponse(List<KensaMstModel> kensaMsts, int totalCount)
        {
            KensaMsts = kensaMsts;
            TotalCount = totalCount;
        }

        public List<KensaMstModel> KensaMsts { get; private set; }
        public int TotalCount { get; private set; }
    }
}
