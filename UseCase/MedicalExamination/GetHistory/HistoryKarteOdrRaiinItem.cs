﻿using Helper.Constants;
using System.Text.Json.Serialization;

namespace UseCase.MedicalExamination.GetHistory
{
    public class HistoryKarteOdrRaiinItem
    {
        [JsonPropertyName("raiinNo")]
        public long RaiinNo { get; private set; }

        [JsonPropertyName("sinDate")]
        public int SinDate { get; private set; }

        [JsonPropertyName("hokenPid")]
        public int HokenPid { get; private set; }

        [JsonPropertyName("hokenTitle")]
        public string HokenTitle { get; private set; }

        [JsonPropertyName("hokenRate")]
        public string HokenRate { get; private set; }

        [JsonPropertyName("hokenType")]
        public int HokenType { get; private set; }

        /// <summary>
        /// hospital type come 
        /// </summary>
        [JsonPropertyName("syosaisinKbn")]
        public int SyosaisinKbn { get; private set; }

        [JsonPropertyName("syosaisinDisplay")]
        public string SyosaisinDisplay { get => SyosaiConst.ReceptionShinDict.FirstOrDefault(x => x.Key == SyosaisinKbn).Value; }

        /// <summary>
        /// time to hospital
        /// </summary>
        [JsonPropertyName("jikanKbn")]
        public int JikanKbn { get; private set; }

        [JsonPropertyName("jikanDisplay")]
        public string JikanDisplay { get => JikanConst.JikanKotokuDict.FirstOrDefault(x => x.Key == JikanKbn).Value; }

        /// <summary>
        /// medical examination department
        /// </summary>
        [JsonPropertyName("kaId")]
        public int KaId { get; private set; }

        [JsonPropertyName("kaName")]
        public string KaName { get; private set; }

        /// <summary>
        /// doctor
        /// </summary>

        [JsonPropertyName("tantoId")]
        public int TantoId
        {
            get; private set;
        }

        [JsonPropertyName("tantoName")]
        public string TantoName { get; private set; }

        [JsonPropertyName("santeiKbn")]
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
        [JsonPropertyName("tagNo")]
        public int TagNo { get; private set; }

        /// <summary>
        /// approve info
        /// </summary>
        [JsonPropertyName("sinryoTitle")]
        public string SinryoTitle { get; private set; }

        [JsonPropertyName("status")]
        public int Status { get; private set; }

        [JsonPropertyName("uketukeTime")]
        public string UketukeTime { get; set; }

        [JsonPropertyName("uketsukeName")]
        public string UketsukeName { get; set; }

        [JsonPropertyName("sinStartTime")]
        public string SinStartTime { get; set; }

        [JsonPropertyName("sinEndTime")]
        public string SinEndTime { get; set; }

        [JsonPropertyName("hokenGroups")]
        public List<HokenGroupHistoryItem> HokenGroups { get; private set; }

        [JsonPropertyName("karteHistories")]
        public List<GrpKarteHistoryItem> KarteHistories { get; private set; }

        [JsonPropertyName("listKarteFiles")]
        public List<FileInfOutputItem> ListKarteFiles { get; private set; }

        public HistoryKarteOdrRaiinItem(long raiinNo, int sinDate, int hokenPid, string hokenTitle, string hokenRate, int syosaisinKbn, int jikanKbn, int kaId, string kaName, int tantoId, string tantoName, int santeiKbn, int tagNo, string sinryoTitle, int hokenType, List<HokenGroupHistoryItem> hokenGroups, List<GrpKarteHistoryItem> karteHistories, List<FileInfOutputItem> listKarteFiles, int status, string uketukeTime, string uketsukeName, string sinStartTime, string sinEndTime)
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
            ListKarteFiles = listKarteFiles;
            Status = status;
            UketukeTime = uketukeTime;
            UketsukeName = uketsukeName;
            SinStartTime = sinStartTime;
            SinEndTime = sinEndTime;
        }
    }
}
