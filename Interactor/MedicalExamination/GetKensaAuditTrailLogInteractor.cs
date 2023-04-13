using Domain.Models.MedicalExamination;
using UseCase.MedicalExamination.GetKensaAuditTrailLog;

namespace Interactor.MedicalExamination
{
    public class GetKensaAuditTrailLogInteractor : IGetKensaAuditTrailLogInputPort
    {
        private readonly IMedicalExaminationRepository _medicalExaminationRepository;

        public GetKensaAuditTrailLogInteractor(IMedicalExaminationRepository medicalExaminationRepository)
        {
            _medicalExaminationRepository = medicalExaminationRepository;
        }

        public GetKensaAuditTrailLogOutputData Handle(GetKensaAuditTrailLogInputData inputData)
        {
            try
            {
                if (inputData.PtId <= 0)
                {
                    return new GetKensaAuditTrailLogOutputData(GetKensaAuditTrailLogStatus.InvalidPtId, new());
                }
                if (inputData.SinDate <= 0)
                {
                    return new GetKensaAuditTrailLogOutputData(GetKensaAuditTrailLogStatus.InvalidSinDate, new());
                }
                if (inputData.RaiinNo <= 0)
                {
                    return new GetKensaAuditTrailLogOutputData(GetKensaAuditTrailLogStatus.InvalidRaiinNo, new());
                }

                var auditTrailLogs = _medicalExaminationRepository.GetKensaAuditTrailLogs(inputData.EventCd, inputData.PtId, inputData.SinDate, inputData.RaiinNo);
                var result = auditTrailLogs.Select(a => new AuditTrailLogItem(
                        a.LogId,
                        a.LogDate,
                        a.HpId,
                        a.UserId,
                        a.EventCd,
                        a.PtId,
                        a.SinDate,
                        a.RaiinNo,
                        a.Machine,
                        a.Hosuke
                    )).ToList();

                return new GetKensaAuditTrailLogOutputData(GetKensaAuditTrailLogStatus.Successed, result);
            }
            finally
            {
                _medicalExaminationRepository.ReleaseResource();
            }
        }
    }
}
