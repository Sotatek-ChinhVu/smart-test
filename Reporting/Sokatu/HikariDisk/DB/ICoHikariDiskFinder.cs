using Reporting.Sokatu.Common.Models;

namespace Reporting.Sokatu.HikariDisk.DB;

public interface ICoHikariDiskFinder
{
    List<CoReceInfModel> GetReceInf(int hpId, int seikyuYm, int hokenKbn);

    CoHpInfModel GetHpInf(int hpId, int seikyuYm);
}
