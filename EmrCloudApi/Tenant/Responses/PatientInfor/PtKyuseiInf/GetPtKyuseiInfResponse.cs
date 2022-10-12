using Domain.Models.PatientInfor.PtKyuseiInf;

namespace EmrCloudApi.Tenant.Responses.PatientInfor.PtKyuseiInf
{
    public class GetPtKyuseiInfResponse
    {
        public GetPtKyuseiInfResponse(List<PtKyuseiInfModel> ptKyuseiInfModels)
        {
            PtKyuseiInfModels = ptKyuseiInfModels;
        }

        public List<PtKyuseiInfModel> PtKyuseiInfModels { get; private set; }
    }
}
