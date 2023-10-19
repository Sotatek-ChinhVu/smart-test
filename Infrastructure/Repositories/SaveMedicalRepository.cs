using Domain.Models.Diseases;
using Domain.Models.Family;
using Domain.Models.FlowSheet;
using Domain.Models.KarteInfs;
using Domain.Models.Medical;
using Domain.Models.MonshinInf;
using Domain.Models.NextOrder;
using Domain.Models.OrdInfs;
using Domain.Models.SpecialNote;
using Domain.Models.SpecialNote.ImportantNote;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.SpecialNote.SummaryInf;
using Domain.Models.TodayOdr;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SaveMedicalRepository : RepositoryBase, ISaveMedicalRepository
{
    private readonly IFamilyRepository _familyRepository;
    private readonly ITodayOdrRepository _todayOdrRepository;
    private readonly INextOrderRepository _nextOrderRepository;
    private readonly ISpecialNoteRepository _specialNoteRepository;
    private readonly IPtDiseaseRepository _ptDiseaseRepository;
    private readonly IFlowSheetRepository _flowSheetRepository;
    private readonly IMonshinInforRepository _monshinInforRepository;

    public SaveMedicalRepository(ITenantProvider tenantProvider, IFamilyRepository familyRepository, ITodayOdrRepository todayOdrRepository, INextOrderRepository nextOrderRepository, ISpecialNoteRepository specialNoteRepository, IPtDiseaseRepository ptDiseaseRepository, IFlowSheetRepository flowSheetRepository, IMonshinInforRepository monshinInforRepository) : base(tenantProvider)
    {
        _familyRepository = familyRepository;
        _todayOdrRepository = todayOdrRepository;
        _nextOrderRepository = nextOrderRepository;
        _specialNoteRepository = specialNoteRepository;
        _ptDiseaseRepository = ptDiseaseRepository;
        _flowSheetRepository = flowSheetRepository;
        _monshinInforRepository = monshinInforRepository;
    }

    public bool Upsert(int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, byte status, List<OrdInfModel> odrInfs, KarteInfModel karteInfModel, int userId, List<FamilyModel> familyList, List<NextOrderModel> rsvkrtOrderInfModels, SummaryInfModel summaryInfModel, ImportantNoteModel importantNoteModel, PatientInfoModel patientInfoModel, List<PtDiseaseModel> ptDiseaseModels, List<FlowSheetModel> flowSheetData, MonshinInforModel monshin)
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

                    if (rsvkrtOrderInfModels.Count > 0)
                    {
                        _nextOrderRepository.Upsert(userId, hpId, ptId, rsvkrtOrderInfModels);
                    }

                    var specialNote = _specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, summaryInfModel, importantNoteModel, patientInfoModel, userId);
                    if (!specialNote)
                    {
                        transaction.Rollback();
                        return false;
                    }

                    _ptDiseaseRepository.Upsert(ptDiseaseModels, hpId, userId);

                    _flowSheetRepository.UpsertTag(flowSheetData, hpId, userId);
                    _flowSheetRepository.UpsertCmt(flowSheetData, hpId, userId);

                    if (familyList.Count > 0 && !_familyRepository.SaveFamilyList(hpId, userId, familyList))
                    {
                        transaction.Rollback();
                        return false;
                    }

                    if (!(monshin.HpId == 0 && monshin.RaiinNo == 0 && monshin.PtId == 0 && monshin.SeqNo == 0))
                    {
                        var checkMoshin = _monshinInforRepository.SaveMonshinSheet(monshin);
                        if (!checkMoshin)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }


                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            });
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
