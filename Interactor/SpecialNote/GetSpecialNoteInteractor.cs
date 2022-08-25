using Domain.Models.PtCmtInf;
using Domain.Models.SpecialNote.ImportantNote;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.SpecialNote.SummaryInf;
using UseCase.SpecialNote.Get;

namespace Interactor.SpecialNote
{
    public class GetSpecialNoteInteractor : IGetSpecialNoteInputPort
    {
        private readonly IPtCmtInfRepository _ptCmtInfRepository;
        private readonly ISummaryInfRepository _summaryInfRepository;
        private readonly IImportantNoteRepository _importantNoteRepository;
        private readonly IPatientInfoRepository _patientInfoRepository;

        public GetSpecialNoteInteractor(IPtCmtInfRepository ptCmtInfRepository, ISummaryInfRepository summaryInfRepository, IImportantNoteRepository importantNoteRepository, IPatientInfoRepository patientInfoRepository)
        {
            _ptCmtInfRepository = ptCmtInfRepository;
            _summaryInfRepository = summaryInfRepository;
            _importantNoteRepository = importantNoteRepository;
            _patientInfoRepository = patientInfoRepository;
        }

        public GetSpecialNoteOutputData Handle(GetSpecialNoteInputData inputData)
        {
            if (inputData.HpId <= 0)
            {
                return new GetSpecialNoteOutputData(GetSpecialNoteStatus.InvalidHpId);
            }
            if (inputData.PtId <= 0)
            {
                return new GetSpecialNoteOutputData(GetSpecialNoteStatus.InvalidPtId);
            }

            var summaryTab = GetSummaryTab(inputData.HpId, inputData.PtId);
            var importantNoteTab = GetImportantNoteTab(inputData.PtId);
            var patientInfoTab = GetPatientInfoTab(inputData.PtId, inputData.HpId);

            return new GetSpecialNoteOutputData(summaryTab, importantNoteTab, patientInfoTab, GetSpecialNoteStatus.Successed);
        }

        #region Get data for tab
        private SummaryInfModel GetSummaryTab(int hpId, long ptId)
        {
            return _summaryInfRepository.GetList(hpId, ptId);
        }
        private ImportantNoteModel GetImportantNoteTab(long ptId)
        {
            var listPtAlrgyElseItem = _importantNoteRepository.GetAlrgyElseList(ptId);
            var listPtAlrgyFoodItem = _importantNoteRepository.GetAlrgyFoodList(ptId);
            var listPtAlrgyDrugItem = _importantNoteRepository.GetAlrgyDrugList(ptId);

            var listPtOtherDrugItem = _importantNoteRepository.GetOtherDrugList(ptId);
            var listPtOtcDrugItem = _importantNoteRepository.GetOtcDrugList(ptId);
            var listPtSuppleItem = _importantNoteRepository.GetSuppleList(ptId);

            var listPtKioRekiItem = _importantNoteRepository.GetKioRekiList(ptId);
            var listPtInfectionItem = _importantNoteRepository.GetInfectionList(ptId);

            return new ImportantNoteModel(listPtAlrgyFoodItem, listPtAlrgyElseItem, listPtAlrgyDrugItem, listPtKioRekiItem, listPtInfectionItem, listPtOtherDrugItem, listPtOtcDrugItem, listPtSuppleItem);
        }

        private PatientInfoModel GetPatientInfoTab(long ptId, int hpId)
        {
            var ptPregnancyItem = _patientInfoRepository.GetPregnancyList(ptId, hpId).FirstOrDefault();
            var ptCmtInfItem = _ptCmtInfRepository.GetList(ptId, hpId).FirstOrDefault();
            var seikaturekiInfItem = _patientInfoRepository.GetSeikaturekiInfList(ptId, hpId).FirstOrDefault();
            var listPhysicalItems = _patientInfoRepository.GetPhysicalList(hpId, ptId);

            return new PatientInfoModel(ptPregnancyItem ?? new PtPregnancyModel(), ptCmtInfItem ?? new PtCmtInfModel(), seikaturekiInfItem ?? new SeikaturekiInfModel(), listPhysicalItems ?? new List<PhysicalInfoModel>());
        }
        #endregion
    }
}
