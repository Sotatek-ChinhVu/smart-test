using UseCase.Core.Sync.Core;

namespace UseCase.KarteInfs.GetLists
{
    public class GetListKarteInfOuputItem : IOutputData
    {
        public GetListKarteInfOuputItem(int hpId, long raiinNo, int karteKbn, long seqNo, long ptId, int sinDate, string text, int isDeleted, string richText)
        {
            HpId = hpId;
            RaiinNo = raiinNo;
            KarteKbn = karteKbn;
            SeqNo = seqNo;
            PtId = ptId;
            SinDate = sinDate;
            Text = text;
            IsDeleted = isDeleted;
            RichText = richText;
        }

        public int HpId { get; set; }
        public long RaiinNo { get; set; }
        public int KarteKbn { get; set; }
        public long SeqNo { get; set; }
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public string Text { get; set; }
        public int IsDeleted { get; set; }
        public string RichText { get; set; }
    }
}
