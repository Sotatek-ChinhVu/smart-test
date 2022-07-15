
using Domain.Models.SpecialNote;

namespace EmrCloudApi.Tenant.Responses.SpecialNote
{
    public class GetSpecialNoteResponse
    {
        public SpecialNoteDTO SpecialNote { get; set; } = new SpecialNoteDTO();
    }
}
