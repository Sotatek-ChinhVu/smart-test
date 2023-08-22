using Domain.Models.OrdInf;
using Domain.Models.TodayOdr;
using System.Text.Json.Serialization;

namespace Domain.Models.MstItem
{
    public class SetDataTenMstOriginModel
    {
        public SetDataTenMstOriginModel(BasicSettingTabModel basicSettingTab, IjiSettingTabModel ijiSettingTab, PrecriptionSettingTabModel precriptionSettingTab, UsageSettingTabModel usageSettingTab, DrugInfomationTabModel drugInfomationTab, TeikyoByomeiTabModel teikyoByomeiTab, SanteiKaishuTabModel santeiKaishuTab, HaihanTabModel haihanTab, HoukatsuTabModel houkatsuTab, CombinedContraindicationTabModel combinedContraindicationTab)
        {
            BasicSettingTab = basicSettingTab;
            IjiSettingTab = ijiSettingTab;
            PrecriptionSettingTab = precriptionSettingTab;
            UsageSettingTab = usageSettingTab;
            DrugInfomationTab = drugInfomationTab;
            TeikyoByomeiTab = teikyoByomeiTab;
            SanteiKaishuTab = santeiKaishuTab;
            HaihanTab = haihanTab;
            HoukatsuTab = houkatsuTab;
            CombinedContraindicationTab = combinedContraindicationTab;
        }

        public BasicSettingTabModel BasicSettingTab { get; private set; }

        public IjiSettingTabModel IjiSettingTab { get; private set; }

        public PrecriptionSettingTabModel PrecriptionSettingTab { get; private set; }

        public UsageSettingTabModel UsageSettingTab { get; private set; }

        public DrugInfomationTabModel DrugInfomationTab { get; private set; }

        public TeikyoByomeiTabModel TeikyoByomeiTab { get; private set; }

        public SanteiKaishuTabModel SanteiKaishuTab { get; private set; }

        public HaihanTabModel HaihanTab { get; private set; }

        public HoukatsuTabModel HoukatsuTab { get; private set; }

        public CombinedContraindicationTabModel CombinedContraindicationTab { get; private set; }
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
        [JsonConstructor]
        public IjiSettingTabModel(string searchItemName, string agekasanCd1Note, string agekasanCd2Note, string agekasanCd3Note, string agekasanCd4Note)
        {
            SearchItemName = searchItemName;
            AgekasanCd1Note = agekasanCd1Note;
            AgekasanCd2Note = agekasanCd2Note;
            AgekasanCd3Note = agekasanCd3Note;
            AgekasanCd4Note = agekasanCd4Note;
        }

        public IjiSettingTabModel()
        {
            SearchItemName = string.Empty;
            AgekasanCd1Note = string.Empty;
            AgekasanCd2Note = string.Empty;
            AgekasanCd3Note = string.Empty;
            AgekasanCd4Note = string.Empty;
        }

        public string SearchItemName { get; private set; }

        #region property selected
        public string AgekasanCd1Note { get; private set; }

        public string AgekasanCd2Note { get; private set; }

        public string AgekasanCd3Note { get; private set; }

        public string AgekasanCd4Note { get; private set; }
        #endregion
    }

    public class PrecriptionSettingTabModel
    {
        public PrecriptionSettingTabModel(List<M10DayLimitModel> m10DayLimits, List<IpnMinYakkaMstModel> ipnMinYakkaMsts, List<DrugDayLimitModel> drugDayLimits, DosageMstModel dosageMst, IpnNameMstModel ipnNameMst)
        {
            M10DayLimits = m10DayLimits;
            IpnMinYakkaMsts = ipnMinYakkaMsts;
            DrugDayLimits = drugDayLimits;
            DosageMst = dosageMst;
            IpnNameMst = ipnNameMst;
        }

        public List<M10DayLimitModel> M10DayLimits { get; private set; }

        public List<IpnMinYakkaMstModel> IpnMinYakkaMsts { get; private set; }

        public List<DrugDayLimitModel> DrugDayLimits { get; private set; }

        public DosageMstModel DosageMst { get; private set; }

        public IpnNameMstModel IpnNameMst { get; private set; }
    }

    public class UsageSettingTabModel
    {
        [JsonConstructor]
        public UsageSettingTabModel(string yohoInfMstPrefix)
        {
            YohoInfMstPrefix = yohoInfMstPrefix;
        }

        public UsageSettingTabModel()
        {
            YohoInfMstPrefix = string.Empty;
        }

        public string YohoInfMstPrefix { get; private set; }
    }

    public class DrugInfomationTabModel
    {
        public DrugInfomationTabModel(List<DrugInfModel> drugInfs, PiImageModel zaiImage, PiImageModel houImage)
        {
            DrugInfs = drugInfs;
            ZaiImage = zaiImage;
            HouImage = houImage;
        }

        public List<DrugInfModel> DrugInfs { get; private set; }

        public PiImageModel ZaiImage { get; private set; }

        public PiImageModel HouImage { get; private set; }
    }

    public class TeikyoByomeiTabModel
    {
        public TeikyoByomeiTabModel(List<TeikyoByomeiModel> teikyoByomeis, TekiouByomeiMstExcludedModel tekiouByomeiMstExcluded)
        {
            TeikyoByomeis = teikyoByomeis;
            TekiouByomeiMstExcluded = tekiouByomeiMstExcluded;
        }

        public List<TeikyoByomeiModel> TeikyoByomeis { get; private set; }

        public TekiouByomeiMstExcludedModel TekiouByomeiMstExcluded { get; private set; }
    }

    public class SanteiKaishuTabModel
    {
        public SanteiKaishuTabModel(List<DensiSanteiKaisuModel> densiSanteiKaisus)
        {
            DensiSanteiKaisus = densiSanteiKaisus;
        }

        public List<DensiSanteiKaisuModel> DensiSanteiKaisus { get; private set; }
    }

    public class HaihanTabModel
    {
        public HaihanTabModel(List<DensiHaihanModel> densiHaihanModel1s, List<DensiHaihanModel> densiHaihanModel2s, List<DensiHaihanModel> densiHaihanModel3s)
        {
            DensiHaihanModel1s = densiHaihanModel1s;
            DensiHaihanModel2s = densiHaihanModel2s;
            DensiHaihanModel3s = densiHaihanModel3s;
        }

        public List<DensiHaihanModel> DensiHaihanModel1s { get; private set; }

        public List<DensiHaihanModel> DensiHaihanModel2s { get; private set; }

        public List<DensiHaihanModel> DensiHaihanModel3s { get; private set; }
    }

    public class HoukatsuTabModel
    {
        public HoukatsuTabModel(List<DensiHoukatuModel> listDensiHoukatuModels, List<DensiHoukatuGrpModel> listDensiHoukatuGrpModels, List<DensiHoukatuModel> listDensiHoukatuMaster)
        {
            ListDensiHoukatuModels = listDensiHoukatuModels;
            ListDensiHoukatuGrpModels = listDensiHoukatuGrpModels;
            ListDensiHoukatuMaster = listDensiHoukatuMaster;
        }

        public List<DensiHoukatuModel> ListDensiHoukatuModels { get; private set; }

        public List<DensiHoukatuGrpModel> ListDensiHoukatuGrpModels { get; private set; }

        public List<DensiHoukatuModel> ListDensiHoukatuMaster { get; private set; }
    }

    public class CombinedContraindicationTabModel
    {
        public CombinedContraindicationTabModel(List<CombinedContraindicationModel> combinedContraindications)
        {
            CombinedContraindications = combinedContraindications;
        }

        public List<CombinedContraindicationModel> CombinedContraindications { get; private set; }
    }
}
