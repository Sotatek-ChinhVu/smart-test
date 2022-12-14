using Domain.Models.KarteFilterMst;

namespace Domain.Models.HistoryOrder
{
    public interface IHistoryOrderRepository
    {
        (int, List<HistoryOrderModel>) GetList(int hpId, int userId, long ptId, int sinDate, int offset, int limit, int filterId, int isDeleted);

        KarteFilterMstModel GetFilter(int hpId, int userId, int filterId);

        bool CheckExistedFilter(int hpId, int userId, int filterId);
    }
}
