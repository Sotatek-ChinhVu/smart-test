namespace Domain.Models.NextOrder
{
    public interface INextOrderRepository
    {
        List<RsvkrtByomeiModel> GetByomeis(int hpId, long ptId, long rsvkrtNo, int type);

        RsvkrtKarteInfModel GetKarteInf(int hpId, long ptId, long rsvkrtNo);

        List<RsvkrtOrderInfModel> GetOrderInfs(int hpId, long ptId, long rsvkrtNo, int sinDate, int userId);

        List<NextOrderFileModel> GetNextOrderFiles(int hpId, long ptId, long rsvkrtNo);

        long GetLastSeqNo(int hpId, long ptId, long rsvkrtNo);

        List<NextOrderModel> GetList(int hpId, long ptId, int rsvkrtKbn, bool isDeleted);
    }
}
