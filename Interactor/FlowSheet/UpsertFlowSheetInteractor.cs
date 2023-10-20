using Domain.Models.FlowSheet;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.FlowSheet.Upsert;

namespace Interactor.FlowSheet
{
    public class UpsertFlowSheetInteractor : IUpsertFlowSheetInputPort
    {
        private readonly IFlowSheetRepository _flowsheetRepository;
        private readonly IPatientInforRepository _patientRepository;
        private readonly IReceptionRepository _receptionRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public UpsertFlowSheetInteractor(ITenantProvider tenantProvider, IFlowSheetRepository repository, IPatientInforRepository patientRepository, IReceptionRepository receptionRepository)
        {
            _flowsheetRepository = repository;
            _patientRepository = patientRepository;
            _receptionRepository = receptionRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public UpsertFlowSheetOutputData Handle(UpsertFlowSheetInputData inputData)
        {
            try
            {
                if (inputData.ToList().Count == 0)
                {
                    return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.InputDataNoValid);
                }
                if (inputData.Items.Any(i => i.RainNo <= 0))
                {
                    return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.RainNoNoValid);
                }
                if (inputData.Items.Any(i => i.PtId <= 0))
                {
                    return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.PtIdNoValid);
                }
                if (inputData.Items.Any(i => i.SinDate < 19000000 || i.SinDate > 30000000))
                {
                    return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.SinDateNoValid);
                }

                var checkTagNo = inputData.Items.Any(i => i.TagNo < -1 || i.TagNo > 7);
                if (checkTagNo)
                {
                    return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.TagNoNoValid);
                }

                if (!_patientRepository.CheckExistIdList(inputData.Items.Select(i => i.PtId).Distinct().ToList()))
                {
                    return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.PtIdNoExist);
                }
                if (!_receptionRepository.CheckListNo(inputData.Items.Select(i => i.RainNo).ToList()))
                {
                    return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.RaiinNoExist);
                }

                var flowSheetDatas = inputData?.ToList()?.Select(i => new FlowSheetModel(
                        i.SinDate,
                        i.TagNo,
                        "",
                        i.RainNo,
                        0,
                        i.Cmt,
                        0,
                        true,
                        true,
                        new List<RaiinListInfModel>(),
                        i.PtId,
                        false
                    )).ToList() ?? new List<FlowSheetModel>();
                _flowsheetRepository.UpsertTag(flowSheetDatas, inputData?.HpId ?? 1, inputData?.UserId ?? 0);

                _flowsheetRepository.UpsertCmt(flowSheetDatas, inputData?.HpId ?? 1, inputData?.UserId ?? 0);

                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.Success);
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _flowsheetRepository.ReleaseResource();
                _patientRepository.ReleaseResource();
                _receptionRepository.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }
    }
}
