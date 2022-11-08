namespace Domain.Models.NextOrder
{
    public interface INextOrderRepository
    {
        List<NextOrderModel> GetList(int hpId, long ptId, int rsvkrtKbn, bool isDeleted);
    }
}
