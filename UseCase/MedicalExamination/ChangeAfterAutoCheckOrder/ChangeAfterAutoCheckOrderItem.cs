using UseCase.OrdInfs.GetListTrees;

namespace UseCase.MedicalExamination.ChangeAfterAutoCheckOrder
{
    public class ChangeAfterAutoCheckOrderItem
    {
        public ChangeAfterAutoCheckOrderItem(int position, OdrInfItem odrInfItem)
        {
            Position = position;
            OdrInfItem = odrInfItem;
        }

        public int Position { get; private set; }

        public OdrInfItem OdrInfItem { get; private set; }
    }
}
