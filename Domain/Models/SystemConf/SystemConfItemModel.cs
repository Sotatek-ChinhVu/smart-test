namespace Domain.Models.SystemConf
{
    public class SystemConfItemModel
    {
        public SystemConfItemModel(int hpId, int menuId, int seqNo, int sortNo, string itemName, int val, int paramMin, int paramMax)
        {
            HpId = hpId;
            MenuId = menuId;
            SeqNo = seqNo;
            SortNo = sortNo;
            ItemName = itemName;
            Val = val;
            ParamMin = paramMin;
            ParamMax = paramMax;
        }

        public int HpId { get; set; }

        public int MenuId { get; set; }

        public int SeqNo { get; set; }

        public int SortNo { get; set; }

        public string ItemName { get; set; }

        public int Val { get; set; }

        public int ParamMin { get; set; }

        public int ParamMax { get; set; }
    }
}
