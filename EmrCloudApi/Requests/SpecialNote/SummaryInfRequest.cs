using UseCase.SpecialNote.Save;

namespace EmrCloudApi.Requests.SpecialNote
{
    public class SummaryInfRequest
    {
        public long Id { get; set; }

        public int HpId { get; set; }

        public long PtId { get; set; }

        public long SeqNo { get; set; }

        public string Text { get; set; } = string.Empty;

        public string Rtext { get; set; } = string.Empty;

        public SummaryInfItem Map(int hpId)
        {
            return new SummaryInfItem(Id, hpId, PtId, SeqNo, Text, Rtext);
        }
    }
}
