using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.UpsertTodayOrd
{
    public class UpsertTodayOrdInputData : IInputData<UpsertTodayOrdOutputData>
    {
        public UpsertTodayOrdInputData(int status, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, List<OdrInfItemInputData> odrItems, List<KarteItemInputData> karteInfs)
        {
            Status = status;
            SyosaiKbn = syosaiKbn;
            JikanKbn = jikanKbn;
            HokenPid = hokenPid;
            SanteiKbn = santeiKbn;
            TantoId = tantoId;
            KaId = kaId;
            OdrItems = odrItems;
            KarteInfs = karteInfs;
            UketukeTime = uketukeTime;
            SinStartTime = sinStartTime;
            SinEndTime = sinEndTime;
        }

        public int Status { get; private set; }
        public int SyosaiKbn { get; private set; }
        public int JikanKbn { get; private set; }
        public int HokenPid { get; private set; }
        public int SanteiKbn { get; private set; }
        public int TantoId { get; private set; }
        public int KaId { get; private set; }
        public string UketukeTime { get; private set; }
        public string SinStartTime { get; private set; }
        public string SinEndTime { get; private set; }
        public List<OdrInfItemInputData> OdrItems { get; private set; }
        public List<KarteItemInputData> KarteInfs { get; private set; }
    }
}
