using UseCase.Core.Sync.Core;

namespace UseCase.OrdInfs.ValidationInputItem
{
    public class ValidationInputItemInputData : IInputData<ValidationInputItemOutputData>
    {
        public ValidationInputItemInputData(List<ValidationInputItemItem> items)
        {
            Items = items;
        }

        public List<ValidationInputItemItem> Items { get; private set; }

        public List<ValidationInputItemItem> ToList()
        {
            return Items;
        }
    }
}
