using UseCase.Core.Sync.Core;

namespace UseCase.OrdInfs.ValidationInputItem
{
    public class ValidationInputItemInputData : IInputData<ValidationInputItemOutputData>
    {
        public ValidationInputItemInputData(int hpId, int sinDate, List<ValidationInputItemItem> items)
        {
            Items = items;
            HpId = hpId;
            SinDate = sinDate;
        }
        public int HpId { get; private set; }
        public int SinDate { get; private set; }

        public List<ValidationInputItemItem> Items { get; private set; }

        public List<ValidationInputItemItem> ToList()
        {
            return Items;
        }
    }
}
