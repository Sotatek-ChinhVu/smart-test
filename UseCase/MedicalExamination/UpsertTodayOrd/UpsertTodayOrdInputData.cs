using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.UpsertTodayOrd
{
    public class UpsertTodayOrdInputData : IInputData<UpsertTodayOrdOutputData>
    {
        public UpsertTodayOrdInputData(int status, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, List<OdrInfItem> odrItems, List<KarteItem> karteInfs)
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
        }

        public int Status { get; private set; }
        public int SyosaiKbn { get; private set; }
        public int JikanKbn { get; private set; }
        public int HokenPid { get; private set; }
        public int SanteiKbn { get; private set; }
        public int TantoId { get; private set; }
        public int KaId { get; private set; }
        public List<OdrInfItem> OdrItems { get; private set; }
        public List<KarteItem> KarteInfs { get; private set; }
    }
}
