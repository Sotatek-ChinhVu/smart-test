using UseCase.Core.Sync.Core;

namespace UseCase.SpecialNote.AddAlrgyDrugList
{
    public class AddAlrgyDrugListInputData : IInputData<AddAlrgyDrugListOutputData>
    {
        public AddAlrgyDrugListInputData(List<AddAlrgyDrugListItemInputData> dataList, int hpId, int userId)
        {
            DataList = dataList;
            HpId = hpId;
            UserId = userId;
        }

        public List<AddAlrgyDrugListItemInputData> DataList { get; private set; }

        public int HpId { get; private set; }

        public int UserId { get; private set; }
    }
}
