namespace EmrCloudApi.Tenant.Responses.SpecialNote
{
    public class SaveSpecialNoteResponse
    {
        public SaveSpecialNoteResponse(bool success)
        {
            Success = success;
        }

        public bool Success { get; private set; }

    }
}
