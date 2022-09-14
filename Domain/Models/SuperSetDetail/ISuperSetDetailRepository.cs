using Entity.Tenant;

namespace Domain.Models.SuperSetDetail;

public interface ISuperSetDetailRepository
{
    SuperSetDetailModel GetSuperSetDetail(int hpId, int setCd);

    bool SaveSetByomei(List<SetByomeiModel> setByomeis);
}