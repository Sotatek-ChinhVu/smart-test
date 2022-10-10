using System.Linq.Expressions;

namespace Domain.Models.PtTag
{
    public interface IPtTagRepository
    {
        List<StickyNoteModel> SearchByPtId(int hpId, int ptId, int isDeleted);
    }
}
