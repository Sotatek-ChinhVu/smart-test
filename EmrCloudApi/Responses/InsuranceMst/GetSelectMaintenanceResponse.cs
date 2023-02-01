using Domain.Models.InsuranceMst;

namespace EmrCloudApi.Responses.InsuranceMst
{
    public class GetSelectMaintenanceResponse
    {
        public IEnumerable<SelectMaintenanceModel> Datas { get; private set; }

        public GetSelectMaintenanceResponse(IEnumerable<SelectMaintenanceModel> datas)
        {
            Datas = datas;
        }
    }
}
