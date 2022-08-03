using Domain.Models.InsuranceMst;

namespace EmrCloudApi.Tenant.Responses.InsuranceMst
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
