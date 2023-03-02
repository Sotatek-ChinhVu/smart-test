namespace Domain.Models.Accounting
{
    public class CalcLogModel
    {
        public CalcLogModel(long raiinNo, int logSbt, string text)
        {
            RaiinNo = raiinNo;
            LogSbt = logSbt;
            Text = text;
            DelItemCd = string.Empty;
            ItemCd = string.Empty;
        }

        public CalcLogModel(long raiinNo, int logSbt, string text, long ptId, int sinDate, int seqNo, int hokenId, string itemCd, string delItemCd, int delSbt, int isWarning, int termCnt, int termSbt)
        {
            RaiinNo = raiinNo;
            LogSbt = logSbt;
            Text = text;
            PtId = ptId;
            SinDate = sinDate;
            SeqNo = seqNo;
            HokenId = hokenId;
            ItemCd = itemCd;
            DelItemCd = delItemCd;
            DelSbt = delSbt;
            IsWarning = isWarning;
            TermCnt = termCnt;
            TermSbt = termSbt;
        }

        public long RaiinNo { get; private set; }

        public int LogSbt { get; private set; }

        public string Text { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public int SeqNo { get; private set; }

        public int HokenId { get; private set; }

        public string ItemCd { get; private set; }

        public string DelItemCd { get; private set; }

        public int DelSbt { get; private set; }

        public int IsWarning { get; private set; }

        public int TermCnt { get; private set; }

        public int TermSbt { get; private set; }
    }
}
