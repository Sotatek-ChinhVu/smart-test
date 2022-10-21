using UseCase.Core.Sync.Core;
using UseCase.MedicalExamination.UpsertTodayOrd;
using UseCase.OrdInfs.ValidationTodayOrd;

namespace UseCase.OrdInfs.CheckedSpecialItem
{
    public class CheckedSpecialItemInputData : IInputData<CheckedSpecialItemOrdOutputData>
    {
        public CheckedSpecialItemInputData(int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, List<OdrInfItemInputData> odrInfs, CheckedSpecialItemStatus status)
        {
            SyosaiKbn = syosaiKbn;
            JikanKbn = jikanKbn;
            HokenPid = hokenPid;
            SanteiKbn = santeiKbn;
            TantoId = tantoId;
            KaId = kaId;
            UketukeTime = uketukeTime;
            SinStartTime = sinStartTime;
            SinEndTime = sinEndTime;
            OdrInfs = odrInfs;
            Status = status;
        }

        public int SyosaiKbn { get; private set; }
        public int JikanKbn { get; private set; }
        public int HokenPid { get; private set; }
        public int SanteiKbn { get; private set; }
        public int TantoId { get; private set; }
        public int KaId { get; private set; }
        public string UketukeTime { get; private set; }
        public string SinStartTime { get; private set; }
        public string SinEndTime { get; private set; }
        public List<OdrInfItemInputData> OdrInfs { get; private set; }
        public CheckedSpecialItemStatus Status { get; private set; }
    }
}
