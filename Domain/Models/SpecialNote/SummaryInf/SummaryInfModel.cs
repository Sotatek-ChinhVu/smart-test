using Helper.Common;
using Helper.Extension;

namespace Domain.Models.SpecialNote.SummaryInf
{
    public class SummaryInfModel
    {
        public SummaryInfModel(long id, int hpId, long ptId, long seqNo, string text, string rtext, DateTime createDate, DateTime updateDate)
        {
            Id = id;
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
            Text = text;
            Rtext = rtext;
            CreateDate = createDate;
            UpdateDate = updateDate;
        }

        public SummaryInfModel()
        {
            Text = string.Empty;
            Rtext = string.Empty;
        }

        public long Id { get; private set; }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public long SeqNo { get; private set; }

        public string Text { get; private set; }

        public string Rtext { get; private set; }

        public DateTime CreateDate { get; private set; }

        public DateTime UpdateDate { get; private set; }

        public string DisplayUpdateDate
        {
            get
            {
                return CIUtil.SDateToShowSDate(UpdateDate.ToString("yyyyMMdd").AsInteger()) + " 更新";
            }
        }
    }
}
