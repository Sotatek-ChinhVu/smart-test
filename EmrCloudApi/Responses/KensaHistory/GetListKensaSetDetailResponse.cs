using Domain.Models.KensaSet;
using Domain.Models.KensaSetDetail;

namespace EmrCloudApi.Responses.KensaHistory
{
    public class GetListKensaSetDetailResponse
    {
        public GetListKensaSetDetailResponse(List<KensaSetDetailModel> data)
        {
            Data = data;
        }

        public List<KensaSetDetailModel> Data { get; private set; }
    }
}
