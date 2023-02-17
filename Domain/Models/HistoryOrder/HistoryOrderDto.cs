using Domain.Models.KarteInfs;
using Domain.Models.OrdInfs;
using Helper.Constants;

namespace Domain.Models.HistoryOrder
{
    public class HistoryOrderDto
    {
        public long RaiinNo { get; private set; }

        public int SinDate { get; private set; }

        public int HokenPid { get; private set; }

        public string HokenTitle { get; private set; }

        public string HokenRate { get; private set; }

        public int HokenType { get; private set; }

        public int SyosaisinKbn { get; private set; }

        public int JikanKbn { get; private set; }

        public int KaId { get; private set; }

        public int TantoId { get; private set; }

        public string KaName { get; private set; }

        public string TantoName { get; private set; }

        public int SanteiKbn { get; private set; }

        public int TagNo { get; private set; }

        public string SinryoTitle { get; private set; }

        public List<OrdInfModel> OrderInfList { get; private set; }

        public List<KarteInfModel> KarteInfModels { get; private set; }

        public string SanteiKbnDisplay { get => _jihiSanteiDict.FirstOrDefault(x => x.Key == SanteiKbn).Value; }

        public string SyosaisinDisplay { get => SyosaiConst.ReceptionShinDict.FirstOrDefault(x => x.Key == SyosaisinKbn).Value; }

        public string JikanDisplay { get => JikanConst.JikanKotokuDict.FirstOrDefault(x => x.Key == JikanKbn).Value; }

        private readonly Dictionary<int, string> _jihiSanteiDict = new Dictionary<int, string>()
        {
            {0,"－" },
            {2,"自費" }
        };

        public HistoryOrderDto(long raiinNo, int sinDate, int hokenPid, string hokenTitle, string hokenRate, int hokenType, int syosaisinKbn, int jikanKbn, int kaId, int tantoId, string kaName, string tantoName, int santeiKbn, int tagNo, string sinryoTitle, List<OrdInfModel> orderInfList, List<KarteInfModel> karteInfModels)
        {
            RaiinNo = raiinNo;
            SinDate = sinDate;
            HokenPid = hokenPid;
            HokenTitle = hokenTitle;
            HokenRate = hokenRate;
            HokenType = hokenType;
            SyosaisinKbn = syosaisinKbn;
            JikanKbn = jikanKbn;
            KaId = kaId;
            TantoId = tantoId;
            KaName = kaName;
            TantoName = tantoName;
            SanteiKbn = santeiKbn;
            TagNo = tagNo;
            SinryoTitle = sinryoTitle;
            OrderInfList = orderInfList;
            KarteInfModels = karteInfModels;
        }
    }
}
