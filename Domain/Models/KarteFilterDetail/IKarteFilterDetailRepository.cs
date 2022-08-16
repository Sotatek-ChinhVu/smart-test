namespace Domain.Models.KarteFilterDetail;

public interface IKarteFilterDetailRepository
{
    List<KarteFilterDetailModel> GetList(int hpId, int userId);
}
