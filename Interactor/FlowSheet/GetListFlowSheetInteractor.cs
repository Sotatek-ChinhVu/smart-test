using Domain.Models.FlowSheet;
using UseCase.FlowSheet.GetList;

namespace Interactor.FlowSheet;

public class GetListFlowSheetInteractor : IGetListFlowSheetInputPort
{
    private readonly IFlowSheetRepository _flowsheetRepository;
    private const string startGroupOrderKey = "group_";

    public GetListFlowSheetInteractor(IFlowSheetRepository repository)
    {
        _flowsheetRepository = repository;
    }

    public GetListFlowSheetOutputData Handle(GetListFlowSheetInputData inputData)
    {
        bool sortGroup = inputData.SortData.Select(item => item.Key).ToList().Exists(item => item.StartsWith(startGroupOrderKey));
        try
        {
            if (inputData.IsHolidayOnly)
            {
                var holidayMstList = _flowsheetRepository.GetHolidayMst(inputData.HpId, inputData.HolidayFrom, inputData.HolidayTo);
                return new GetListFlowSheetOutputData(holidayMstList.Select(h => new HolidayDto(h.SeqNo, h.SinDate, h.HolidayKbn, h.KyusinKbn, h.HolidayName)).ToList());
            }
            else
            {
                List<FlowSheetModel> flowsheetList = new();
                long totalFlowSheet = 0;
                if (sortGroup)
                {
                    flowsheetList = _flowsheetRepository.GetListFlowSheet(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo, ref totalFlowSheet, inputData.PageIndex, inputData.PageSize, inputData.SortData);
                }
                else
                {
                    flowsheetList = _flowsheetRepository.GetListFlowSheet(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo, ref totalFlowSheet, inputData.PageIndex, inputData.PageSize, inputData.SortData);
                }
                var raiinListMst = _flowsheetRepository.GetRaiinListMsts(inputData.HpId);
                var raiinListInfList = _flowsheetRepository.GetRaiinListInf(inputData.HpId, inputData.PtId);
                var raiinListInfForNextOrderList = _flowsheetRepository.GetRaiinListInfForNextOrder(inputData.HpId, inputData.PtId);

                return new GetListFlowSheetOutputData(flowsheetList, raiinListMst, raiinListInfList, raiinListInfForNextOrderList, totalFlowSheet);
            }
        }
        finally
        {
            _flowsheetRepository.ReleaseResource();
        }
    }
}
