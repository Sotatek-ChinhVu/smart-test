namespace EmrCloudApi.Responses.MedicalExamination
{
    public class ConvertInputItemToTodayOrdResponse
    {
        public ConvertInputItemToTodayOrdResponse(Dictionary<string, bool> yakkaOfOdrDetails)
        {
            YakkaOfOdrDetails = yakkaOfOdrDetails;
        }

        public Dictionary<string, bool> YakkaOfOdrDetails { get; private set; }
    }
}
