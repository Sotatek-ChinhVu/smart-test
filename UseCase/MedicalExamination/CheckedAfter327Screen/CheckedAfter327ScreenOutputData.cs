using Domain.Models.Medical;
using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.CheckedAfter327Screen
{
    public class CheckedAfter327ScreenOutputData : IOutputData
    {
        public CheckedAfter327ScreenOutputData(CheckedAfter327ScreenStatus status, string message, List<SinKouiCountModel> sinKouiCountModels)
        {
            Status = status;
            Message = message;
            SinKouiCountModels = sinKouiCountModels;
        }

        public CheckedAfter327ScreenStatus Status { get; private set; }

        public string Message { get; private set; }

        public List<SinKouiCountModel> SinKouiCountModels { get; private set; }
    }
}
