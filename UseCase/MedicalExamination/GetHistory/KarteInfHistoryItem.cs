using System.Text.Json.Serialization;

namespace UseCase.MedicalExamination.GetHistory
{
    public class KarteInfHistoryItem
    {
        public KarteInfHistoryItem(int hpId, long raiinNo, int karteKbn, long seqNo, long ptId, int sinDate, string text, DateTime updateDate, DateTime createDate, int isDeleted, string richText, string createName)
        {
            HpId = hpId;
            RaiinNo = raiinNo;
            KarteKbn = karteKbn;
            SeqNo = seqNo;
            PtId = ptId;
            SinDate = sinDate;
            Text = text;
            UpdateDate = updateDate;
            CreateDate = createDate;
            IsDeleted = isDeleted;
            RichText = richText;
            CreateName = createName;
        }

        [JsonPropertyName("hpId")]
        public int HpId { get; private set; }

        [JsonPropertyName("raiinNo")]
        public long RaiinNo { get; private set; }

        [JsonPropertyName("karteKbn")]
        public int KarteKbn { get; private set; }

        [JsonPropertyName("seqNo")]
        public long SeqNo { get; private set; }

        [JsonPropertyName("ptId")]
        public long PtId { get; private set; }

        [JsonPropertyName("sinDate")]
        public int SinDate { get; private set; }

        [JsonPropertyName("text")]
        public string Text { get; private set; }

        [JsonPropertyName("richText")]
        public string RichText { get; private set; }

        [JsonPropertyName("updateDate")]
        public DateTime UpdateDate { get; private set; }

        [JsonPropertyName("createDate")]
        public DateTime CreateDate { get; private set; }

        [JsonPropertyName("isDeleted")]
        public int IsDeleted { get; private set; }

        [JsonPropertyName("createName")]
        public string CreateName { get; private set; }

        [JsonPropertyName("updateDateDisplay")]
        public string UpdateDateDisplay
        {
            get => UpdateDate.ToString("yyyy/MM/dd HH:mm");
        }

        [JsonPropertyName("createDateDisplay")]
        public string CreateDateDisplay
        {
            get => CreateDate.ToString("yyyy/MM/dd HH:mm");
        }
    }
}
