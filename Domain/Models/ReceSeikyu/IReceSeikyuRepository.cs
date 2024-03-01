using Domain.Common;

namespace Domain.Models.ReceSeikyu
{
    public interface IReceSeikyuRepository : IRepositoryBase
    {
        List<ReceSeikyuModel> GetListReceSeikyModel(int hpId, int sinDate, int sinYm, bool isIncludingUnConfirmed, long ptNumSearch, bool noFilter, bool isFilterMonthlyDelay, bool isFilterReturn, bool isFilterOnlineReturn, bool isGetDataPending);

        List<ReceSeikyuModel> GetListReceSeikyModel(int hpId, int seikyuYm, List<long> ptIdList);

        IEnumerable<RegisterSeikyuModel> SearchReceInf(int hpId, long ptNum, int sinYm);

        bool InsertNewReceSeikyu(List<ReceSeikyuModel> listInsert, int userId, int hpId);

        int InsertNewReceSeikyu(ReceSeikyuModel model, int userId, int hpId);

        bool UpdateReceSeikyu(List<ReceSeikyuModel> receSeikyuList, int userId, int hpId);

        void EntryDeleteHenJiyuu(int hpId, long ptId, int sinYm, int preHokenId, int userId);

        bool SaveReceSeiKyu(int hpId, int userId, List<ReceSeikyuModel> data);

        bool RemoveReceSeikyuDuplicateIfExist(long ptId, int sinYm, int hokenId, int userId, int hpId);

        bool UpdateSeikyuYmReceipSeikyuIfExist(long ptId, int sinYm, int hokenId, int seikyuYm, int userId, int hpId);

        bool IsReceSeikyuExisted(int hpId, long ptId, int sinYm, int hokenId);

        int GetReceSeikyuPreHoken(int hpId, long ptId, int sinYm, int hokenId);

        void DeleteReceSeikyu(int hpId, long ptId, int sinYm, int hokenId);

        void DeleteHenJiyuuRireki(int hpId, long ptId, int sinYm, int preHokenId);

        void InsertSingleReceSeikyu(int hpId, long ptId, int sinYm, int hokenId, int userId);

        void InsertSingleRerikiInf(int hpId, long ptId, int sinYm, int hokenId, string searchNo, string rireki, int userId);

        void InsertSingleHenJiyuu(int hpId, long ptId, int sinYm, int hokenId, string hosoku, string henreiJiyuuCd, string henreiJiyuu, int userId);

        ReceSeikyuModel GetReceSeikyuDuplicate(int hpId, long ptId, int sinYm, int hokenId);

        bool SaveChangeImportFileRececeikyus();

        ReceSeikyuModel GetReceSeikyModelByPtNum(int hpId, int sinDate, int sinYm, long ptNum);

        List<RecedenHenJiyuuModel> GetRecedenHenJiyuuModels(int hpId, long ptId, int sinYm);
    }
}
