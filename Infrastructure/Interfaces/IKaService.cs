using Entity.Tenant;

namespace Infrastructure.Interfaces;

public interface IKaService
{
    void Reload();

    string GetNameById(int id);

    List<KaMst> AllKaMstList();

    void DisposeSource();
}
