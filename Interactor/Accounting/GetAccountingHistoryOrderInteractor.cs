using Domain.Models.HistoryOrder;
using UseCase.Accounting.GetHistoryOrder;

namespace Interactor.Accounting
{
    public class GetAccountingHistoryOrderInteractor : IGetAccountingHistoryOrderInputPort
    {
        private readonly IHistoryOrderRepository _historyOrderRepository;

        public GetAccountingHistoryOrderInteractor(IHistoryOrderRepository historyOrderRepository)
        {
            _historyOrderRepository = historyOrderRepository;
        }

        public GetAccountingHistoryOrderOutputData Handle(GetAccountingHistoryOrderInputData inputData)
        {
            try
            {
                (int, List<HistoryOrderModel>) historyList = _historyOrderRepository.GetList(
                   inputData.HpId,
                   inputData.UserId,
                   inputData.PtId,
                   inputData.SinDate,
                   inputData.Offset,
                   inputData.Limit,
                   0,
                   inputData.DeleteConditon,
                   inputData.RaiinNo
                   );
                return new GetAccountingHistoryOrderOutputData(historyList.Item1, ConvertToHistoryOrderDtoModel(historyList.Item2), GetAccountingHistoryOrderStatus.Successed);
            }
            finally
            {
                _historyOrderRepository.ReleaseResource();
            }
        }

        private List<HistoryOrderDto> ConvertToHistoryOrderDtoModel(List<HistoryOrderModel> models)
        {
            List<HistoryOrderDto> historyOrderDtoModelList = new List<HistoryOrderDto>();
            foreach (var item in models)
            {
                historyOrderDtoModelList.Add(new HistoryOrderDto(item.RaiinNo, item.SinDate, item.HokenPid, item.HokenTitle, item.HokenRate,
                    item.HokenType, item.SyosaisinKbn, item.JikanKbn, item.KaId, item.TantoId, item.KaName, item.TantoName, item.SanteiKbn,
                    item.TagNo, item.SinryoTitle, item.OrderInfList, item.KarteInfModels));
            }

            return historyOrderDtoModelList;
        }
    }
}