using Domain.Models.KensaIrai;
using Domain.Models.MstItem;

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
