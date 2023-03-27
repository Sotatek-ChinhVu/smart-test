using Domain.Common;
using Domain.Models.Family;
using Domain.Models.KarteInfs;
using Domain.Models.OrdInfs;

namespace Domain.Models.Medical;

public interface ISaveMedicalRepository : IRepositoryBase
{
    bool Upsert(int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, byte status, List<OrdInfModel> odrInfs, KarteInfModel karteInfModel, int userId, List<FamilyModel> familyList);
}
