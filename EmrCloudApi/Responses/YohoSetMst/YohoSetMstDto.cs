using Domain.Models.OrdInfDetails;

namespace EmrCloudApi.Responses.YohoSetMst
{
    public class YohoSetMstDto
    {
        public YohoSetMstDto(YohoSetMstModel model)
        {
            Itemname = model.Itemname;
            YohoKbn = model.YohoKbn;
            SetId = model.SetId;
            UserId = model.UserId;
            ItemCd = model.ItemCd;
        }

        public string Itemname { get; private set; }
        public int YohoKbn { get; private set; }
        public int SetId { get; private set; }
        public int UserId { get; private set; }
        public string ItemCd { get; private set; }
    }
}
