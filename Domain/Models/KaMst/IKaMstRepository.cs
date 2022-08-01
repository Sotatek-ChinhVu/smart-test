namespace Domain.Models.KaMst;

public interface IKaMstRepository
{
    KaMstModel? GetByKaId(int kaId);
}
