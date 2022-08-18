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

        public UpsertFlowSheetOutputData Handle(UpsertFlowSheetInputData inputDatas)
        {
            try
            {
                _flowsheetRepository.Upsert(inputDatas.ToList().Select(i => new FlowSheetModel(
                    i.PtId,
                    i.SinDate,
                    i.TagNo,
                    "",
                    i.RainNo,
                    0,
                    i.Text,
                    0,
                    false,
                    false,
                    false,
                    i.TagSeqNo,
                    i.CmtSeqNo,
                    i.CmtKbn,
                    new List<RaiinListInfModel>()
                 )).ToList());

                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.Success);
            }
            catch
            {
                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.UpdateNoSuccess);
            }
        }
    }
}
