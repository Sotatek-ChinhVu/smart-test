namespace Domain.Models.KensaInfDetail
{
    public interface IKensaInfDetailRepository
    {
        List<KensaInfDetailModel> GetList(int hpId, long ptId, int sinDate);
    }
}
