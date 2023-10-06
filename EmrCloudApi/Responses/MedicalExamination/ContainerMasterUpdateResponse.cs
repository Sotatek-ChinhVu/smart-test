namespace EmrCloudApi.Responses.MedicalExamination
{
    public class ContainerMasterUpdateResponse
    {
        public ContainerMasterUpdateResponse(bool success)
        {
            Success = success;
        }

        public bool Success { get; set; }
    }
}
