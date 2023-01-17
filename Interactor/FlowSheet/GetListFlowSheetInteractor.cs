using Domain.Models.FlowSheet;
using Domain.Models.RaiinListMst;
using UseCase.FlowSheet.GetList;

namespace Interactor.FlowSheet
{
    public class GetListFlowSheetInteractor : IGetListFlowSheetInputPort
    {
        private readonly IFlowSheetRepository _flowsheetRepository;
        public GetListFlowSheetInteractor(IFlowSheetRepository repository)
        {
            _flowsheetRepository = repository;
        }

        public GetListFlowSheetOutputData Handle(GetListFlowSheetInputData inputData)
        {
            try
            {
                if (inputData.IsHolidayOnly)
                {
                    var holidayMstList = _flowsheetRepository.GetHolidayMst(inputData.HpId, inputData.HolidayFrom, inputData.HolidayTo);
                    return new GetListFlowSheetOutputData(holidayMstList);
                }
                else
                {
                    long totalFlowSheet = 0;
                    int count = inputData.Count <= 0 ? 50 : inputData.Count;
                    var flowsheetList = _flowsheetRepository.GetListFlowSheet(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo, inputData.StartIndex, count, inputData.Sort, ref totalFlowSheet);
                    var raiinListMst = _flowsheetRepository.GetRaiinListMsts(inputData.HpId);
                    var raiinListInfList = _flowsheetRepository.GetRaiinListInf(inputData.HpId, inputData.PtId);

                    return new GetListFlowSheetOutputData(flowsheetList, raiinListMst, raiinListInfList, totalFlowSheet);
                }
            }
            finally
            {
                _flowsheetRepository.ReleaseResource();
            }
        }
    }
}
