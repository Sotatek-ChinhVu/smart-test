using Domain.Common;

namespace Domain.Models.NextOrder
{
    public interface INextOrderRepository : IRepositoryBase
    {
        List<RsvkrtByomeiModel> GetByomeis(int hpId, long ptId, long rsvkrtNo, int type);

        RsvkrtKarteInfModel GetKarteInf(int hpId, long ptId, long rsvkrtNo);

        List<RsvkrtOrderInfModel> GetOrderInfs(int hpId, long ptId, long rsvkrtNo, int sinDate, int userId);

        List<NextOrderFileModel> GetNextOrderFiles(int hpId, long ptId, long rsvkrtNo);

        long GetLastNextOrderSeqNo(int hpId, long ptId);

        List<NextOrderModel> GetList(int hpId, long ptId, int rsvkrtKbn, bool isDeleted);

        long Upsert(int userId, int hpId, long ptId, List<NextOrderModel> nextOrderModels);

        List<RsvkrtOrderInfModel> GetCheckOrderInfs(int hpId, long ptId);

        bool SaveListFileNextOrder(int hpId, long ptId, long rsvkrtNo, List<string> listFileName, bool saveTempFile);

        bool ClearTempData(int hpId, long ptId, List<string> listFileNames);
    }
}
