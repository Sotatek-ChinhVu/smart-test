namespace Domain.Models.MstItem
{
    public class ItemCommentSuggestionModel
    {
        public ItemCommentSuggestionModel(string itemCd, string itemName, string santeiItemCd, List<RecedenCmtSelectModel> itemCmtModels)
        {
            ItemCd = itemCd;
            ItemName = itemName;
            SanteiItemCd = santeiItemCd;
            ItemCmtModels = itemCmtModels;
        }

        public string ItemCd { get; private set; }

        public string ItemName { get; private set; }

        public string SanteiItemCd { get; private set; }

        public List<RecedenCmtSelectModel> ItemCmtModels { get; private set; }

        public List<GroupCommentModel> ListGroupComment { get; private set; }

        public ItemCommentSuggestionModel SetData(List<RecedenCmtSelectModel> itemCmtModels)
        {
            ItemCmtModels = itemCmtModels;
            return this;
        }

        public void SetRecedenCmtSelectModel(List<RecedenCmtSelectModel> data)
        {
            if (data == null || data.Count == 0)
            {
                return;
            }

            var listGroupComment = new List<GroupCommentModel>();
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
                        var itemCmtModels = new List<RecedenCmtSelectModel>();

                        foreach (var item in group.ListItem)
                        {
                            itemCmtModels.Add(new RecedenCmtSelectModel(
                                                                    item.CmtSbt,
                                                                    item.ItemCd,
                                                                    item.CmtCd,
                                                                    item.CommentName,
                                                                    item.ItemNo,
                                                                    item.EdaNo,
                                                                    item.Content,
                                                                    item.SortNo,
                                                                    item.CondKbn,
                                                                    item.TenMst));
                        }

                        var groupComment = new GroupCommentModel(itemCmtModels);
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

    public class GroupCommentModel
    {
        public GroupCommentModel(List<RecedenCmtSelectModel> itemCmtModels)
        {
            ItemCmtModels = itemCmtModels;
        }

        public List<RecedenCmtSelectModel> ItemCmtModels { get; private set; }
    }
}
