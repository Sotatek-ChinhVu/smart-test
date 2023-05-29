namespace Domain.Models.RaiinListSetting
{
    public class RaiinListFileModel
    {
        public RaiinListFileModel(int hpId, int grpId, int kbnCd, int categoryCd, string categoryName, long seqNo, int isDeleted, bool isModify)
        {
            HpId = hpId;
            GrpId = grpId;
            KbnCd = kbnCd;
            CategoryCd = categoryCd;
            CategoryName = categoryName;
            SeqNo = seqNo;
            IsDeleted = isDeleted;
            IsModify = isModify;
        }

        public int HpId { get; private set; }

        public int GrpId { get; private set; }

        public int KbnCd { get; private set; }

        public int CategoryCd { get; private set; }

        /// <summary>
        /// FilingCategoryModel?.CategoryName
        /// </summary>
        public string CategoryName { get; private set; }


        public long SeqNo { get; private set; }

        public int IsDeleted { get; private set; }

        public bool IsModify { get; private set; }

        public bool CheckDefaultValue()
        {
            return HpId == 0 && CategoryCd == 0;
        }
    }
}
