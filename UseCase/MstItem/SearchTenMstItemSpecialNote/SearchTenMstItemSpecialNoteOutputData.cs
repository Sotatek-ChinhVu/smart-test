using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SearchTenMstItemSpecialNote
{
    public class SearchTenMstItemSpecialNoteOutputData : IOutputData
    {
        public SearchTenMstItemSpecialNoteOutputData(List<TenItemModel> listInputModel, int totalCount, SearchTenMstItemSpecialNoteStatus status)
        {
            ListInputModel = listInputModel;
            Status = status;
            TotalCount = totalCount;
        }

        public List<TenItemModel> ListInputModel { get; private set; }
        public int TotalCount { get; private set; }
        public SearchTenMstItemSpecialNoteStatus Status { get; private set; }
    }
}
