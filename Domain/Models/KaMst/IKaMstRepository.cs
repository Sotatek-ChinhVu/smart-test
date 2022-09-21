namespace Domain.Models.KaMst;

public interface IKaMstRepository
{
    KaMstModel? GetByKaId(int kaId);
    List<KaMstModel> GetByKaIds(List<int> kaIds);
    List<KaMstModel> GetList();
    bool CheckKaId(int kaId);
}
