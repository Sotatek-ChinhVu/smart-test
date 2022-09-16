using UseCase.Core.Sync.Core;

namespace UseCase.SpecialNote.AddAlrgyDrugList
{
    public class AddAlrgyDrugListInputData : IInputData<AddAlrgyDrugListOutputData>
    {
        public AddAlrgyDrugListInputData(List<AddAlrgyDrugListItemInputData> dataList)
        {
            DataList = dataList;
        }

        public List<AddAlrgyDrugListItemInputData> DataList { get; private set; }
    }
}
