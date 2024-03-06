using Domain.Common;

namespace Domain.Models.Ka;

public interface IKaRepository : IRepositoryBase
{
    KaMstModel GetByKaId(int hpId, int kaId);

    List<KaMstModel> GetList(int hpId, int isDeleted);

    List<KaCodeMstModel> GetListKacode(int hpId);

    bool SaveKaMst(int hpId, int userId, List<KaMstModel> kaMstModels);

    bool CheckKaId(int hpId, int kaId);

    bool CheckKaId(List<int> kaIds, int hpId);

    List<KaCodeMstModel> GetKacodeMstYossi(int hpId);

    List<KacodeYousikiMstModel> GetKacodeYousikiMst(int hpId);
}
