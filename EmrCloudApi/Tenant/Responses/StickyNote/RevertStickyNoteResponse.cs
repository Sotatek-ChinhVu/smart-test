namespace EmrCloudApi.Tenant.Responses.StickyNote
{
    public class DeleteRevertStickyNoteResponse
    {
        public DeleteRevertStickyNoteResponse(bool successed)
        {
            Successed = successed;
        }

        public bool Successed { get; private set; }
    }
}
