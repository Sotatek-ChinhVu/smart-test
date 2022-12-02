namespace EmrCloudApi.Responses.PatientInfor.InsuranceMasterLinkage
{
    public class SaveInsuranceMasterLinkageResponse
    {
        public SaveInsuranceMasterLinkageResponse(bool success)
        {
            Success = success;
        }

        public bool Success { get; private set; }
    }
}
