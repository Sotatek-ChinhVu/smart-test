using System.Text.Json.Serialization;

namespace Domain.Models.Insurance
{
    public class ConfirmDateModel
    {
        public int HokenGrp { get; private set; }

        public int HokenId { get; private set; }

        public long SeqNo { get; private set; }

        public int CheckId { get; private set; }

        public string CheckName{ get; private set; }

        public string CheckComment { get; private set; }

        public int ConfirmDate { get; private set; }

        public ConfirmDateModel(int hokenGrp, int hokenId, long seqNo, int checkId, string checkName, string checkComment, DateTime confirmDate)
        {
            HokenGrp = hokenGrp;
            HokenId = hokenId;
            SeqNo = seqNo;
            CheckId = checkId;
            CheckName = checkName;
            CheckComment = checkComment;
            ConfirmDate = int.Parse(confirmDate.ToString("yyyyMMdd"));
        }

        [JsonConstructor]
        public ConfirmDateModel(int hokenGrp, int hokenId, long seqNo, int checkId, string checkName, string checkComment, int confirmDate)
        {
            HokenGrp = hokenGrp;
            HokenId = hokenId;
            SeqNo = seqNo;
            CheckId = checkId;
            CheckName = checkName;
            CheckComment = checkComment;
            ConfirmDate = confirmDate;
        }
    }
}
