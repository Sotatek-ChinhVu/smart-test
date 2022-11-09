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
                if (inputData.ToList() == null || inputData.ToList().Count == 0)
                {
                    return new UpsertPtDiseaseListOutputData(UpsertPtDiseaseListStatus.PtDiseaseListInputNoData);
                }

                var datas = inputData.ptDiseaseModel.Select(i => new PtDiseaseModel(
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
                        return new UpsertPtDiseaseListOutputData(ConvertStatus(status));
                    }
                }

                if (!_patientInforRepository.CheckListId(datas.Select(i => i.PtId).Distinct().ToList()))
                {
                    return new UpsertPtDiseaseListOutputData(UpsertPtDiseaseListStatus.PtDiseaseListPtIdNoExist);
                }
                if (!_insuranceInforRepository.CheckHokenPIdList(datas.Where(i => i.HokenPid > 0).Select(i => i.HokenPid).Distinct().ToList(), datas.Select(i => i.HpId).Distinct().ToList(), datas.Select(i => i.PtId).Distinct().ToList()))
                {
                    return new UpsertPtDiseaseListOutputData(UpsertPtDiseaseListStatus.PtDiseaseListHokenPIdNoExist);
                }

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
            if (status == ValidationStatus.InvalidTenkiDateCommon)
                return UpsertPtDiseaseListStatus.PtDiseaseListInvalidTenkiDateCommon;
            if (status == ValidationStatus.InvalidTekiDateAndStartDate)
                return UpsertPtDiseaseListStatus.PtDiseaseListInvalidTekiDateAndStartDate;
            if (status == ValidationStatus.InvalidByomei)
                return UpsertPtDiseaseListStatus.PtDiseaseListInvalidByomei;
            if (status == ValidationStatus.InvalidId)
                return UpsertPtDiseaseListStatus.PtInvalidId;
            if (status == ValidationStatus.InvalidHpId)
                return UpsertPtDiseaseListStatus.PtInvalidHpId;
            if (status == ValidationStatus.InvalidPtId)
                return UpsertPtDiseaseListStatus.PtInvalidPtId;
            if (status == ValidationStatus.InvalidSortNo)
                return UpsertPtDiseaseListStatus.PtInvalidSortNo;
            if (status == ValidationStatus.InvalidByomeiCd)
                return UpsertPtDiseaseListStatus.PtInvalidByomeiCd;
            if (status == ValidationStatus.InvalidStartDate)
                return UpsertPtDiseaseListStatus.PtInvalidStartDate;
            if (status == ValidationStatus.InvalidTenkiDate)
                return UpsertPtDiseaseListStatus.PtInvalidTenkiDate;
            if (status == ValidationStatus.InvalidSyubyoKbn)
                return UpsertPtDiseaseListStatus.PtInvalidSyubyoKbn;
            if (status == ValidationStatus.InvalidHosokuCmt)
                return UpsertPtDiseaseListStatus.PtInvalidHosokuCmt;
            if (status == ValidationStatus.InvalidHokenPid)
                return UpsertPtDiseaseListStatus.PtInvalidHokenPid;
            if (status == ValidationStatus.InvalidIsNodspRece)
                return UpsertPtDiseaseListStatus.PtInvalidIsNodspRece;
            if (status == ValidationStatus.InvalidIsNodspKarte)
                return UpsertPtDiseaseListStatus.PtInvalidIsNodspKarte;
            if (status == ValidationStatus.InvalidSeqNo)
                return UpsertPtDiseaseListStatus.PtInvalidSeqNo;
            if (status == ValidationStatus.InvalidIsImportant)
                return UpsertPtDiseaseListStatus.PtInvalidIsImportant;
            if (status == ValidationStatus.InvalidIsDeleted)
                return UpsertPtDiseaseListStatus.PtInvalidIsDeleted;

            return UpsertPtDiseaseListStatus.Success;
        }
    }
}
