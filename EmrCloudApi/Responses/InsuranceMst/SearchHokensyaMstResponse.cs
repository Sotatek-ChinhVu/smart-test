using Domain.Models.InsuranceMst;

namespace EmrCloudApi.Responses.InsuranceMst
{
    public class SearchHokensyaMstResponse
    {
        public List<HokensyaMstModel> ListData { get; private set; }

        public SearchHokensyaMstResponse(List<HokensyaMstModel> listData)
        {
            ListData = listData;
        }
    }
}
