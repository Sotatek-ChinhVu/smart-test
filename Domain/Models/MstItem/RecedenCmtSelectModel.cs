namespace Domain.Models.MstItem
{
    public class RecedenCmtSelectModel
    {
        private List<int> cmtSbts = new List<int> { 20, 21, 22 };

        public RecedenCmtSelectModel(int cmtSbt, string itemCd, string cmtCd, string commentName, int itemNo, int edaNo, string content, int sortNo, int condKbn, TenItemModel tenMst)
        {
            CmtSbt = cmtSbt;
            ItemCd = itemCd;
            CmtCd = cmtCd;
            CommentName = commentName;
            ItemNo = itemNo;
            EdaNo = edaNo;
            Content = content;
            SortNo = sortNo;
            CondKbn = condKbn;
            TenMst = tenMst;
        }

        public bool IsSatsueiBui
        {
            get
            {
                return cmtSbts.Contains(CmtSbt);
            }
        }

        public int CmtSbt { get; private set; }

        public string ItemCd { get; private set; }

        public string CmtCd { get; private set; }

        public string CommentName { get; private set; }

        public int ItemNo { get; private set; }

        public int EdaNo { get; private set; }

        public string Content { get; private set; }

        public int SortNo { get; private set; }

        public int CondKbn { get; private set; }

        public TenItemModel TenMst { get; private set; }
    }
}
