namespace EmrCloudApi.Tenant.Responses.StickyNote
{
    public class ActionStickyNoteResponse
    {
        public ActionStickyNoteResponse(bool successed)
        {
            Successed = successed;
        }

        public bool Successed { get; private set; }
    }
}
