using System.Linq.Expressions;

namespace Domain.Models.PtTag
{
    public interface IPtTagRepository
    {
        List<StickyNoteModel> SearchByPtId(int hpId, int ptId);
        bool UpdateIsDeleted(int hpId, int ptId, int seqNo,int isDeleted, int userId);
        bool SaveStickyNote(List<StickyNoteModel> stickyNoteModels, int userId);
        StickyNoteModel GetStickyNoteModel(int hpId,long ptId,long seqNo);
    }
}
