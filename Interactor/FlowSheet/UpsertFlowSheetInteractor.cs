using Domain.Models.FlowSheet;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using UseCase.FlowSheet.Upsert;

namespace Interactor.FlowSheet
{
    public class UpsertFlowSheetInteractor : IUpsertFlowSheetInputPort
    {
        private readonly IFlowSheetRepository _flowsheetRepository;
        private readonly IPatientInforRepository _patientRepository;
        private readonly IReceptionRepository _receptionRepository;
        public UpsertFlowSheetInteractor(IFlowSheetRepository repository, IPatientInforRepository patientRepository, IReceptionRepository receptionRepository)
        {
            _flowsheetRepository = repository;
            _patientRepository = patientRepository;
            _receptionRepository = receptionRepository;
        }

        public UpsertFlowSheetOutputData Handle(UpsertFlowSheetInputData inputData)
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
            if (inputData.Items.Any(i => i.TagNo < 0 || i.TagNo > 7))
            {
                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.TagNoNoValid);
            }
            if (inputData.Items.Any(i => i.CmtKbn != 1 && i.CmtKbn != 9))
            {
                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.CmtKbnNoValid);
            }
            if (inputData.Items.Any(i => i.RainListCmtSeqNo < 0))
            {
                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.RainListCmtSeqNoNoValid);
            }
            if (inputData.Items.Any(i => i.RainListTagSeqNo < 0))
            {
                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.RainListTagSeqNoNoValid);
            }
            if (!_patientRepository.CheckListId(inputData.Items.Select(i => i.PtId).ToList()))
            {
                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.PtIdNoExist);
            }
            if (!_receptionRepository.CheckListNo(inputData.Items.Select(i => i.RainNo).ToList()))
            {
                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.RaiinNoExist);
            }

            try
            {
                var datas = inputData?.ToList()?.Select(i => new FlowSheetModel(
                        i.SinDate,
                        i.TagNo,
                        "",
                        i.RainNo,
                        0,
                        i.Text,
                        0,
                        true,
                        true,
                        true,
                        new List<RaiinListInfModel>(),
                        i.PtId,
                        i.CmtKbn,
                        0,
                        0
                    )).ToList() ?? new List<FlowSheetModel>();
                _flowsheetRepository.Upsert(datas);

                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.Success);
            }
            catch
            {
                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.UpdateNoSuccess);
            }
        }
    }
}
