namespace UseCase.MedicalExamination.GetOrderSheetGroup
{
    public class OrderSheetItem
    {
        public OrderSheetItem(string nodeText, int groupKouiKbn, int odrKouiKbn, int sinDate, int level, bool isExpanded, bool isSelected, List<OrderSheetItem> childrens)
        {
            NodeText = nodeText;
            GroupKouiKbn = groupKouiKbn;
            OdrKouiKbn = odrKouiKbn;
            SinDate = sinDate;
            Level = level;
            IsExpanded = isExpanded;
            IsSelected = isSelected;
            Childrens = childrens;
        }

        public OrderSheetItem(int level, int groupKouiKbn, int odrKouiKbn, string nodeText)
        {
            NodeText = nodeText;
            GroupKouiKbn = groupKouiKbn;
            OdrKouiKbn= odrKouiKbn;
            Level = level;
            Childrens = new();
        }

        public OrderSheetItem(int level, int sinDate, int odrKouiKbn, int groupKouiKbn, string nodeText)
        {
            NodeText = nodeText;
            OdrKouiKbn = odrKouiKbn;
            GroupKouiKbn = groupKouiKbn;
            Level = level;
            SinDate = sinDate;
            Childrens = new();
        }

        public OrderSheetItem ChangeIsExpanded(bool isExpanded)
        {
            IsExpanded = isExpanded;
            return this;
        }

        public OrderSheetItem ChangeIsSelected(bool isSelected)
        {
            IsSelected = isSelected;
            return this;
        }

        public OrderSheetItem ChangeSomeProperties(bool isExpanded, bool isSelected)
        {
            IsExpanded = isExpanded;
            IsSelected = isSelected;
            return this;
        }

        public string NodeText { get; private set; }

        public int GroupKouiKbn { get; private set; }

        public int OdrKouiKbn { get; private set; }

        public int SinDate { get; private set; }

        public int Level { get; private set; }

        public bool IsExpanded { get; private set; }

        public bool IsSelected { get; private set; }

        public List<OrderSheetItem> Childrens { get; private set; }

        public bool HasChild => Childrens?.Count > 0;
    }
}
