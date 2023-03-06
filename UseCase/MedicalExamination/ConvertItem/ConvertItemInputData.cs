using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;
using UseCase.OrdInfs.GetListTrees;

namespace UseCase.MedicalExamination.ConvertItem
{
    public class ConvertItemInputData : IInputData<ConvertItemOutputData>
    {
        public ConvertItemInputData(int hpId, int userId, List<OdrInfItem> odrInfItems, Dictionary<string, List<TenItemModel>> expiredItems)
        {
            HpId = hpId;
            UserId = userId;
            OdrInfItems = odrInfItems;
            ExpiredItems = expiredItems;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public List<OdrInfItem> OdrInfItems { get; private set; }

        public Dictionary<string, List<TenItemModel>> ExpiredItems { get; private set; }
    }
}
