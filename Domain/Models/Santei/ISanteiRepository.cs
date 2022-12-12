namespace Domain.Models.Santei;

public interface ISanteiRepository
{
    IEnumerable<SanteiOrderDetailModel> CheckAutoAddOrderItem(int hpId, string itemCd, int sinDate);
}
