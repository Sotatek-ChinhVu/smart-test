using Domain.Common;

namespace Domain.Models.ReceSeikyu
{
    public interface IReceSeikyuRepository : IRepositoryBase
    {
        List<ReceSeikyuModel> GetListReceSeikyModel(int hpId, int sinDate, int sinYm, bool isIncludingUnConfirmed, long ptNumSearch, bool noFilter, bool isFilterMonthlyDelay, bool isFilterReturn, bool isFilterOnlineReturn);

        List<ReceSeikyuModel> GetListReceSeikyModel(int hpId, int seikyuYm, List<long> ptIdList);

        bool SaveReceSeiKyu(int hpId, string userId, List<ReceSeikyuModel> data);
    }
}
