using Reporting.ReceTarget.Model;

namespace Reporting.ReceTarget.DB;

public interface ICoReceTargetFinder
{
    CoReceTargetModel FindReceInf(int hpId, int seikyuYm);
}
