using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetRenkeiMstResponse
    {
        public GetRenkeiMstResponse(RenkeiMstModel renkeiMst)
        {
            RenkeiMst = renkeiMst;
        }

        public RenkeiMstModel RenkeiMst { get; private set; }
    }
}
