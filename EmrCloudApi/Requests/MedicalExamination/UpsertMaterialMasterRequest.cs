using Helper.Constants;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class UpsertMaterialMasterRequest
    {
        public List<MaterialMasterRequest> MaterialMasterList { get; set; } = new List<MaterialMasterRequest>();
    }

    public class MaterialMasterRequest
    {
        public long MaterialCd { get; set; }

        public string MaterialName { get; set; } = string.Empty;

        public ModelStatus MaterialModelStatus { get; set; }
    }
}
