using Domain.Models.SpecialNote.SummaryInf;

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

        public DateTime CreateDate { get; set; }
        public SummaryInfModel Map()
        {
            return new SummaryInfModel(Id, HpId, PtId, SeqNo, Text, Rtext, CreateDate, DateTime.MinValue);
        }
    }
}
