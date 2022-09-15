namespace Domain.Models.OrdInfDetails
{
    public class YohoSetMstModel
    {
        public YohoSetMstModel(string itemname, int yohoKbn, int setId, int userId, string itemCd)
        {
            Itemname = itemname;
            YohoKbn = yohoKbn;
            SetId = setId;
            UserId = userId;
            ItemCd = itemCd;
        }

        public string Itemname { get; private set; }
        public int YohoKbn { get; private set; }
        public int SetId { get; private set; }
        public int UserId { get; private set; }
        public string ItemCd { get; private set; }
    }
}
