using Domain.Models.OrdInfs;

namespace EmrCloudApi.Responses.OrdInfs
{
    public class GetHeaderInfResponse
    {
        public GetHeaderInfResponse(int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, int tagNo, OrdInfModel odrInfs)
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
            TagNo = tagNo;
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

        public int TagNo { get; private set; }

        public OrdInfModel OdrInfs { get; private set; }
    }
}
