namespace Domain.Models.NextOrder
{
    public interface INextOrderRepository
    {
        (List<RsvkrtByomeiModel> byomeis, RsvkrtKarteInfModel karteInf, List<RsvkrtOrderInfModel> orderInfs) Get(int hpId, long ptId, long rsvkrtNo, int userId, int sinDate, int type);
    }
}
