using Domain.Models.KensaSet;
using EmrCloudApi.Responses.MstItem;
using Entity.Tenant;

namespace EmrCloudApi.Responses.KensaHistory
{
    public class GetListKensaSetResponse
    {
        public GetListKensaSetResponse(List<KensaSetModel> data)
        {
            Data = data;
        }

        public List<KensaSetModel> Data { get; private set; }
    }
}
