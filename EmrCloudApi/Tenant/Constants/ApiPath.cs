namespace EmrCloudApi.Tenant.Constants
{
    public static class ApiPath
    {
        public const string Get = "Get";
        public const string GetList = "GetList";
        public const string Update = "Update";
        public const string Insert = "Insert";
        public const string Upsert = "Upsert";
        public const string UpsertList = "UpsertList";
        public const string SaveList = "SaveList";
        public const string Save = "Save";
        public const string Revert = "Revert";
        public const string Delete = "Delete";


        // SuperSet
        public const string Validate = "Validate";
        public const string Reorder = "Reorder";
        public const string Paste = "Paste";
        public const string GetSuperSetDetail = "GetSuperSetDetail";
        public const string DiseaseSearch = "DiseaseSearch";
        public const string SaveSuperSetDetail = "SaveSuperSetDetail";

        //Mst Item
        public const string GetDosageDrugList = "GetDosageDrugList";
        public const string GetFoodAlrgy = "GetFoodAlrgy";
        public const string SearchOTC = "SearchOTC";
        public const string SearchSupplement = "SearchSupplement";
        public const string SearchTenItem = "SearchTenItem";
        public const string UpdateAdoptedInputItem = "UpdateAdoptedInputItem";
        public const string UpdateAdoptedByomei = "UpdateAdoptedByomei";
        public const string FindTenMst = "FindTenMst";

        //Schema
        public const string SaveImageTodayOrder = "SaveImageTodayOrder";
        public const string SaveImageSuperSetDetail = "SaveImageSuperSetDetail";

        //Special Note
        public const string AddAlrgyDrugList = "AddAlrgyDrugList";

        //Today Oder
        public const string GetMaxRpNo = "GetMaxRpNo";
        public const string GetHeaderInf = "GetHeaderInf";

        // KaCode
        public const string GetListKaCode = "GetListKaCode";
        public const string SaveListKaMst = "SaveListKaMst";

        //PostCode
        public const string SearchPostCode = "SearchPostCode";

        //PatientGroupMst
        public const string SavePatientGroupMst = "SavePatientGroupMst";

        // ExportReport
        public const string ExportKarte1 = "ExportKarte1";

        //PatientInfor
        public const string SearchEmptyId = "SearchEmptyId";
        public const string GetInsuranceMasterLinkage = "GetInsuranceMasterLinkage";

        //HokenMst
        public const string GetDetailHokenMst = "GetDetailHokenMst";

        //Validate Main Insurance
        public const string ValidateMainInsurance = "ValidateMainInsurance";

        //Validate Insurance rousai jibai
        public const string ValidateRousaiJibai = "ValidateRousaiJibai";

        //Validate Kohi
        public const string ValidateKohi = "ValidateKohi";

        // Validate Insurance Other
        public const string ValidateInsuranceOther = "ValidateInsuranceOther";

        //Validate InputItem
        public const string ValidateInputItem = "ValidateInputItem";


        //Drug Infor
        public const string GetDrugMenuTree = "GetDrugMenuTree";
        public const string DrugDataSelectedTree = "DrugDataSelectedTree";

        //PtKyuseiInf
        public const string GetPtKyuseiInf = "GetPtKyuseiInf";
    }
}
