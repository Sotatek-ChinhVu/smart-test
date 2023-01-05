using Helper.Enum;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class InitKbnSettingRequest
    {
        public WindowType WindowType { get; set; }

        public int FrameId { get; set; }

        public bool IsEnableLoadDefaultVal { get; set; }

        public long PtId { get; set; }

        public long RaiinNo { get; set; }

        public int SinDate { get; set; }

        public List<OdrInfItemInputData> OdrInfs { get; set; } = new();
    }
}
