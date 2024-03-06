using Domain.Models.Diseases;
using Domain.Models.Insurance;
using Domain.Models.PatientInfor;
using UseCase.Diseases.Validation;
using static Helper.Constants.PtDiseaseConst;
namespace Interactor.Diseases
{
    public class ValidationPtDiseaseListInteractor : IValidationPtDiseaseListInputPort
    {
        private readonly IPtDiseaseRepository _diseaseRepository;
        private readonly IPatientInforRepository _patientInforRepository;
        private readonly IInsuranceRepository _insuranceInforRepository;
        public ValidationPtDiseaseListInteractor(IPtDiseaseRepository diseaseRepository, IPatientInforRepository patientInforRepository, IInsuranceRepository insuranceInforRepository)
        {
            _diseaseRepository = diseaseRepository;
            _patientInforRepository = patientInforRepository;
            _insuranceInforRepository = insuranceInforRepository;
        }
        public ValidationPtDiseaseListOutputData Handle(ValidationPtDiseaseListInputData inputData)
        {
            try
            {
                if (inputData.ToList() == null || inputData.ToList().Count == 0)
                {
                    return new ValidationPtDiseaseListOutputData(ValidationPtDiseaseListStatus.PtDiseaseListInputNoData);
                }

                var datas = inputData.PtDiseaseModel.Select(i => new PtDiseaseModel(
                        i.HpId,
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
                        return new ValidationPtDiseaseListOutputData(ConvertStatus(status));
                    }
                }

                if (!_patientInforRepository.CheckExistIdList(inputData.HpId, datas.Select(i => i.PtId).Distinct().ToList()))
                {
                    return new ValidationPtDiseaseListOutputData(ValidationPtDiseaseListStatus.PtDiseaseListPtIdNoExist);
                }
                if (!_insuranceInforRepository.CheckExistHokenPIdList(datas.Where(i => i.HokenPid > 0).Select(i => i.HokenPid).Distinct().ToList(), datas.Select(i => i.HpId).Distinct().ToList(), datas.Select(i => i.PtId).Distinct().ToList()))
                {
                    return new ValidationPtDiseaseListOutputData(ValidationPtDiseaseListStatus.PtDiseaseListHokenPIdNoExist);
                }

                return new ValidationPtDiseaseListOutputData(ValidationPtDiseaseListStatus.Valid);
            }
            finally
            {
                _diseaseRepository.ReleaseResource();
                _insuranceInforRepository.ReleaseResource();
                _patientInforRepository.ReleaseResource();
            }
        }

        private static ValidationPtDiseaseListStatus ConvertStatus(ValidationStatus status)
        {
            if (status == ValidationStatus.InvalidTenkiKbn)
                return ValidationPtDiseaseListStatus.PtDiseaseListInvalidTenkiKbn;
            if (status == ValidationStatus.InvalidSikkanKbn)
                return ValidationPtDiseaseListStatus.PtDiseaseListInvalidSikkanKbn;
            if (status == ValidationStatus.InvalidNanByoCd)
                return ValidationPtDiseaseListStatus.PtDiseaseListInvalidNanByoCd;
            if (status == ValidationStatus.InvalidFreeWord)
                return ValidationPtDiseaseListStatus.PtDiseaseListInvalidFreeWord;
            if (status == ValidationStatus.InvalidTenkiDateContinue)
                return ValidationPtDiseaseListStatus.PtDiseaseListInvalidTenkiDateContinue;
            if (status == ValidationStatus.InvalidTenkiDateCommon)
                return ValidationPtDiseaseListStatus.PtDiseaseListInvalidTenkiDateCommon;
            if (status == ValidationStatus.InvalidTekiDateAndStartDate)
                return ValidationPtDiseaseListStatus.PtDiseaseListInvalidTekiDateAndStartDate;
            if (status == ValidationStatus.InvalidByomei)
                return ValidationPtDiseaseListStatus.PtDiseaseListInvalidByomei;
            if (status == ValidationStatus.InvalidId)
                return ValidationPtDiseaseListStatus.PtInvalidId;
            if (status == ValidationStatus.InvalidHpId)
                return ValidationPtDiseaseListStatus.PtInvalidHpId;
            if (status == ValidationStatus.InvalidPtId)
                return ValidationPtDiseaseListStatus.PtInvalidPtId;
            if (status == ValidationStatus.InvalidSortNo)
                return ValidationPtDiseaseListStatus.PtInvalidSortNo;
            if (status == ValidationStatus.InvalidByomeiCd)
                return ValidationPtDiseaseListStatus.PtInvalidByomeiCd;
            if (status == ValidationStatus.InvalidStartDate)
                return ValidationPtDiseaseListStatus.PtInvalidStartDate;
            if (status == ValidationStatus.InvalidTenkiDate)
                return ValidationPtDiseaseListStatus.PtInvalidTenkiDate;
            if (status == ValidationStatus.InvalidSyubyoKbn)
                return ValidationPtDiseaseListStatus.PtInvalidSyubyoKbn;
            if (status == ValidationStatus.InvalidHosokuCmt)
                return ValidationPtDiseaseListStatus.PtInvalidHosokuCmt;
            if (status == ValidationStatus.InvalidHokenPid)
                return ValidationPtDiseaseListStatus.PtInvalidHokenPid;
            if (status == ValidationStatus.InvalidIsNodspRece)
                return ValidationPtDiseaseListStatus.PtInvalidIsNodspRece;
            if (status == ValidationStatus.InvalidIsNodspKarte)
                return ValidationPtDiseaseListStatus.PtInvalidIsNodspKarte;
            if (status == ValidationStatus.InvalidSeqNo)
                return ValidationPtDiseaseListStatus.PtInvalidSeqNo;
            if (status == ValidationStatus.InvalidIsImportant)
                return ValidationPtDiseaseListStatus.PtInvalidIsImportant;
            if (status == ValidationStatus.InvalidIsDeleted)
                return ValidationPtDiseaseListStatus.PtInvalidIsDeleted;

            return ValidationPtDiseaseListStatus.Valid;
        }
    }
}
