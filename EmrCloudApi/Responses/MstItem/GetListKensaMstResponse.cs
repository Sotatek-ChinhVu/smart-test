using Domain.Models.KensaIrai;
using Domain.Models.MstItem;
using Entity.Tenant;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetListKensaMstResponse
    {
        public GetListKensaMstResponse(List<KensaMstModel> kensaMsts)
        {
            KensaMsts = kensaMsts;
        }

        public List<KensaMstModel> KensaMsts { get; private set; }
    }
}
