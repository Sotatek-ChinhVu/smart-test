using Domain.Common;
using Domain.Models.KarteFilterMst;
using Domain.Models.Receipt.Recalculation;
using Domain.Models.Reception;

namespace Domain.Models.HistoryOrder
{
    public interface IHistoryOrderRepository : IRepositoryBase
    {
        (int, List<HistoryOrderModel>) GetList(int hpId, int userId, long ptId, int sinDate, int offset, int limit, int filterId, int isDeleted, List<Tuple<long, bool>> raiinNos, int isShowApproval = 0, int type = 0);

        (int totalCount, List<HistoryOrderModel> historyOrderModelList) GetList(int hpId, long ptId, int sinDate, int startDate, int endDate, int isDeleted, int isShowApproval = 0, int type = 0);

        public (int, ReceptionModel) Search(int hpId, int userId, long ptId, int sinDate, int currentIndex, int filterId, int isDeleted, string keyWord, int searchType, bool isNext, List<Tuple<long, bool>> raiinNos);

        KarteFilterMstModel GetFilter(int hpId, int userId, int filterId);

        bool CheckExistedFilter(int hpId, int userId, int filterId);

        long GetHistoryIndex(int hpId, long ptId, long raiinNo, int userId, int filterId, int isDeleted, List<Tuple<long, bool>> raiinNos);

        List<HistoryOrderModel> GetListByRaiin(int hpId, int userId, long ptId, int sinDate, int filterId, int isDeleted, long raiinNo, byte isKarteInf, List<Tuple<long, bool>> raiinNos, int isShowApproval);

        (int totalCount, List<HistoryOrderModel> historyOrderModels) GetOrdersForOneOrderSheetGroup(int hpId, long ptId, int odrKouiKbn, int grpKouiKbn, int sinDate, int offset, int limit);

        List<SinKouiListModel> GetSinkouiList(int hpId, long ptId, List<int> sinDateList, List<long> raiinNoList, List<int> mainPidList);

        void Dispose();
    }
}
