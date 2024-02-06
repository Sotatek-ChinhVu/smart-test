using Domain.Common;

namespace Domain.Models.Ka;

public interface IKaRepository : IRepositoryBase
{
    KaMstModel GetByKaId(int kaId);

    List<KaMstModel> GetList(int isDeleted);

    List<KaCodeMstModel> GetListKacode(int hpId);

    bool SaveKaMst(int hpId, int userId, List<KaMstModel> kaMstModels);

    bool CheckKaId(int kaId);

    bool CheckKaId(List<int> kaIds);

    List<KaCodeMstModel> GetKacodeMstYossi(int hpId);

    List<KacodeYousikiMstModel> GetKacodeYousikiMst(int hpId);
}
