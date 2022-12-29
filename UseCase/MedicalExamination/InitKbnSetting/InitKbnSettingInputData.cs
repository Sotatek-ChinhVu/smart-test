using Helper.Enum;
using UseCase.Core.Sync.Core;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace UseCase.MedicalExamination.InitKbnSetting
{
    public class InitKbnSettingInputData : IInputData<InitKbnSettingOutputData>
    {
        public InitKbnSettingInputData(int hpId, WindowType windowType, int frameId, bool isEnableLoadDefaultVal, long ptId, long raiinNo, int sinDate, List<OdrInfItemInputData> odrInfs)
        {
            HpId = hpId;
            WindowType = windowType;
            FrameId = frameId;
            IsEnableLoadDefaultVal = isEnableLoadDefaultVal;
            PtId = ptId;
            RaiinNo = raiinNo;
            SinDate = sinDate;
            OdrInfs = odrInfs;
        }

        public int HpId { get; private set; }

        public WindowType WindowType { get; private set; }

        public int FrameId { get; private set; }

        public bool IsEnableLoadDefaultVal { get; private set; }

        public long PtId { get; private set; }

        public long RaiinNo { get; private set; }

        public int SinDate { get; private set; }

        public List<OdrInfItemInputData> OdrInfs { get; private set; }
    }
}
