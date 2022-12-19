using Domain.Models.KarteFilterMst;
using Domain.Models.Reception;

namespace Domain.Models.HistoryOrder
{
    public interface IHistoryOrderRepository
    {
        (int, List<HistoryOrderModel>) GetList(int hpId, int userId, long ptId, int sinDate, int offset, int limit, int filterId, int isDeleted);

        public (int, ReceptionModel) Search(int hpId, int userId, long ptId, int sinDate, int currentIndex, int filterId, int isDeleted, string keyWord, int searchType, bool isNext);

        KarteFilterMstModel GetFilter(int hpId, int userId, int filterId);

        bool CheckExistedFilter(int hpId, int userId, int filterId);
    }
}
