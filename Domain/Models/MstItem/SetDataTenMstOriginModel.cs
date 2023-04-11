using Domain.Models.DrugInfor;
using Domain.Models.OrdInf;

namespace Domain.Models.MstItem
{
    public class SetDataTenMstOriginModel
    {
        public TenMstOriginModel TenOriginSelected { get; private set; }

        public BasicSettingTabModel BasicSettingTab { get; private set; }

        public IjiSettingTabModel IjiSettingTab { get; private set; }
    }

    public class BasicSettingTabModel
    {
        public BasicSettingTabModel(List<CmtKbnMstModel> cmtKbnMstModels)
        {
            CmtKbnMstModels = cmtKbnMstModels;
        }

        public List<CmtKbnMstModel> CmtKbnMstModels { get; private set; }
    }

    public class IjiSettingTabModel
    {
        public IjiSettingTabModel(string searchItemName)
        {
            SearchItemName = searchItemName;
        }

        public string SearchItemName { get; private set; }
    }

    public class PrecriptionSettingTabModel
    {
        public PrecriptionSettingTabModel(List<M10DayLimitModel> m10DayLimits, List<IpnMinYakkaMstModel> ipnMinYakkaMsts, List<DrugDayLimitModel> drugDayLimits, DosageMstModel dosageMst, IpnNameMstModel ipnNameMst)
        {
            M10DayLimits = m10DayLimits;
            IpnMinYakkaMsts = ipnMinYakkaMsts;
            DrugDayLimits = drugDayLimits;
            DosageMst = dosageMst;
            this.ipnNameMst = ipnNameMst;
        }

        public List<M10DayLimitModel> M10DayLimits { get; private set; }

        public List<IpnMinYakkaMstModel> IpnMinYakkaMsts { get; private set; }

        public List<DrugDayLimitModel> DrugDayLimits { get; private set; }

        public DosageMstModel DosageMst { get; private set; }

        public IpnNameMstModel ipnNameMst { get; private set; }
    }

    public class UsageSettingTabModel
    {
        public UsageSettingTabModel(string yohoInfMstPrefix)
        {
            YohoInfMstPrefix = yohoInfMstPrefix;
        }

        public string YohoInfMstPrefix { get; private set; }
    }


    public class DrugInfomationTabModel
    {
        public List<DrugInfModel> DrugInfs { get; private set; }

        public PiImageModel ZaiImage { get; private set; }

        public PiImageModel HouImage { get; private set; }
    }

}
