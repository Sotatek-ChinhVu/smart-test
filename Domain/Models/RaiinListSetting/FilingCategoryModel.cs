namespace Domain.Models.RaiinListSetting
{
    public class FilingCategoryModel
    {
        public FilingCategoryModel(int hpId, int sortNo, int categoryCd, string categoryName, int dspKanzok)
        {
            HpId = hpId;
            SortNo = sortNo;
            CategoryCd = categoryCd;
            CategoryName = categoryName;
            DspKanzok = dspKanzok;
        }

        public int HpId { get; private set; }

        public int SortNo { get; private set; }

        public int CategoryCd { get; private set; }

        public string CategoryName { get; private set; }

        public int DspKanzok { get; private set; }
    }
}
