namespace EmrCloudApi.Requests.ConfirmDate
{
    public class ConfirmDateDto
    {
        public ConfirmDateDto(int hokenGrp, int hokenId, long seqNo, int checkId, string checkName, string checkComment, int confirmDate, bool isHokenGrpKohi)
        {
            HokenGrp = hokenGrp;
            HokenId = hokenId;
            SeqNo = seqNo;
            CheckId = checkId;
            CheckName = checkName;
            CheckComment = checkComment;
            ConfirmDate = confirmDate;
            IsHokenGrpKohi = isHokenGrpKohi;
        }

        public int HokenGrp { get; private set; }

        public int HokenId { get; private set; }

        public long SeqNo { get; private set; }

        public int CheckId { get; private set; }

        public string CheckName { get; private set; }

        public string CheckComment { get; private set; }

        public int ConfirmDate { get; private set; }

        public bool IsHokenGrpKohi { get; private set; }
    }
}