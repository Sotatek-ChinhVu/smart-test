using Domain.Common;
using Reporting.Karte1.Model;

namespace Reporting.Karte1.DB;

public interface ICoKarte1Finder : IRepositoryBase
{
    CoPtInfModel FindPtInf(int hpId, long ptId, int sinDate);

    List<CoPtByomeiModel> FindPtByomei(int hpId, long ptId, int hokenPid, bool TenkiByomei);

    CoPtHokenInfModel FindPtHokenInf(int hpId, long ptId, int hokenId, int sinDate);
}
