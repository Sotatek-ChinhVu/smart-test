using UseCase.Core.Sync.Core;

namespace UseCase.OrdInfs.ValidationTodayOrd
{
    public class ValidationTodayOrdInputData : IInputData<ValidationTodayOrdOutputData>
    {
        public ValidationTodayOrdInputData(int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, List<ValidationOdrInfItem> odrInfs, ValidationKarteItem karte)
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
            Karte = karte;
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
        public List<ValidationOdrInfItem> OdrInfs { get; private set; }
        public ValidationKarteItem Karte { get; private set; }

        public List<ValidationOdrInfItem> ToList()
        {
            return OdrInfs;
        }
    }
}
