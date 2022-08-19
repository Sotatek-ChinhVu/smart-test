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
            try
            {
                var datas = inputData.ToList().Select(i => new FlowSheetModel(
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
                    )).ToList();
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
