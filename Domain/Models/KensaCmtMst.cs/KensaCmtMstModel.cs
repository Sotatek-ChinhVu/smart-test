namespace Domain.Models.KensaCmtMst.cs
{
    public class KensaCmtMstModel
    {
        public KensaCmtMstModel(string cmtCd, string cmt, int cmtSeqNo, string centerName)
        {
            CmtCd = cmtCd;
            Cmt = cmt;
            CmtSeqNo = cmtSeqNo;
            CenterName = centerName;
        }

        public string CmtCd { get; private set; }
        public string Cmt { get; private set; }
        public int CmtSeqNo { get; private set; }
        public string CenterName { get; private set; }
    }
}
