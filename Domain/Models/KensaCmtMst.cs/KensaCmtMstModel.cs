namespace Domain.Models.KensaCmtMst.cs
{
    public class KensaCmtMstModel
    {
        public KensaCmtMstModel(string cmtCd, string? cmt, int cmtSeqNo, string? centerName)
        {
            CmtCd = cmtCd;
            Cmt = cmt;
            CmtSeqNo = cmtSeqNo;
            CenterName = centerName;
        }

        public string CmtCd { get; set; }
        public string? Cmt { get; set; }
        public int CmtSeqNo { get; set; }
        public string? CenterName { get; set; }
    }
}
