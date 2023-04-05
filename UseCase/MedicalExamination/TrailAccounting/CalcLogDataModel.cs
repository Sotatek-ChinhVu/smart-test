using Entity.Tenant;

namespace UseCase.MedicalExamination.TrailAccounting
{
    public class CalcLogDataModel
    {
        public CalcLogDataModel(CalcLog calcLog, int hpId, int ptId, int sinDate, int raiinNo, int seqNo, int logSbt, string text, int hokenId, string itemCd, string delItemCd, int delSbt, int isWarning, int termCnt, int termSbt, DateTime createDate, int createId, string createMachine, DateTime updateDate, int updateId, string updateMachine)
        {
            CalcLog = calcLog;
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            SeqNo = seqNo;
            LogSbt = logSbt;
            Text = text;
            HokenId = hokenId;
            ItemCd = itemCd;
            DelItemCd = delItemCd;
            DelSbt = delSbt;
            IsWarning = isWarning;
            TermCnt = termCnt;
            TermSbt = termSbt;
            CreateDate = createDate;
            CreateId = createId;
            CreateMachine = createMachine;
            UpdateDate = updateDate;
            UpdateId = updateId;
            UpdateMachine = updateMachine;
        }

        public CalcLog CalcLog { get; private set; }
        public int HpId { get; private set; }
        public int PtId { get; private set; }
        public int SinDate { get; private set; }
        public int RaiinNo { get; private set; }
        public int SeqNo { get; private set; }
        public int LogSbt { get; private set; }
        public string Text { get; private set; }
        public int HokenId { get; private set; }
        public string ItemCd { get; private set; }
        public string DelItemCd { get; private set; }
        public int DelSbt { get; private set; }
        public int IsWarning { get; private set; }
        public int TermCnt { get; private set; }
        public int TermSbt { get; private set; }
        public DateTime CreateDate { get; private set; }
        public int CreateId { get; private set; }
        public string CreateMachine { get; private set; }
        public DateTime UpdateDate { get; private set; }
        public int UpdateId { get; private set; }
        public string UpdateMachine { get; private set; }
    }
}
