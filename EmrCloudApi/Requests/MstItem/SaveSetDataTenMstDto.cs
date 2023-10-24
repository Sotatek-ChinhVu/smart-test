namespace EmrCloudApi.Requests.MstItem
{
    public class CmtKbnMstModelDto
    {
        public long Id { get; set; }

        public int StartDate { get; set; }

        public int EndDate { get; set; }

        public int CmtKbn { get; set; }

        public string ItemCd { get; set; } = string.Empty;

        public bool IsDeleted { get; set; }
    }

    public class M10DayLimitModelDto
    {
        public string YjCd { get; set; } = string.Empty;

        public int SeqNo { get; set; }

        public int LimitDay { get; set; }

        public string StDate { get; set; } = string.Empty;

        public string EdDate { get; set; } = string.Empty;

        public string Cmt { get; set; } = string.Empty;
    }

    public class IpnMinYakkaMstModelDto
    {
        public int Id { get; set; }

        public int HpId { get; set; }

        public string IpnNameCd { get; set; } = string.Empty;

        public int StartDate { get; set; }

        public int EndDate { get; set; }

        public double Yakka { get; set; }

        public int SeqNo { get; set; }

        public int IsDeleted { get; set; }

        public bool ModelModified { get; set; }
    }

    public class DrugDayLimitModelDto
    {
        public int Id { get; set; }

        public int HpId { get; set; }

        public string ItemCd { get; set; } = string.Empty;

        public int SeqNo { get; set; }

        public int LimitDay { get; set; }

        public int StartDate { get; set; }

        public int EndDate { get; set; }

        public int IsDeleted { get; set; }

        public bool ModelModified { get; set; }
    }

    public class DosageMstModelDto
    {
        public int Id { get; set; }

        public int HpId { get; set; }

        public string ItemCd { get; set; } = string.Empty;

        public int SeqNo { get; set; }

        public double OnceMin { get; set; }

        public double OnceMax { get; set; }

        public double OnceLimit { get; set; }

        public int OnceUnit { get; set; }

        public double DayMin { get; set; }

        public double DayMax { get; set; }

        public double DayLimit { get; set; }

        public int DayUnit { get; set; }

        public int IsDeleted { get; set; }

        public bool ModelModified { get; set; }
    }

    public class IpnNameMstModelDto
    {
        public int HpId { get; set; }

        public string IpnNameCd { get; set; } = string.Empty;

        public string IpnNameCdOrigin { get; set; } = string.Empty;

        public int StartDate { get; set; }

        public int EndDate { get; set; }

        public string IpnName { get; set; } = string.Empty;

        public int SeqNo { get; set; }

        public int IsDeleted { get; set; }

        public bool ModelModified { get; set; }
    }

    public class DrugInfModelDto
    {
        public int HpId { get; set; }

        public string ItemCd { get; set; } = string.Empty;

        public int InfKbn { get; set; }

        public long SeqNo { get; set; }

        public string DrugInfo { get; set; } = string.Empty;

        public int IsDeleted { get; set; }

        public bool IsModified { get; set; }

        public string OldDrugInfo { get; set; } = string.Empty;
    }

    public class PiImageModelDto
    {
        public int HpId { get; set; }

        /// <summary>
        /// 包装剤形区分
        /// 0:剤形 1:包装
        /// </summary>
        public int ImageType { get; set; }

        public string ItemCd { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public bool IsModified { get; set; }

        public bool IsDeleted { get; set; }
    }

    public class TeikyoByomeiModelDto
    {
        public int SikkanCd { get; set; }

        public int HpId { get; set; }

        public string ItemCd { get; set; } = string.Empty;

        public string ByomeiCd { get; set; } = string.Empty;

        public int StartYM { get; set; }

        public int EndYM { get; set; }

        public int IsInvalid { get; set; }

        /// <summary>
        /// 特処無効区分
        /// 0:有効 1:無効
        /// </summary>
        public int IsInvalidTokusyo { get; set; }

        public int EditKbn { get; set; }

        public int SystemData { get; set; }

        public string Byomei { get; set; } = string.Empty;

        public string KanaName { get; set; } = string.Empty;

        public bool IsDeleted { get; set; }

        public bool IsModified { get; set; }
    }

    public class TekiouByomeiMstExcludedModelDto
    {
        public int HpId { get; set; }

        public string ItemCd { get; set; } = string.Empty;

        public int SeqNo { get; set; }

        public int IsDeleted { get; set; }
    }

    public class DensiSanteiKaisuModelDto
    {
        public int Id { get; set; }

        public int HpId { get; set; }

        public string ItemCd { get; set; } = string.Empty;

        public int UnitCd { get; set; }

        public int MaxCount { get; set; }

        public int SpJyoken { get; set; }

        public int StartDate { get; set; }

        public int EndDate { get; set; }

        public long SeqNo { get; set; }

        public int UserSetting { get; set; }

        public int TargetKbn { get; set; }

        public int TermCount { get; set; }

        public int TermSbt { get; set; }

        public int IsInvalid { get; set; }

        public long ItemGrpCd { get; set; }

        public bool IsModified { get; set; }

        public bool IsDeleted { get; set; }
    }

    public class DensiHaihanModelDto
    {
        public int InitModelType { get; set; }

        public int Id { get; set; }

        public int HpId { get; set; }

        public string ItemCd1 { get; set; } = string.Empty;

        public string OriginItemCd2 { get; set; } = string.Empty;

        public string ItemCd2 { get; set; } = string.Empty;

        public string PrevItemCd2 { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public int HaihanKbn { get; set; }

        public int SpJyoken { get; set; }

        public int StartDate { get; set; }

        public int EndDate { get; set; }

        public long SeqNo { get; set; }

        public int UserSetting { get; set; }

        public int TargetKbn { get; set; }

        public int TermCnt { get; set; }

        public int TermSbt { get; set; }

        public int IsInvalid { get; set; }

        public int ModelType { get; set; }

        public bool IsReadOnly { get; set; }

        public bool IsModified { get; set; }

        public bool IsDeleted { get; set; }
    }

    public class DensiHoukatuModelDto
    {
        public int HpId { get; set; }

        public string ItemCd { get; set; } = string.Empty;

        public int StartDate { get; set; }

        public int EndDate { get; set; }

        public int TargetKbn { get; set; }

        public long SeqNo { get; set; }

        public int HoukatuTerm { get; set; }

        public string HoukatuGrpNo { get; set; } = string.Empty;

        public int UserSetting { get; set; }

        public int IsInvalid { get; set; }

        public bool IsInvalidBinding { get; set; }

        public string Name { get; set; } = string.Empty;

        public string HoukatuGrpItemCd { get; set; } = string.Empty;

        public int SpJyoken { get; set; }

        public bool IsModified { get; set; }

        public bool IsDeleted { get; set; }
    }

    public class DensiHoukatuGrpModelDto
    {
        public int HpId { get; set; }
        
        public string HoukatuGrpNo { get; set; } = string.Empty;

        public string ItemCd { get; set; } = string.Empty;

        public int SpJyoken { get; set; }

        public int StartDate { get; set; }

        public int EndDate { get; set; }

        public long SeqNo { get; set; }

        public int UserSetting { get; set; }

        public int TargetKbn { get; set; }

        public int IsInvalid { get; set; }

        public int HoukatuTerm { get; set; }

        public string Name { get; set; } = string.Empty;

        public string HoukatuItemCd { get; set; } = string.Empty;

        public bool IsUpdate { get; set; }

        public bool IsDeleted { get; set; }
    }

    public class CombinedContraindicationModelDto
    {
        public long Id { get; set; }

        public int HpId { get; set; }

        public string ACd { get; set; } = string.Empty;

        public string BCd { get; set; } = string.Empty;

        public int SeqNo { get; set; }

        public bool IsDeleted { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsAddNew { get; set; }

        public bool IsUpdated { get; set; }

        public string OriginBCd { get; set; } = string.Empty;
    }
}
