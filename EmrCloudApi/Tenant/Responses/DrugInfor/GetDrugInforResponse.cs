using Domain.Models.DrugInfor;

namespace EmrCloudApi.Tenant.Responses.DrugInfor
{
    public class GetDrugInforResponse
    {
        public GetDrugInforResponse(DrugInforModel drugInf)
        {
            DrugInf = drugInf;
        }

        public DrugInforModel DrugInf { get; private set; }
    }
}
