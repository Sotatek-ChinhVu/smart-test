using Domain.Models.FlowSheet;
using UseCase.FlowSheet.Upsert;

namespace Interactor.FlowSheet
{
    public class UpsertFlowSheetInteractor : IUpsertFlowSheetInputPort
    {
        private readonly IFlowSheetRepository _flowsheetRepository;
        public UpsertFlowSheetInteractor(IFlowSheetRepository repository)
        {
            _flowsheetRepository = repository;
        }

        public UpsertFlowSheetOutputData Handle(UpsertFlowSheetInputData inputData)
        {
            if (inputData.RainNo <= 0)
            {
                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.InvalidRaiinNo);
            }
            if (inputData.PtId <= 0)
            {
                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.InvalidPtId);
            }
            if (inputData.SinDate <= 0)
            {
                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.InvalidSinDate);
            }
            if (inputData.TagNo <= 0)
            {
                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.InvalidTagNo);
            }
            if (inputData.CmtKbn <= 0)
            {
                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.InvalidCmtKbn);
            }
            try
            {
                _flowsheetRepository.Upsert(inputData.RainNo, inputData.PtId, inputData.SinDate, inputData.TagNo, inputData.CmtKbn, inputData.Text, inputData.SeqNo);

                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.Success);
            }
            catch
            {
                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.UpdateNoSuccess);
            }
        }
    }
}
