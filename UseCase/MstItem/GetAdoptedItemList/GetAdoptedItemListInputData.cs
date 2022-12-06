using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetAdoptedItemList
{
    public class GetAdoptedItemListInputData : IInputData<GetAdoptedItemListOutputData>
    {
        public GetAdoptedItemListInputData(List<string> itemCds, int sinDate, int hpId)
        {
            ItemCds = itemCds;
            SinDate = sinDate;
            HpId = hpId;
        }

        public List<string> ItemCds { get; private set; }
        public int SinDate { get; private set; }
        public int HpId { get; private set; }
    }
}
