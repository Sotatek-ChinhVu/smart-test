namespace CommonChecker.Models
{
    public class ItemCodeModel
    {
        public ItemCodeModel(string itemCd, string id)
        {
            ItemCd = itemCd;
            Id = id;
        }

        public string ItemCd { get; set; }
        public string Id { get; set; }
    }
}
