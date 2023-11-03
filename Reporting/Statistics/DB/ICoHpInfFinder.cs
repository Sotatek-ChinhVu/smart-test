using Domain.Common;
using Reporting.Statistics.Model;

namespace Reporting.Statistics.DB;

public interface ICoHpInfFinder : IRepositoryBase
{
    CoHpInfModel GetHpInf(int hpId, int sinDate);
}
