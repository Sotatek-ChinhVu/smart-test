namespace EmrCloudApi.Responses.MedicalExamination
{
    public class UpsertMaterialMasterResponse
    {
        public UpsertMaterialMasterResponse(bool success)
        {
            Success = success;
        }

        public bool Success { get; set; }
    }
}
