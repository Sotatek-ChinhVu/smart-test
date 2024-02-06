using Domain.Models.Yousiki;
using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.GetYousiki1InfDetailsByCodeNo
{
    public class GetYousiki1InfDetailsByCodeNoOutputData : IOutputData
    {
        public GetYousiki1InfDetailsByCodeNoOutputData(List<Yousiki1InfDetailModel> yousiki1InfDetailList, GetYousiki1InfDetailsByCodeNoStatus status)
        {
            Yousiki1InfDetailList = yousiki1InfDetailList;
            Status = status;
        }

        public List<Yousiki1InfDetailModel> Yousiki1InfDetailList { get; private set; }

        public GetYousiki1InfDetailsByCodeNoStatus Status { get; private set; }
    }
}
