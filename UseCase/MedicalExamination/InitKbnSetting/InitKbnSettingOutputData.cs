using Domain.Models.RaiinKubunMst;
using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.InitKbnSetting
{
    public class InitKbnSettingOutputData : IOutputData
    {
        public InitKbnSettingOutputData(InitKbnSettingStatus status, List<RaiinKbnModel> raiinKbnModels)
        {
            Status = status;
            RaiinKbnModels = raiinKbnModels;
        }

        public InitKbnSettingStatus Status { get; private set; }

        public List<RaiinKbnModel> RaiinKbnModels { get; private set; }
    }
}
