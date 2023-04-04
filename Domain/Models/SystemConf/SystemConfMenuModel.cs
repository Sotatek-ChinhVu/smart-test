namespace Domain.Models.SystemConf
{
    public class SystemConfMenuModel
    {
        public SystemConfMenuModel(int hpId, int menuId, int menuGrp, int sortNo, string menuName, int grpCd, int grpEdaNo, int pathGrpCd, int isParam, int paramMask, int paramType, string paramHint, double valMin, double valMax, double paramMin, double paramMax, string itemCd, int prefNo, int isVisible, int managerKbn, int isValue, int paramMaxLength, DateTime createDate, int createId, string createMachine, DateTime updateDate, int updateId, string updateMachine)
        {
            HpId = hpId;
            MenuId = menuId;
            MenuGrp = menuGrp;
            SortNo = sortNo;
            MenuName = menuName;
            GrpCd = grpCd;
            GrpEdaNo = grpEdaNo;
            PathGrpCd = pathGrpCd;
            IsParam = isParam;
            ParamMask = paramMask;
            ParamType = paramType;
            ParamHint = paramHint;
            ValMin = valMin;
            ValMax = valMax;
            ParamMin = paramMin;
            ParamMax = paramMax;
            ItemCd = itemCd;
            PrefNo = prefNo;
            IsVisible = isVisible;
            ManagerKbn = managerKbn;
            IsValue = isValue;
            ParamMaxLength = paramMaxLength;
            CreateDate = createDate;
            CreateId = createId;
            CreateMachine = createMachine;
            UpdateDate = updateDate;
            UpdateId = updateId;
            UpdateMachine = updateMachine;
        }

        public int HpId { get; private set; }

        public int MenuId { get; private set; }

        public int MenuGrp { get; private set; }

        public int SortNo { get; private set; }

        public string MenuName { get; private set; }

        public int GrpCd { get; private set; }

        public int GrpEdaNo { get; private set; }

        public int PathGrpCd { get; private set; }

        public int IsParam { get; private set; }

        public int ParamMask { get; private set; }

        public int ParamType { get; private set; }

        public string ParamHint { get; private set; }

        public double ValMin { get; private set; }

        public double ValMax { get; private set; }

        public double ParamMin { get; private set; }

        public double ParamMax { get; private set; }

        public string ItemCd { get; private set; }

        public int PrefNo { get; private set; }

        public int IsVisible { get; private set; }

        public int ManagerKbn { get; private set; }

        public int IsValue { get; private set; }

        public int ParamMaxLength { get; private set; }

        public DateTime CreateDate { get; private set; }

        public int CreateId { get; private set; }

        public string CreateMachine { get; private set; }

        public DateTime UpdateDate { get; private set; }

        public int UpdateId { get; private set; }

        public string UpdateMachine { get; private set; }
    }
}
