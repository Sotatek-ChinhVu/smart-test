namespace EmrCloudApi.Responses.MedicalExamination
{
    public class GetMaxAuditTrailLogDateForPrintResponse
    {
        public GetMaxAuditTrailLogDateForPrintResponse(Dictionary<string, DateTime> values)
        {
            Values = values;
        }

        public Dictionary<string, DateTime> Values { get; private set; }
    }
}
