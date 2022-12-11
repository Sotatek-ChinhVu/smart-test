using Domain.Models.MstItem;

namespace UseCase.MstItem.GetSelectiveComment
{
    public class GetSelectiveCommentItemOfList
    {
        public GetSelectiveCommentItemOfList(List<GroupSelectiveCommentItem> listGroupComment, string itemCd, string itemName, string santeiItemCd)
        {
            ListGroupComment = listGroupComment;
            ItemCd = itemCd;
            ItemName = itemName;
            SanteiItemCd = santeiItemCd;
        }

        public GetSelectiveCommentItemOfList()
        {
            ListGroupComment = new();
            ItemCd = string.Empty;
            ItemName = string.Empty;
            SanteiItemCd = string.Empty;
        }

        public List<GroupSelectiveCommentItem> ListGroupComment { get; private set; }

        public string ItemCd { get; private set; }

        public string ItemName { get; private set; }

        public string SanteiItemCd { get; private set; }

        public void SetData(List<RecedenCmtSelectModel> data)
        {
            if (data == null || data.Count == 0)
            {
                return;
            }
            List<GroupSelectiveCommentItem> listGroupComment = new List<GroupSelectiveCommentItem>();
            try
            {
                var groupingData = data.GroupBy(x => new { x.ItemNo, x.EdaNo })
                    .OrderBy(g => g.Key.ItemNo)
                    .ThenBy(g => g.Key.EdaNo)
                    .Select(g => new
                    {
                        ListItem = g.ToList()
                    })
                    .ToList();

                groupingData.ForEach((group) =>
                {
                    if (group.ListItem.Count > 0)
                    {
                        GroupSelectiveCommentItem groupComment = new GroupSelectiveCommentItem(group.ListItem);
                        listGroupComment.Add(groupComment);
                    }
                });
            }
            finally
            {
                ListGroupComment = listGroupComment;
            }
        }
    }
}
