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

        public int HpId { get; private set; }

        public int MenuId { get; private set; }

        public int SeqNo { get; private set; }

        public int SortNo { get; private set; }

        public string ItemName { get; private set; }

        public int Val { get; private set; }

        public int ParamMin { get; private set; }

        public int ParamMax { get; private set; }
    }
}
