using Domain.Models.PatientInfor;

namespace EmrCloudApi.Responses.PatientInfor.PtKyuseiInf
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
