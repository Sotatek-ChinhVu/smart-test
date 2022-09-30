namespace Domain.Models.SuperSetDetail;

public interface ISuperSetDetailRepository
{
    SuperSetDetailModel GetSuperSetDetail(int hpId, int setCd, int sindate);
}