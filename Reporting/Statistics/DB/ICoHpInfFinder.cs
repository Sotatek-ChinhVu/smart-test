using Reporting.Statistics.Model;

namespace Reporting.Statistics.DB;

public interface ICoHpInfFinder
{
    CoHpInfModel GetHpInf(int hpId, int sinDate);
}
