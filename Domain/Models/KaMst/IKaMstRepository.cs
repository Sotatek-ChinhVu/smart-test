namespace Domain.Models.KaMst;

public interface IKaMstRepository
{
    int GetKaIdByKaSname(string kaSname);
}
