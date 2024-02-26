﻿using Domain.Models.AuditLog;
using Domain.Models.Diseases;
using Domain.Models.Insurance;
using Domain.Models.PatientInfor;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.Diseases.Upsert;
using static Helper.Constants.PtDiseaseConst;
namespace Interactor.Diseases
{
    public class UpsertPtDiseaseListInteractor : IUpsertPtDiseaseListInputPort
    {
        private readonly IPtDiseaseRepository _diseaseRepository;
        private readonly IPatientInforRepository _patientInforRepository;
        private readonly IInsuranceRepository _insuranceInforRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly ILoggingHandler _loggingHandler;

        public UpsertPtDiseaseListInteractor(ITenantProvider tenantProvider, IPtDiseaseRepository diseaseRepository, IPatientInforRepository patientInforRepository, IInsuranceRepository insuranceInforRepository, IAuditLogRepository auditLogRepository)
        {
            _diseaseRepository = diseaseRepository;
            _patientInforRepository = patientInforRepository;
            _insuranceInforRepository = insuranceInforRepository;
            _auditLogRepository = auditLogRepository;
            _loggingHandler = new LoggingHandler(tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public UpsertPtDiseaseListOutputData Handle(UpsertPtDiseaseListInputData inputData)
        {
            try
            {
                if (inputData.ToList() == null || inputData.ToList().Count == 0)
                {
                    return new UpsertPtDiseaseListOutputData(UpsertPtDiseaseListStatus.PtDiseaseListInputNoData, new());
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
                        return new UpsertPtDiseaseListOutputData(ConvertStatus(status), new());
                    }
                }

                if (!_patientInforRepository.CheckExistIdList(inputData.HpId, datas.Select(i => i.PtId).Distinct().ToList()))
                {
                    return new UpsertPtDiseaseListOutputData(UpsertPtDiseaseListStatus.PtDiseaseListPtIdNoExist, new());
                }

                var result = _diseaseRepository.Upsert(datas, inputData.HpId, inputData.UserId);

                AddAuditLog(inputData.HpId, inputData.UserId, inputData.PtDiseaseModel.FirstOrDefault()?.PtId ?? 0);

                return new UpsertPtDiseaseListOutputData(UpsertPtDiseaseListStatus.Success, result);
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _diseaseRepository.ReleaseResource();
                _insuranceInforRepository.ReleaseResource();
                _patientInforRepository.ReleaseResource();
                _auditLogRepository.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }

        private static UpsertPtDiseaseListStatus ConvertStatus(ValidationStatus status)
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

        private void AddAuditLog(int hpId, int userId, long ptId)
        {
            var arg = new ArgumentModel(
                            EventCode.DiseaseRegUpdate,
                            ptId,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            string.Empty
                );

            _auditLogRepository.AddAuditTrailLog(hpId, userId, arg);
        }
    }
}
