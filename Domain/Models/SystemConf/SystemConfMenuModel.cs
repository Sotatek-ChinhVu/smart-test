using Domain.Models.SystemGenerationConf;
using System.Text.Json.Serialization;

namespace Domain.Models.SystemConf
{
    public class SystemConfMenuModel
    {
        public SystemConfMenuModel(int hpId, int menuId, int menuGrp, int sortNo, string menuName, int grpCd, int grpEdaNo, int pathGrpCd, int isParam, int paramMask, int paramType, string paramHint, double valMin, double valMax, double paramMin, double paramMax, string itemCd, int prefNo, int isVisible, int managerKbn, int isValue, int paramMaxLength)
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
            SystemConfItems = new();
            SystemGenerationConfs = new();
            SystemConf = new();
        }

        public SystemConfMenuModel(int hpId, int menuId, int menuGrp, int sortNo, string menuName, int grpCd, int grpEdaNo, int pathGrpCd, int isParam, int paramMask, int paramType, string paramHint, double valMin, double valMax, double paramMin, double paramMax, string itemCd, int prefNo, int isVisible, int managerKbn, int isValue, int paramMaxLength, List<SystemConfItemModel> systemConfItems, List<SystemGenerationConfModel> systemGenerationConfs)
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
            SystemConfItems = systemConfItems;
            SystemGenerationConfs = systemGenerationConfs;
            SystemConf = new();
        }

        public SystemConfMenuModel(int hpId, int menuId, int menuGrp, int sortNo, string menuName, int grpCd, int grpEdaNo, int pathGrpCd, int isParam, int paramMask, int paramType, string paramHint, double valMin, double valMax, double paramMin, double paramMax, string itemCd, int prefNo, int isVisible, int managerKbn, int isValue, int paramMaxLength, List<SystemConfItemModel> systemConfItems, SystemConfModel systemConf)
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
            SystemConfItems = systemConfItems;
            SystemConf = systemConf;
            SystemGenerationConfs = new();
        }

        [JsonConstructor]
        public SystemConfMenuModel(List<SystemGenerationConfModel> systemGenerationConfs, SystemConfModel systemConf)
        {
            MenuName = string.Empty;
            ItemCd = string.Empty;
            ParamHint = string.Empty;
            SystemConfItems = new();
            SystemGenerationConfs = systemGenerationConfs;
            SystemConf = systemConf;
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

        public List<SystemConfItemModel> SystemConfItems { get; private set; }

        public List<SystemGenerationConfModel> SystemGenerationConfs { get; private set; }

        public SystemConfModel SystemConf { get; private set; }
    }
}
