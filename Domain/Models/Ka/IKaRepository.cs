namespace Domain.Models.Ka;

public interface IKaRepository
{
    KaMstModel GetByKaId(int kaId);

    List<KaMstModel> GetByKaIds(List<int> kaIds);

    List<KaMstModel> GetList();

    List<KaCodeMstModel> GetListKacode();

    bool SaveKaMst(int hpId, int userId, List<KaMstModel> kaMstModels);

    bool CheckKaId(int kaId);

}
