using Domain.Models.KensaIrai;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetParrentKensaMstListResponse
    {
        public GetParrentKensaMstListResponse(List<KensaMstModel> datas)
        {
            Datas = datas;
        }

        public List<KensaMstModel> Datas { get; private set; }
    }
}
