using Helper.Constants;

namespace UseCase.MedicalExamination.GetHistory
{
    public class HistoryKarteOdrRaiinItem
    {
        public long RaiinNo { get; private set; }
        public int SinDate { get; private set; }
        public int HokenPid { get; private set; }
        public string HokenTitle { get; private set; }
        public string HokenRate { get; private set; }
        public int HokenType { get; private set; }

        /// <summary>
        /// hospital type come 
        /// </summary>
        public int SyosaisinKbn { get; private set; }
        public string SyosaisinDisplay { get => SyosaiConst.FlowSheetShinDict.FirstOrDefault(x => x.Key == SyosaisinKbn).Value; }

        /// <summary>
        /// time to hospital
        /// </summary>
        public int JikanKbn { get; private set; }
        public string JikanDisplay { get => JikanConst.JikanDict.FirstOrDefault(x => x.Key == JikanKbn).Value; }

        /// <summary>
        /// medical examination department
        /// </summary>
        public int KaId { get; private set; }
        public string KaName { get; private set; }

        /// <summary>
        /// doctor
        /// </summary>
        public int TantoId
        {
            get; private set;
        }
        public string TantoName { get; private set; }
        public int SanteiKbn { get; private set; }

        public string SanteiKbnDisplay { get => _jihiSanteiDict.FirstOrDefault(x => x.Key == SanteiKbn).Value; }

        private readonly Dictionary<int, string> _jihiSanteiDict = new Dictionary<int, string>()
        {
            {0,"－" },
            {2,"自費" }
        };

        /// <summary>
        /// raiin list tag
        /// </summary>
        public int TagNo { get; private set; }

        /// <summary>
        /// approve info
        /// </summary>
        public string SinryoTitle { get; private set; }

        public List<HokenGroupHistoryItem> HokenGroups { get; private set; }
        public List<GrpKarteHistoryItem> KarteHistories { get; private set; }

        public HistoryKarteOdrRaiinItem(long raiinNo, int sinDate, int hokenPid, string hokenTitle, string hokenRate, int syosaisinKbn, int jikanKbn, int kaId, string kaName, int tantoId, string tantoName, int santeiKbn, int tagNo, string sinryoTitle, int hokenType, List<HokenGroupHistoryItem> hokenGroups, List<GrpKarteHistoryItem> karteHistories)
        {
            RaiinNo = raiinNo;
            SinDate = sinDate;
            HokenPid = hokenPid;
            HokenTitle = hokenTitle;
            HokenRate = hokenRate;
            HokenGroups = hokenGroups;
            KarteHistories = karteHistories;
            SyosaisinKbn = syosaisinKbn;
            JikanKbn = jikanKbn;
            KaId = kaId;
            KaName = kaName;
            TantoId = tantoId;
            TantoName = tantoName;
            SanteiKbn = santeiKbn;
            HokenGroups = hokenGroups;
            KarteHistories = karteHistories;
            TagNo = tagNo;
            SinryoTitle = sinryoTitle;
            HokenType = hokenType;
        }
    }
}
