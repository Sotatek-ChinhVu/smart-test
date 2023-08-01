namespace Infrastructure.Interfaces
{
    public interface ISystemConfigService
    {
        void Reload();

        double GetConfigValue(int grpCd, int grpEdaNo);

        void Dispose();
    }
}
