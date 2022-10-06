using Helper.Common;

namespace Domain.Models.PatientInfor
{
    public class PtHokenCheckModel
    {
        public PtHokenCheckModel(int hokenGrp, int hokenId, int checkDateInt, int checkId, string checkCmt, long seqNo)
        {
            HokenGrp = hokenGrp;
            HokenId = hokenId;
            CheckDateInt = checkDateInt;
            CheckId = checkId;
            CheckCmt = checkCmt;
            SeqNo = seqNo;
        }

        public int HokenGrp { get; set; }
        public int HokenId { get; set; }
        public int CheckDateInt
        {
            get; set;
        }
        public DateTime CheckDate
        {
            get
            {
                return CIUtil.IntToDate(CheckDateInt);
            }
        }
        public int CheckId { get; set; }
        public string CheckCmt { get; set; }
        public long SeqNo { get; set; }
    }
}
