using Domain.Models.SpecialNote.ImportantNote;
using UseCase.Core.Sync.Core;

namespace UseCase.SpecialNote.AddAlrgyDrugList
{
    public class AddAlrgyDrugListInputData : IInputData<AddAlrgyDrugListOutputData>
    {
        public AddAlrgyDrugListInputData(List<PtAlrgyDrugModel> dataList)
        {
            DataList = dataList;
        }

        public List<PtAlrgyDrugModel> DataList { get; private set; }
    }
}
