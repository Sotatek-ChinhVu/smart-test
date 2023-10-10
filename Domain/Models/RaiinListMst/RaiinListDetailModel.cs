using Domain.Models.RaiinListSetting;
using Helper.Extension;
using System.Text.Json.Serialization;

namespace Domain.Models.RaiinListMst
{
    public class RaiinListDetailModel
    {
        [JsonConstructor]
        public RaiinListDetailModel(int grpId, int kbnCd, int sortNo, string kbnName, string colorCd, int isDeleted)
        {
            GrpId = grpId;
            KbnCd = kbnCd;
            SortNo = sortNo;
            KbnName = kbnName;
            ColorCd = colorCd;
            IsDeleted = isDeleted;
            RaiinListDoc = new List<RaiinListDocModel>();
            RaiinListItem = new List<RaiinListItemModel>();
            RaiinListFile = new List<RaiinListFileModel>();
            KouCollection = ObjectExtension.CreateInstance<KouiKbnCollectionModel>();
        }

        public RaiinListDetailModel(int grpId, int kbnCd, int sortNo, string kbnName, string colorCd, int isDeleted, bool isOnlySwapSortNo, int sortNoDetailMax, int kbnCdDetailMax, List<RaiinListDocModel> raiinListDoc, List<RaiinListItemModel> raiinListItem, List<RaiinListFileModel> raiinListFile, KouiKbnCollectionModel kouCollection)
        {
            GrpId = grpId;
            KbnCd = kbnCd;
            SortNo = sortNo;
            KbnName = kbnName;
            ColorCd = colorCd;
            IsDeleted = isDeleted;
            IsOnlySwapSortNo = isOnlySwapSortNo;
            SortNoDetailMax = sortNoDetailMax;
            KbnCdDetailMax = kbnCdDetailMax;
            RaiinListDoc = raiinListDoc;
            RaiinListItem = raiinListItem;
            RaiinListFile = raiinListFile;
            KouCollection = kouCollection;
        }

        public RaiinListDetailModel(int grpId, int kbnCd, int sortNo, string kbnName, string colorCd, int isDeleted, bool isOnlySwapSortNo, List<RaiinListDocModel> raiinListDoc, List<RaiinListItemModel> raiinListItem, List<RaiinListFileModel> raiinListFile, KouiKbnCollectionModel kouCollection)
        {
            GrpId = grpId;
            KbnCd = kbnCd;
            SortNo = sortNo;
            KbnName = kbnName;
            ColorCd = colorCd;
            IsDeleted = isDeleted;
            IsOnlySwapSortNo = isOnlySwapSortNo;
            RaiinListDoc = raiinListDoc;
            RaiinListItem = raiinListItem;
            RaiinListFile = raiinListFile;
            KouCollection = kouCollection;
        }

        public int GrpId { get; private set; }

        public int KbnCd { get; private set; }

        public int SortNo { get; private set; }

        public string KbnName { get; private set; }

        public string ColorCd { get; private set; }

        public int IsDeleted { get; private set; }

        /// <summary>
        /// not chage model , only change sortNo && KouiKbnCollection not Changed && RaiinListDoc not Changed && RaiinListFile not Changed && RaiinList not Changed
        /// </summary>
        public bool IsOnlySwapSortNo { get; private set; }
        
        public int SortNoDetailMax { get; private set; }

        public int KbnCdDetailMax { get; private set; }

        public List<RaiinListDocModel> RaiinListDoc { get; private set; }

        public List<RaiinListItemModel> RaiinListItem { get; private set; }

        public List<RaiinListFileModel> RaiinListFile { get; private set; }

        public KouiKbnCollectionModel KouCollection { get; private set; }
    }
}
