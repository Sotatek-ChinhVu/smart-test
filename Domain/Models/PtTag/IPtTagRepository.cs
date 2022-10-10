using System.Linq.Expressions;

namespace Domain.Models.PtTag
{
    public interface IPtTagRepository
    {
        List<StickyNoteModel> SearchByPtId(int hpId, int ptId, int isDeleted);
        bool UpdateIsDeleted(int hpId, int ptId, int seqNo,int isDeleted);
    }
}
