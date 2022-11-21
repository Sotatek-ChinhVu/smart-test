namespace Domain.Models.NextOrder
{
    public interface INextOrderRepository
    {
        List<RsvkrtByomeiModel> GetByomeis(int hpId, long ptId, long rsvkrtNo, int type);

        public RsvkrtKarteInfModel GetKarteInf(int hpId, long ptId, long rsvkrtNo);

        public List<RsvkrtOrderInfModel> GetOrderInfs(int hpId, long ptId, long rsvkrtNo, int sinDate, int userId);

        List<NextOrderModel> GetList(int hpId, long ptId, int rsvkrtKbn, bool isDeleted);

        bool Upsert(int userId, int hpId, long ptId, long rsvkrtNo, int rsvDate, List<NextOrderModel> nextOrderModels);
    }
}
