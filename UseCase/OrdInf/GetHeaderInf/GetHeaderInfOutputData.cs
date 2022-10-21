using Domain.Models.OrdInfs;
using UseCase.Core.Sync.Core;

namespace UseCase.OrdInfs.GetHeaderInf
{
    public class GetHeaderInfOutputData : IOutputData
    {
        public GetHeaderInfOutputData(int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, int tagNo, OrdInfModel odrInfs, GetHeaderInfStatus status)
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
            TagNo = tagNo;
        }
        public GetHeaderInfOutputData(GetHeaderInfStatus status)
        {
            SyosaiKbn = 0;
            JikanKbn = 0;
            HokenPid = 0;
            SanteiKbn = 0;
            TantoId = 0;
            KaId = 0;
            UketukeTime = string.Empty;
            SinStartTime = string.Empty;
            SinEndTime = string.Empty;
            TagNo = 0;
            OdrInfs = new OrdInfModel();
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

        public int TagNo { get; private set; }

        public OrdInfModel OdrInfs { get; private set; }

        public GetHeaderInfStatus Status { get; private set; }

    }
}
