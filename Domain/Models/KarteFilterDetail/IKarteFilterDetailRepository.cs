namespace Domain.Models.KarteFilterDetail;

public interface IKarteFilterDetailRepository
{
    List<KarteFilterDetail> GetList(int hpId, int userId);
}
