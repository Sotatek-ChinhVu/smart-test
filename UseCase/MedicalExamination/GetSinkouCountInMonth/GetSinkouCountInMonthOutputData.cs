using Domain.Models.Medical;
using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetSinkouCountInMonth
{
    public class GetSinkouCountInMonthOutputData : IOutputData
    {
        public GetSinkouCountInMonthOutputData(GetSinkouCountInMonthStatus status, List<SinKouiCountModel> sinKouiCounts)
        {
            Status = status;
            SinKouiCounts = sinKouiCounts;
        }

        public GetSinkouCountInMonthStatus Status { get; private set; }
        public List<SinKouiCountModel> SinKouiCounts { get; private set; }
    }
}
