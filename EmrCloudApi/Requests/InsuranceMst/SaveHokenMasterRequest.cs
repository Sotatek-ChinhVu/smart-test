namespace EmrCloudApi.Requests.InsuranceMst
{
    public class SaveHokenMasterRequest
    {
        public SaveHokenMasterRequest(HokenMasterDto insurance) => Insurance = insurance;

        public HokenMasterDto Insurance { get; private set; }
    }
}
