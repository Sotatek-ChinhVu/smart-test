using Domain.Models.Insurance;

namespace EmrCloudApi.Responses.Schema
{
    public class GetListInsuranceScanResponse
    {
        public GetListInsuranceScanResponse(IEnumerable<InsuranceScanDto> datas)
        {
            Datas = datas;
        }

        public IEnumerable<InsuranceScanDto> Datas { get; private set; }
    }
}
