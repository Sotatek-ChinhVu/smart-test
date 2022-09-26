namespace Domain.Models.UsageTreeSet
{
    public class ListSetMstModel
    {
        public ListSetMstModel(int hpId, int generationId,int setId, string setName, string itemCd, int isTitle, int setKbn, int selectType, double suryo, int level1, int level2, int level3, int level4, int level5, string cmtName, string cmtOpt,string unitName,int sinKouiKbn)
        {
            HpId = hpId;
            GenerationId = generationId;
            SetId = setId;
            SetName = setName;
            ItemCd = itemCd;
            IsTitle = isTitle;
            SetKbn = setKbn;
            SelectType = selectType;
            Suryo = suryo;
            Level1 = level1;
            Level2 = level2;
            Level3 = level3;
            Level4 = level4;
            Level5 = level5;
            CmtName = cmtName;
            CmtOpt = cmtOpt;
            UnitName = unitName;
            SinKouiKbn = sinKouiKbn;
        }

        public int HpId { get; private set; }
        public int GenerationId { get; private set; }
        public int SetId { get; private set; }
        public string SetName { get; private set; }
        public string ItemCd { get; private set; }
        public int IsTitle { get; private set; }
        public int SetKbn { get; private set; }
        public int SelectType { get; private set; }
        public double Suryo { get; private set; }
        public int Level1 { get; private set; }
        public int Level2 { get; private set; }
        public int Level3 { get; private set; }
        public int Level4 { get; private set; }
        public int Level5 { get; private set; }
        public string CmtName { get; private set; }
        public string CmtOpt { get; private set; }
        public int Level { get; set; }
        public string UnitName { get; private set; }
        public int SinKouiKbn { get; private set; }

        public bool HasChildItems
        {
            get
            {
                return Childrens != null && Childrens.Any();
            }
        }

        public IEnumerable<ListSetMstModel> Childrens { get; set; } = new List<ListSetMstModel>();
    }
}