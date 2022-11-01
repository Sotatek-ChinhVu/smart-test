using Domain.Models.PtCmtInf;
using Domain.Models.SpecialNote.ImportantNote;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.SpecialNote.SummaryInf;
using Infrastructure.Interfaces;
using Infrastructure.Repositories.SpecialNote;
using System.Collections.Generic;
using System.Threading.Tasks;
using UseCase.SpecialNote.Get;

namespace Interactor.SpecialNote
{
    public class GetSpecialNoteInteractor : IGetSpecialNoteInputPort
    {
        private readonly IPtCmtInfRepository _ptCmtInfRepository;
        private readonly ISummaryInfRepository _summaryInfRepository;
        private readonly IImportantNoteRepository _importantNoteRepository;
        private readonly IPatientInfoRepository _patientInfoRepository;

        public GetSpecialNoteInteractor(IPtCmtInfRepository ptCmtInfRepository, ISummaryInfRepository summaryInfRepository, ITenantProvider tenant)
        {
            _ptCmtInfRepository = ptCmtInfRepository;
            _summaryInfRepository = summaryInfRepository;
            _importantNoteRepository = new ImportantNoteRepository(tenant);
            _patientInfoRepository = new PatientInfoRepository(tenant);
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

            var taskSummaryTab = Task<SummaryInfModel>.Factory.StartNew(() => GetSummaryTab(inputData.HpId, inputData.PtId));
            var taskImportantNoteTab = Task<ImportantNoteModel>.Factory.StartNew(() => GetImportantNoteTab(inputData.PtId));
            var taskPatientInfoTab = Task<PatientInfoModel>.Factory.StartNew(() => GetPatientInfoTab(inputData.PtId, inputData.HpId));
            Task.WaitAll(taskSummaryTab, taskImportantNoteTab, taskPatientInfoTab);

            return new GetSpecialNoteOutputData(taskSummaryTab.Result, taskImportantNoteTab.Result, taskPatientInfoTab.Result, GetSpecialNoteStatus.Successed);
        }

        #region Get data for tab
        private SummaryInfModel GetSummaryTab(int hpId, long ptId)
        {
            return _summaryInfRepository.GetList(hpId, ptId);
        }

        private ImportantNoteModel GetImportantNoteTab(long ptId)
        {
            var taskAlrgyElseList = Task<List<PtAlrgyElseModel>>.Factory.StartNew(() => _importantNoteRepository.GetAlrgyElseList(ptId));
            var taskAlrgyFoodList = Task<List<PtAlrgyFoodModel>>.Factory.StartNew(() => _importantNoteRepository.GetAlrgyFoodList(ptId));
            var taskAlrgyDrugList = Task<List<PtAlrgyDrugModel>>.Factory.StartNew(() => _importantNoteRepository.GetAlrgyDrugList(ptId));

            var taskOtherDrugList = Task<List<PtOtherDrugModel>>.Factory.StartNew(() => _importantNoteRepository.GetOtherDrugList(ptId));
            var taskOctDrugList = Task<List<PtOtcDrugModel>>.Factory.StartNew(() => _importantNoteRepository.GetOtcDrugList(ptId));
            var taskSuppleList = Task<List<PtSuppleModel>>.Factory.StartNew(() => _importantNoteRepository.GetSuppleList(ptId));

            var taskKioRekiList = Task<List<PtKioRekiModel>>.Factory.StartNew(() => _importantNoteRepository.GetKioRekiList(ptId));
            var taskInfectionList = Task<List<PtInfectionModel>>.Factory.StartNew(() => _importantNoteRepository.GetInfectionList(ptId));
            Task.WaitAll(taskAlrgyElseList, taskAlrgyFoodList, taskAlrgyDrugList, taskOtherDrugList, taskOctDrugList, taskSuppleList, taskKioRekiList, taskInfectionList);

            return new ImportantNoteModel(taskAlrgyFoodList.Result, taskAlrgyElseList.Result, taskAlrgyDrugList.Result, taskKioRekiList.Result, taskInfectionList.Result, taskOtherDrugList.Result, taskOctDrugList.Result, taskSuppleList.Result);
        }

        private PatientInfoModel GetPatientInfoTab(long ptId, int hpId)
        {
            var taskPregnancyItem = Task<PtPregnancyModel>.Factory.StartNew(() => _patientInfoRepository.GetPregnancyList(ptId, hpId).FirstOrDefault() ?? new());
            var taskCmtInfItem = Task<PtCmtInfModel>.Factory.StartNew(() => _ptCmtInfRepository.GetList(ptId, hpId).FirstOrDefault() ?? new());
            var taskSeikaturekiInfItem = Task<SeikaturekiInfModel>.Factory.StartNew(() => _patientInfoRepository.GetSeikaturekiInfList(ptId, hpId).FirstOrDefault() ?? new());
            var taskPhysicalItems = Task<List<PhysicalInfoModel>>.Factory.StartNew(() => _patientInfoRepository.GetPhysicalList(hpId, ptId));
            Task.WaitAll(taskPregnancyItem, taskCmtInfItem, taskSeikaturekiInfItem, taskPhysicalItems);

            return new PatientInfoModel(taskPregnancyItem.Result, taskCmtInfItem.Result, taskSeikaturekiInfItem.Result, taskPhysicalItems.Result);
        }
        #endregion
    }
}
