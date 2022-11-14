namespace Domain.Models.NextOrder
{
    public interface INextOrder
    {
        (List<RsvkrtByomeiModel> byomeis, RsvkrtKarteInfModel karteInf, List<RsvkrtOrderInfModel> orderInfs) Get(int hpId, long ptId, long rsvkrtNo, int type);
    }
}
