using Domain.Models.PatientInfor.PtKyuseiInf;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.PtKyuseiInf.GetList
{
    public class GetPtKyuseiInfOutputData : IOutputData
    {
        public GetPtKyuseiInfOutputData(List<PtKyuseiInfModel> ptKyuseiInfModels, GetPtKyuseiInfStatus status)
        {
            PtKyuseiInfModels = ptKyuseiInfModels;
            Status = status;
        }

        public List<PtKyuseiInfModel> PtKyuseiInfModels { get; private set; }
        public GetPtKyuseiInfStatus Status { get; private set; }
    }
}
