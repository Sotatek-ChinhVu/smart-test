using Domain.Models.Family;
using Domain.Models.KarteInfs;
using Domain.Models.Medical;
using Domain.Models.OrdInfs;
using Domain.Models.TodayOdr;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SaveMedicalRepository : RepositoryBase, ISaveMedicalRepository
{
    private readonly IFamilyRepository _familyRepository;
    private readonly ITodayOdrRepository _todayOdrRepository;

    public SaveMedicalRepository(ITenantProvider tenantProvider, IFamilyRepository familyRepository, ITodayOdrRepository todayOdrRepository) : base(tenantProvider)
    {
        _familyRepository = familyRepository;
        _todayOdrRepository = todayOdrRepository;
    }

    public bool Upsert(int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, byte status, List<OrdInfModel> odrInfs, KarteInfModel karteInfModel, int userId, List<FamilyModel> familyList)
    {
        var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();

        return executionStrategy.Execute(
            () =>
            {
                using var transaction = TrackingDataContext.Database.BeginTransaction();
                try
                {
                    if (!_todayOdrRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, odrInfs, karteInfModel, userId, status))
                    {
                        transaction.Rollback();
                        return false;
                    }
                    if (!_familyRepository.SaveFamilyList(hpId, userId, familyList))
                    {
                        transaction.Rollback();
                        return false;
                    }
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            });
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
