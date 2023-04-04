using Domain.Common;
using Domain.Models.KarteFilterMst;
using Domain.Models.Reception;

namespace Domain.Models.HistoryOrder
{
    public interface IHistoryOrderRepository : IRepositoryBase
    {
        (int, List<HistoryOrderModel>) GetList(int hpId, int userId, long ptId, int sinDate, int offset, int limit, int filterId, int isDeleted);

        public (int, ReceptionModel) Search(int hpId, int userId, long ptId, int sinDate, int currentIndex, int filterId, int isDeleted, string keyWord, int searchType, bool isNext);

        KarteFilterMstModel GetFilter(int hpId, int userId, int filterId);

        bool CheckExistedFilter(int hpId, int userId, int filterId);

        long GetHistoryIndex(int hpId, long ptId, long raiinNo, int userId, int filterId, int isDeleted);

        List<HistoryOrderModel> GetListByRaiin(int hpId, int userId, long ptId, int sinDate, int filterId, int isDeleted, long raiinNo);

        (int totalCount, List<HistoryOrderModel> historyOrderModels) GetOrdersForOneOrderSheetGroup(int hpId, long ptId, int odrKouiKbn, int grpKouiKbn, int sinDate, int offset, int limit);
    }
}
