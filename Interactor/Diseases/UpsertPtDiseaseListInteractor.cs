using Domain.Models.Diseases;
using Domain.Models.Insurance;
using Domain.Models.PatientInfor;
using UseCase.Diseases.Upsert;
using static Helper.Constants.PtDiseaseConst;

namespace Interactor.Diseases
{
    public class UpsertPtDiseaseListInteractor : IUpsertPtDiseaseListInputPort
    {
        private readonly IPtDiseaseRepository _diseaseRepository;
        private readonly IPatientInforRepository _patientInforRepository;
        private readonly IInsuranceRepository _insuranceInforRepository;
        public UpsertPtDiseaseListInteractor(IPtDiseaseRepository diseaseRepository, IPatientInforRepository patientInforRepository, IInsuranceRepository insuranceInforRepository)
        {
            _diseaseRepository = diseaseRepository;
            _patientInforRepository = patientInforRepository;
            _insuranceInforRepository = insuranceInforRepository;
        }
        public UpsertPtDiseaseListOutputData Handle(UpsertPtDiseaseListInputData inputData)
        {
            try
            {
                if (inputData.ToList() == null)
                {
                    return new UpsertPtDiseaseListOutputData(UpsertPtDiseaseListStatus.PtDiseaseListInputNoData);
                }

                var datas = inputData.ptDiseaseModel.Select(i => new PtDiseaseModel(
                        0,
                        i.PtId,
                        i.SeqNo,
                        i.ByomeiCd,
                        i.SortNo,
                        i.PrefixList,
                        i.SuffixList,
                        i.Byomei,
                        i.StartDate,
                        i.TenkiKbn,
                        i.TenkiDate,
                        i.SyubyoKbn,
                        i.SikkanKbn,
                        i.NanByoCd,
                        i.IsNodspRece,
                        i.IsNodspKarte,
                        i.IsDeleted,
                        i.Id,
                        i.IsImportant,
                        0,
                        "",
                        "",
                        "",
                        "",
                        i.HokenPid,
                        i.HosokuCmt
                    )).ToList();

                foreach (var data in datas)
                {
                    var status = data.Validation();
                    if (status != ValidationStatus.Valid)
                    {
                        return new UpsertPtDiseaseListOutputData(ConvertStatus(status));
                    }
                }

                if (!_patientInforRepository.CheckListId(datas.Select(i => i.PtId).ToList()))
                {
                    return new UpsertPtDiseaseListOutputData(UpsertPtDiseaseListStatus.PtDiseaseListPtIdNoExist);
                }
                if (!_insuranceInforRepository.CheckHokenPIdList(datas.Where(i => i.HokenPid > 0).Select(i => i.HokenPid).ToList()))
                {
                    return new UpsertPtDiseaseListOutputData(UpsertPtDiseaseListStatus.PtDiseaseListHokenPIdNoExist);
                }
                if (inputData.ToList().Count == 0) return new UpsertPtDiseaseListOutputData(UpsertPtDiseaseListStatus.PtDiseaseListInputNoData);

                _diseaseRepository.Upsert(datas);
                return new UpsertPtDiseaseListOutputData(UpsertPtDiseaseListStatus.Success);
            }
            catch
            {
                return new UpsertPtDiseaseListOutputData(UpsertPtDiseaseListStatus.PtDiseaseListUpdateNoSuccess);
            }

        }

        private UpsertPtDiseaseListStatus ConvertStatus(ValidationStatus status)
        {
            if (status == ValidationStatus.InvalidTenkiKbn)
                return UpsertPtDiseaseListStatus.PtDiseaseListInvalidTenkiKbn;
            if (status == ValidationStatus.InvalidSikkanKbn)
                return UpsertPtDiseaseListStatus.PtDiseaseListInvalidSikkanKbn;
            if (status == ValidationStatus.InvalidNanByoCd)
                return UpsertPtDiseaseListStatus.PtDiseaseListInvalidNanByoCd;
            if (status == ValidationStatus.InvalidFreeWord)
                return UpsertPtDiseaseListStatus.PtDiseaseListInvalidFreeWord;
            if (status == ValidationStatus.InvalidTenkiDateContinue)
                return UpsertPtDiseaseListStatus.PtDiseaseListInvalidTenkiDateContinue;
            if (status == ValidationStatus.InvalidTekiDateAndStartDate)
                return UpsertPtDiseaseListStatus.PtDiseaseListInvalidTenkiDateContinue;
            return UpsertPtDiseaseListStatus.Success;
        }
    }
}
