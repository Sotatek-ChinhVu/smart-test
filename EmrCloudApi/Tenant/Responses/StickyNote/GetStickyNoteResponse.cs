using Domain.Models.PtTag;

namespace EmrCloudApi.Tenant.Responses.StickyNote
{
    public class GetStickyNoteResponse
    {
        public GetStickyNoteResponse(List<StickyNoteModel> stickyNoteModels)
        {
            StickyNoteModels = stickyNoteModels;
        }

        public List<StickyNoteModel> StickyNoteModels { get; private set; }
    }
    public class RevertStickyNoteResponse
    {

    }
}
