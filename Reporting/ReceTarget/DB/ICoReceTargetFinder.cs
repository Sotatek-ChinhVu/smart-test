using Domain.Common;
using Reporting.ReceTarget.Model;

namespace Reporting.ReceTarget.DB;

public interface ICoReceTargetFinder : IRepositoryBase
{
    CoReceTargetModel FindReceInf(int hpId, int seikyuYm);
}
