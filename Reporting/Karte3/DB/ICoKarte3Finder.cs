using Domain.Common;
using Reporting.Karte3.Model;

namespace Reporting.Karte3.DB;

public interface ICoKarte3Finder : IRepositoryBase
{
    CoPtInfModel FindPtInf(int hpId, long ptId);

    CoPtHokenInfModel FindPtHoken(int hpId, long ptId, int hokenId, int sinDate);

    List<CoKaikeiInfModel> FindKaikeiInf(int hpId, long ptId, int startYm, int endYm, int hokenId);

    List<CoKaikeiInfModel> FindKaikeiInf(int hpId, long ptId, int startYm, int endYm, List<int> hokenIds);

    HashSet<string> FindKohiInf(int hpId, long ptId, int startYm, int endYm, int hokenId);

    List<CoSinKouiModel> FindSinKoui(int hpId, long ptId, int startSinYm, int endSinYm, bool includeHoken, bool includeJihi);
}