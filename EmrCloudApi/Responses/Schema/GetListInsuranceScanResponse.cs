using Domain.Models.Insurance;

namespace EmrCloudApi.Responses.Schema
{
    public class GetListInsuranceScanResponse
    {
        public GetListInsuranceScanResponse(List<InsuranceScanDto> datas)
        {
            Datas = datas;
        }

        public List<InsuranceScanDto> Datas { get; private set; }
    }
}
