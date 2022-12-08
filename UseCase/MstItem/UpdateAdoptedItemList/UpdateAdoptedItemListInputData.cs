using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.UpdateAdoptedItemList
{
    public class UpdateAdoptedItemListInputData : IInputData<UpdateAdoptedItemListOutputData>
    {
        public UpdateAdoptedItemListInputData(int valueAdopted, List<string> itemCds, int sinDate, int hpId, int userId)
        {
            ValueAdopted = valueAdopted;
            ItemCds = itemCds;
            SinDate = sinDate;
            HpId = hpId;
            UserId = userId;
        }

        public int ValueAdopted { get; private set; }

        public List<string> ItemCds { get; private set; }

        public int SinDate { get; private set; }

        public int HpId { get; private set; }

        public int UserId { get; private set; }
    }
}
