using Domain.Models.PtKyuseiInf;

namespace EmrCloudApi.Tenant.Responses.PtKyuseiInf
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
