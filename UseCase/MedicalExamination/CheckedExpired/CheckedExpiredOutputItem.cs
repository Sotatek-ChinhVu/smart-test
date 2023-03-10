using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.CheckedExpired
{
    public class CheckedExpiredOutputItem : IOutputData
    {
        public CheckedExpiredOutputItem(string itemCd, string itemName, int sinKouiKbn, List<TenItemModel> tenItemModels)
        {
            ItemCd = itemCd;
            ItemName = itemName;
            SinKouiKbn = sinKouiKbn;
            TenItemModels = tenItemModels;
        }

        public string ItemCd { get; private set; }
        public string ItemName { get; private set; }
        public int SinKouiKbn { get; private set; }
        public List<TenItemModel> TenItemModels { get; private set; }
    }
}
