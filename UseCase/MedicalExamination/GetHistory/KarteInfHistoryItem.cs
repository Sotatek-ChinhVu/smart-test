using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.MedicalExamination.GetHistory
{
    public class KarteInfHistoryItem
    {
        public KarteInfHistoryItem(int hpId, long raiinNo, int karteKbn, long seqNo, long ptId, int sinDate, string text, DateTime updateDate, DateTime createDate, int isDeleted)
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
        }

        public int HpId { get; private set; }
        public long RaiinNo { get; private set; }
        public int KarteKbn { get; private set; }
        public long SeqNo { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public string Text { get; private set; }
        public DateTime UpdateDate { get; private set; }
        public DateTime CreateDate { get; private set; }
        public int IsDeleted { get; private set; }
        public string UpdateDateDisplay
        {
            get => UpdateDate.ToString("yyyy/MM/dd hh:mm");
        }
        public string CreateDateDisplay
        {
            get => CreateDate.ToString("yyyy/MM/dd hh:mm");
        }
    }
}
