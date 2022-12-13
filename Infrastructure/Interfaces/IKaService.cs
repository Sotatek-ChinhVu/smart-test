namespace Infrastructure.Interfaces
{
    public interface IKaService
    {
        void Reload();

        string GetNameById(int id);
    }
}
