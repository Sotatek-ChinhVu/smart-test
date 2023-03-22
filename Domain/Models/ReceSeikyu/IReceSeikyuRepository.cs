using Domain.Common;

namespace Domain.Models.ReceSeikyu
{
    public interface IReceSeikyuRepository : IRepositoryBase
    {
        List<ReceSeikyuModel> GetListReceSeikyModel(int hpId, int sinDate, int sinYm, bool isIncludingUnConfirmed, long ptNumSearch, bool noFilter, bool isFilterMonthlyDelay, bool isFilterReturn, bool isFilterOnlineReturn);

        List<ReceSeikyuModel> GetListReceSeikyModel(int hpId, int seikyuYm, List<long> ptIdList);

        IEnumerable<RegisterSeikyuModel> SearchReceInf(int hpId, long ptNum, int sinYm);

        bool InsertNewReceSeikyu(List<ReceSeikyuModel> listInsert, int userId, int hpId);

        void EntryDeleteHenJiyuu(long ptId, int sinYm, int preHokenId, int userId);

        bool SaveReceSeiKyu(int hpId, int userId, List<ReceSeikyuModel> data);

        bool RemoveReceSeikyuDuplicateIfExist(long ptId, int sinYm, int hokenId, int userId, int hpId);

        bool UpdateSeikyuYmReceipSeikyuIfExist(long ptId, int sinYm, int hokenId , int seikyuYm, int userId, int hpId);
    }
}
