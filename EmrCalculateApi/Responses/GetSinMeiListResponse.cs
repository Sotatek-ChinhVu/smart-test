using EmrCalculateApi.Receipt.Models;

namespace EmrCalculateApi.Responses
{
    public class GetSinMeiListResponse
    {
        public List<SinMeiDataModel> SinMeiList { get; private set; }
        
        public GetSinMeiListResponse(List<SinMeiDataModel> sinMeiList)
        {
            SinMeiList = sinMeiList;
        }
    }
}
