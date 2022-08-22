using Domain.Models.PhysicalInfo;
using Domain.Models.PtAlrgyDrug;
using Domain.Models.PtAlrgyElse;
using Domain.Models.PtAlrgyFood;
using Domain.Models.PtCmtInf;
using Domain.Models.PtInfection;
using Domain.Models.PtKioReki;
using Domain.Models.PtOtcDrug;
using Domain.Models.PtOtherDrug;
using Domain.Models.PtPregnancy;
using Domain.Models.PtSupple;
using Domain.Models.SeikaturekiInf;
using Domain.Models.SummaryInf;
using Domain.Models.UserConf;
using UseCase.SpecialNote.Get;

namespace Interactor.SpecialNote
{
    public class GetSpecialNoteInteractor : IGetSpecialNoteInputPort
    {
        private readonly IPtAlrgyElseRepository _ptAlrgryElseRepository;
        private readonly IPtAlrgyFoodRepository _ptPtAlrgyFoodRepository;
        private readonly IPtAlrgyDrugRepository _ptPtAlrgyDrugRepository;
        private readonly IPtKioRekiRepository _ptKioRekiRepository;
        private readonly IPtInfectionRepository _ptInfectionRepository;
        private readonly IPtOtherDrugRepository _ptOtherDrugRepository;
        private readonly IPtOtcDrugRepository _ptPtOtcDrugRepository;
        private readonly IPtSuppleRepository _ptPtSuppleRepository;
        private readonly IPtPregnancyRepository _ptPregnancyRepository;
        private readonly IPtCmtInfRepository _ptCmtInfRepository;
        private readonly ISeikaturekiInfRepository _seikaturekiInRepository;
        private readonly IPhysicalInfoRepository _physicalRepository;
        private readonly ISummaryInfRepository _summaryInfRepository;

        public GetSpecialNoteInteractor(IPtAlrgyElseRepository ptAlrgryElseRepository, IPtAlrgyFoodRepository ptPtAlrgyFoodRepository, IPtAlrgyDrugRepository ptPtAlrgyDrugRepository, IPtKioRekiRepository ptKioRekiRepository, IPtInfectionRepository ptInfectionRepository, IPtOtherDrugRepository ptOtherDrugRepository, IPtOtcDrugRepository ptPtOtcDrugRepository, IPtSuppleRepository ptPtSuppleRepository, IPtPregnancyRepository ptPregnancyRepository, IPtCmtInfRepository ptCmtInfRepository, ISeikaturekiInfRepository seikaturekiInRepository, IPhysicalInfoRepository physicalRepository, ISummaryInfRepository summaryInfRepository)
        {
            _ptAlrgryElseRepository = ptAlrgryElseRepository;
            _ptPtAlrgyFoodRepository = ptPtAlrgyFoodRepository;
            _ptPtAlrgyDrugRepository = ptPtAlrgyDrugRepository;
            _ptKioRekiRepository = ptKioRekiRepository;
            _ptInfectionRepository = ptInfectionRepository;
            _ptOtherDrugRepository = ptOtherDrugRepository;
            _ptPtOtcDrugRepository = ptPtOtcDrugRepository;
            _ptPtSuppleRepository = ptPtSuppleRepository;
            _ptPregnancyRepository = ptPregnancyRepository;
            _ptCmtInfRepository = ptCmtInfRepository;
            _seikaturekiInRepository = seikaturekiInRepository;
            _physicalRepository = physicalRepository;
            _summaryInfRepository = summaryInfRepository;
        }

        public GetSpecialNoteOutputData Handle(GetSpecialNoteInputData inputData)
        {
            if (inputData.HpId <= 0)
            {
                return new GetSpecialNoteOutputData(null, null, null, GetSpecialNoteStatus.InvalidHpId);
            }
            if (inputData.PtId <= 0)
            {
                return new GetSpecialNoteOutputData(null, null, null, GetSpecialNoteStatus.InvalidPtId);
            }

            var summaryTab = GetSummaryTab(inputData.HpId, inputData.PtId);
            var importantNoteTab = GetImportantNoteTab(inputData.PtId);
            var patientInfoTab = GetPatientInfoTab(inputData.PtId, inputData.HpId);

            return new GetSpecialNoteOutputData(summaryTab, importantNoteTab, patientInfoTab, GetSpecialNoteStatus.Successed);
        }

        #region Get data for tab
        private SummaryTabItem GetSummaryTab(int hpId, long ptId)
        {
            var summaryInf = _summaryInfRepository.GetList(hpId, ptId).FirstOrDefault();

            return new SummaryTabItem(summaryInf);
        }
        private ImportantNoteTabItem GetImportantNoteTab(long ptId)
        {
            var listPtAlrgyElseItem = _ptAlrgryElseRepository.GetList(ptId);
            var listPtAlrgyFoodItem = _ptPtAlrgyFoodRepository.GetList(ptId);
            var listPtAlrgyDrugItem = _ptPtAlrgyDrugRepository.GetList(ptId);

            var listPtOtherDrugItem = _ptOtherDrugRepository.GetList(ptId);
            var listPtOtcDrugItem = _ptPtOtcDrugRepository.GetList(ptId);
            var listPtSuppleItem = _ptPtSuppleRepository.GetList(ptId);

            var listPtKioRekiItem = _ptKioRekiRepository.GetList(ptId);
            var listPtInfectionItem = _ptInfectionRepository.GetList(ptId);

            return new ImportantNoteTabItem(listPtAlrgyFoodItem, listPtAlrgyElseItem, listPtAlrgyDrugItem, listPtKioRekiItem, listPtInfectionItem, listPtOtherDrugItem, listPtOtcDrugItem, listPtSuppleItem);
        }

        private PatientInfoTabItem GetPatientInfoTab(long ptId, int hpId)
        {
            var ptPregnancyItem = _ptPregnancyRepository.GetList(ptId, hpId).FirstOrDefault();
            var ptCmtInfItem = _ptCmtInfRepository.GetList(ptId, hpId).FirstOrDefault();
            var seikaturekiInfItem = _seikaturekiInRepository.GetList(ptId, hpId).FirstOrDefault();
            var listPhysicalItems = _physicalRepository.GetList(hpId, ptId);

            return new PatientInfoTabItem(ptPregnancyItem, ptCmtInfItem, seikaturekiInfItem, listPhysicalItems);
        }
        #endregion
    }
}
