using Domain.Models.ReceSeikyu;

namespace EmrCloudApi.Responses.ReceSeikyu
{
    public class GetListReceSeikyuResponse
    {
        public GetListReceSeikyuResponse(List<ReceSeikyuModel> datas)
        {
            Datas = datas;
        }

        public List<ReceSeikyuModel> Datas { get; private set; }
    }
}
