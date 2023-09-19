namespace EmrCloudApi.Responses.MedicalExamination
{
    public class UpdateKensaMstResponse
    {
        public UpdateKensaMstResponse(bool success) 
        {
            Success = success;
        }

        public bool Success { get; set; }
    }
}
