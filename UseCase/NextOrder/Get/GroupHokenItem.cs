namespace UseCase.NextOrder.Get
{
    public class GroupHokenItem
    {

        public int HokenPid { get; private set; }
        public string HokenTitle { get; private set; }
        public List<GroupOdrItem> GroupOdrItems { get; private set; }

        public GroupHokenItem(List<GroupOdrItem> groupOdrItem, int hokenPid, string hokenTitle)
        {
            GroupOdrItems = groupOdrItem;
            HokenPid = hokenPid;
            HokenTitle = hokenTitle;
        }
    }
}
