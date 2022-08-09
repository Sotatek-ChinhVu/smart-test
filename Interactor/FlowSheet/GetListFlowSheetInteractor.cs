using Domain.Constant;
using Domain.Models.FlowSheet;
using Domain.Models.RaiinListMst;
using Helper.Common;
using Helper.Constants;
using Helper.Extendsions;
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
            var holidayMstList = _flowsheetRepository.GetHolidayMst(inputData.HpId, inputData.HolidayFrom, inputData.HolidayTo);

            if (inputData.IsHolidayOnly)
            {
                return new GetListFlowSheetOutputData(new List<FlowSheetModel>(), new List<RaiinListMstModel>(), holidayMstList);
            }

            var flowsheetList = _flowsheetRepository.GetListFlowSheet(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo);
            var raiinListMst = _flowsheetRepository.GetRaiinListMsts(inputData.HpId);
            
            return new GetListFlowSheetOutputData(flowsheetList, raiinListMst, holidayMstList);
        }
    }
}
