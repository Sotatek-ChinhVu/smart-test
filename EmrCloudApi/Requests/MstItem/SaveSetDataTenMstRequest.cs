using Domain.Models.MstItem;

namespace EmrCloudApi.Requests.MstItem
{
    public class SaveSetDataTenMstRequest
    {
        public string ItemCd { get; set; } = string.Empty;

        public List<TenMstOriginModelDto> TenOrigins { get; set; } = new List<TenMstOriginModelDto>();

        #region BasicSettingTabModel
        public List<CmtKbnMstModelDto> CmtKbnMstModels { get; set; } = new List<CmtKbnMstModelDto>();
        #endregion

        #region PrecriptionSettingTab
        public List<M10DayLimitModelDto> M10DayLimitModels { get; set; } = new List<M10DayLimitModelDto>();

        public List<IpnMinYakkaMstModelDto> IpnMinYakkaMsts { get; set; } = new List<IpnMinYakkaMstModelDto>();

        public List<DrugDayLimitModelDto> DrugDayLimits { get; set; } = new List<DrugDayLimitModelDto>();

        public DosageMstModelDto DosageMst { get; set; } = new();

        public IpnNameMstModelDto IpnNameMst { get; set; } = new();
        #endregion

        #region DrugInfomationTab
        public List<DrugInfModelDto> DrugInfs { get; set; } = new List<DrugInfModelDto>();

        public PiImageModelDto ZaiImage { get; set; } = new();

        public PiImageModelDto HouImage { get; set; } = new();
        #endregion

        #region TeikyoByomeiTab
        public List<TeikyoByomeiModelDto> TeikyoByomeis { get; set; } = new List<TeikyoByomeiModelDto>();

        public TekiouByomeiMstExcludedModelDto TekiouByomeiMstExcluded { get; set; } = new();
        #endregion

        #region SanteiKaishuTab
        public List<DensiSanteiKaisuModelDto> DensiSanteiKaisus { get; set; } = new List<DensiSanteiKaisuModelDto>();
        #endregion

        #region HaihanTab
        public List<DensiHaihanModelDto> DensiHaihanModel1s { get; set; } = new List<DensiHaihanModelDto>();

        public List<DensiHaihanModelDto> DensiHaihanModel2s { get; set; } = new List<DensiHaihanModelDto>();

        public List<DensiHaihanModelDto> DensiHaihanModel3s { get; set; } = new List<DensiHaihanModelDto>();
        #endregion

        #region HoukatsuTab
        public List<DensiHoukatuModelDto> ListDensiHoukatuModels { get; set; } = new List<DensiHoukatuModelDto>();

        public List<DensiHoukatuGrpModelDto> ListDensiHoukatuGrpModels { get; set; } = new List<DensiHoukatuGrpModelDto>();

        public List<DensiHoukatuModelDto> ListDensiHoukatuMaster { get; set; } = new List<DensiHoukatuModelDto>();
        #endregion

        #region CombinedContraindicationTab
        public List<CombinedContraindicationModelDto> CombinedContraindications { get; set; } = new List<CombinedContraindicationModelDto>();
        #endregion

        #region usageSettingTab
        public UsageSettingTabModel UsageSettingTabModel { get; set; } = new();
        #endregion

        #region
        public IjiSettingTabModel IjiSettingTabModel { get; set; } = new();
        #endregion
    }
}
