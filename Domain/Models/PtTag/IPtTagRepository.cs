using Domain.Common;

namespace Domain.Models.PtTag
{
    public interface IPtTagRepository : IRepositoryBase
    {
        List<StickyNoteModel> SearchByPtId(int hpId, long ptId);
        bool UpdateIsDeleted(int hpId, long ptId, int seqNo, int isDeleted, int userId);
        bool SaveStickyNote(List<StickyNoteModel> stickyNoteModels, int userId);
        StickyNoteModel GetStickyNoteModel(int hpId, long ptId, long seqNo);
    }
}
