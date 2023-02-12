namespace Domain.Models.HokenMst
{
    public class PtHokenCheckModel
    {
        public PtHokenCheckModel(int hpId, long ptID, int hokenGrp, int hokenId, DateTime checkDate, int checkId, string checkMachine, string checkCmt, int isDeleted)
        {
            HpId = hpId;
            PtID = ptID;
            HokenGrp = hokenGrp;
            HokenId = hokenId;
            CheckDate = checkDate;
            CheckId = checkId;
            CheckMachine = checkMachine;
            CheckCmt = checkCmt;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }

        public long PtID { get; private set; }

        public int HokenGrp { get; private set; }

        public int HokenId { get; private set; }

        public DateTime CheckDate { get; private set; }

        public int CheckId { get; private set; }

        public string CheckMachine { get; private set; }

        public string CheckCmt { get; private set; }

        public int IsDeleted { get; private set; }

    }
}
