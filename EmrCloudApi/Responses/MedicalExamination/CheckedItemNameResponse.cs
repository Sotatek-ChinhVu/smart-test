namespace EmrCloudApi.Responses.MedicalExamination
{
    public class CheckedItemNameResponse
    {
        public CheckedItemNameResponse(Dictionary<string, string> checkedItemNames)
        {
            CheckedItemNames = checkedItemNames;
        }

        public Dictionary<string, string> CheckedItemNames { get; private set; }
    }
}
