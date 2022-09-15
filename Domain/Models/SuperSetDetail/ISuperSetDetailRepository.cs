namespace Domain.Models.SuperSetDetail;

public interface ISuperSetDetailRepository
{
    SuperSetDetailModel GetSuperSetDetail(int hpId, int setCd);

    int SaveSuperSetDetail(int setCd, int userId, int hpId, SuperSetDetailModel superSetDetailModel);
}