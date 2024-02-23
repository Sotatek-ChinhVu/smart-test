﻿using Domain.Models.PtCmtInf;
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
        private readonly IImportantNoteRepository _importantNoteAlrgyElseRepository;
        private readonly IImportantNoteRepository _importantAlrgyFoodRepository;
        private readonly IImportantNoteRepository _importantAlrgyDrugRepository;
        private readonly IImportantNoteRepository _importantOtherDrugRepository;
        private readonly IImportantNoteRepository _importantOtcDrugRepository;
        private readonly IImportantNoteRepository _importantSuppleRepository;
        private readonly IImportantNoteRepository _importantKioRekiRepository;
        private readonly IImportantNoteRepository _importantInfectionRepository;
        private readonly IPatientInfoRepository _patientInfoPregnancyRepository;
        private readonly IPatientInfoRepository _patientInfoSeikaturekiRepository;
        private readonly IPatientInfoRepository _patientInfoPhysicalRepository;
        private readonly IPatientInfoRepository _patientInfoGetStdRepository;

        public GetSpecialNoteInteractor(IPtCmtInfRepository ptCmtInfRepository, ISummaryInfRepository summaryInfRepository, IImportantNoteRepository importantNoteAlrgyElseRepository, IImportantNoteRepository importantAlrgyFoodRepository, IImportantNoteRepository importantAlrgyDrugRepository, IImportantNoteRepository importantOtherDrugRepository, IImportantNoteRepository importantOtcDrugRepository, IImportantNoteRepository importantSuppleRepository, IImportantNoteRepository importantKioRekiRepository, IImportantNoteRepository importantInfectionRepository, IPatientInfoRepository patientInfoPregnancyRepository, IPatientInfoRepository patientInfoSeikaturekiRepository, IPatientInfoRepository patientInfoPhysicalRepository, IPatientInfoRepository patientInfoGetStdRepository)
        {
            _ptCmtInfRepository = ptCmtInfRepository;
            _summaryInfRepository = summaryInfRepository;
            _importantNoteAlrgyElseRepository = importantNoteAlrgyElseRepository;
            _patientInfoPregnancyRepository = patientInfoPregnancyRepository;
            _importantAlrgyFoodRepository = importantAlrgyFoodRepository;
            _importantAlrgyDrugRepository = importantAlrgyDrugRepository;
            _importantOtherDrugRepository = importantOtherDrugRepository;
            _importantOtcDrugRepository = importantOtcDrugRepository;
            _importantSuppleRepository = importantSuppleRepository;
            _importantKioRekiRepository = importantKioRekiRepository;
            _importantInfectionRepository = importantInfectionRepository;
            _patientInfoSeikaturekiRepository = patientInfoSeikaturekiRepository;
            _patientInfoPhysicalRepository = patientInfoPhysicalRepository;
            _patientInfoGetStdRepository = patientInfoGetStdRepository;
        }

        public GetSpecialNoteOutputData Handle(GetSpecialNoteInputData inputData)
        {
            try
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
                var taskImportantNoteTab = Task<ImportantNoteModel>.Factory.StartNew(() => GetImportantNoteTab(inputData.HpId, inputData.PtId));
                var taskPatientInfoTab = Task<PatientInfoModel>.Factory.StartNew(() => GetPatientInfoTab(inputData.PtId, inputData.HpId));
                Task.WaitAll(taskSummaryTab, taskImportantNoteTab, taskPatientInfoTab);

                return new GetSpecialNoteOutputData(taskSummaryTab.Result, taskImportantNoteTab.Result, taskPatientInfoTab.Result, GetSpecialNoteStatus.Successed);
            }
            finally
            {
                _importantAlrgyDrugRepository.ReleaseResource();
                _importantAlrgyFoodRepository.ReleaseResource();
                _importantInfectionRepository.ReleaseResource();
                _importantKioRekiRepository.ReleaseResource();
                _importantNoteAlrgyElseRepository.ReleaseResource();
                _importantOtcDrugRepository.ReleaseResource();
                _importantOtherDrugRepository.ReleaseResource();
                _importantSuppleRepository.ReleaseResource();
                _patientInfoPhysicalRepository.ReleaseResource();
                _patientInfoPregnancyRepository.ReleaseResource();
                _patientInfoSeikaturekiRepository.ReleaseResource();
                _ptCmtInfRepository.ReleaseResource();
                _summaryInfRepository.ReleaseResource();
                _patientInfoGetStdRepository.ReleaseResource();
            }
        }

        #region Get data for tab
        private SummaryInfModel GetSummaryTab(int hpId, long ptId)
        {
            return _summaryInfRepository.Get(hpId, ptId);
        }

        private ImportantNoteModel GetImportantNoteTab(int hpId, long ptId)
        {
            var taskAlrgyElseList = Task<List<PtAlrgyElseModel>>.Factory.StartNew(() => _importantNoteAlrgyElseRepository.GetAlrgyElseList(hpId, ptId));
            var taskAlrgyFoodList = Task<List<PtAlrgyFoodModel>>.Factory.StartNew(() => _importantAlrgyFoodRepository.GetAlrgyFoodList(hpId, ptId));
            var taskAlrgyDrugList = Task<List<PtAlrgyDrugModel>>.Factory.StartNew(() => _importantAlrgyDrugRepository.GetAlrgyDrugList(hpId, ptId));

            var taskOtherDrugList = Task<List<PtOtherDrugModel>>.Factory.StartNew(() => _importantOtherDrugRepository.GetOtherDrugList(hpId, ptId));
            var taskOctDrugList = Task<List<PtOtcDrugModel>>.Factory.StartNew(() => _importantOtcDrugRepository.GetOtcDrugList(hpId, ptId));
            var taskSuppleList = Task<List<PtSuppleModel>>.Factory.StartNew(() => _importantSuppleRepository.GetSuppleList(hpId, ptId));

            var taskKioRekiList = Task<List<PtKioRekiModel>>.Factory.StartNew(() => _importantKioRekiRepository.GetKioRekiList(hpId, ptId));
            var taskInfectionList = Task<List<PtInfectionModel>>.Factory.StartNew(() => _importantInfectionRepository.GetInfectionList(hpId, ptId));
            Task.WaitAll(taskAlrgyElseList, taskAlrgyFoodList, taskAlrgyDrugList, taskOtherDrugList, taskOctDrugList, taskSuppleList, taskKioRekiList, taskInfectionList);

            return new ImportantNoteModel(taskAlrgyFoodList.Result, taskAlrgyElseList.Result, taskAlrgyDrugList.Result, taskKioRekiList.Result, taskInfectionList.Result, taskOtherDrugList.Result, taskOctDrugList.Result, taskSuppleList.Result);
        }

        private PatientInfoModel GetPatientInfoTab(long ptId, int hpId)
        {
            var taskPregnancyItem = Task<List<PtPregnancyModel>>.Factory.StartNew(() => _patientInfoPregnancyRepository.GetPregnancyList(ptId, hpId) ?? new());
            var taskCmtInfItem = Task<PtCmtInfModel>.Factory.StartNew(() => _ptCmtInfRepository.GetList(ptId, hpId).FirstOrDefault() ?? new());
            var taskSeikaturekiInfItem = Task<SeikaturekiInfModel>.Factory.StartNew(() => _patientInfoSeikaturekiRepository.GetSeikaturekiInfList(ptId, hpId).FirstOrDefault() ?? new());
            var taskPhysicalItems = Task<List<PhysicalInfoModel>>.Factory.StartNew(() => _patientInfoPhysicalRepository.GetPhysicalList(hpId, ptId));
            Task.WaitAll(taskPregnancyItem, taskCmtInfItem, taskSeikaturekiInfItem, taskPhysicalItems);

            return new PatientInfoModel(taskPregnancyItem.Result, taskCmtInfItem.Result, taskSeikaturekiInfItem.Result, taskPhysicalItems.Result);
        }
        #endregion
    }
}
