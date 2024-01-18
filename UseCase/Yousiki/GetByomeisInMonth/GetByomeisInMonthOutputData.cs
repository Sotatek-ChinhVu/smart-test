using Domain.Models.Diseases;
using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.GetByomeisInMonth
{
    public class GetByomeisInMonthOutputData : IOutputData
    {
        public GetByomeisInMonthOutputData(List<PtDiseaseModel> byomeisInMonth, GetByomeisInMonthStatus status)
        {
            ByomeisInMonth = byomeisInMonth;
            Status = status;
        }

        public List<PtDiseaseModel> ByomeisInMonth { get; private set; }

        public GetByomeisInMonthStatus Status { get; private set; }
    }
}
