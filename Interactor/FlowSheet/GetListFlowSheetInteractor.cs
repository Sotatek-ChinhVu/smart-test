using Domain.Models.FlowSheet;
using Domain.Models.RaiinListMst;
using System.Diagnostics;
using UseCase.FlowSheet.GetList;

namespace Interactor.FlowSheet
{
    public class GetListFlowSheetInteractor : IGetListFlowSheetInputPort
    {
        private readonly IFlowSheetRepository _flowsheetRepository;
        private readonly IFlowSheetRepository _flowsheetRaiinListMstRepository;
        private readonly IFlowSheetRepository _flowsheetRaiinListInfRepository;
        private readonly IFlowSheetRepository _flowsheetRaiinListNextOrderRepository;
        public GetListFlowSheetInteractor(IFlowSheetRepository repository, IFlowSheetRepository flowsheetRaiinListMstRepository, IFlowSheetRepository flowsheetRaiinListInfRepository, IFlowSheetRepository flowsheetRaiinListNextOrderRepository)
        {
            _flowsheetRepository = repository;
            _flowsheetRaiinListMstRepository = flowsheetRaiinListMstRepository;
            _flowsheetRaiinListInfRepository = flowsheetRaiinListInfRepository;
            _flowsheetRaiinListNextOrderRepository = flowsheetRaiinListNextOrderRepository;
        }

        public GetListFlowSheetOutputData Handle(GetListFlowSheetInputData inputData)
        {
            try
            {
                if (inputData.IsHolidayOnly)
                {
                    var holidayMstList = _flowsheetRepository.GetHolidayMst(inputData.HpId, inputData.HolidayFrom, inputData.HolidayTo);
                    return new GetListFlowSheetOutputData(holidayMstList.Select(h => new HolidayDto(h.SeqNo, h.SinDate, h.HolidayKbn, h.KyusinKbn, h.HolidayName)).ToList());
                }
                else
                {
                    long totalFlowSheet = 0;
                    var stopwatch = Stopwatch.StartNew();
                    Console.WriteLine("Start FlowSheet Interactor");

                    //var flowsheetList = 
                    var taskFlowsheetList = Task<List<FlowSheetModel>>.Factory.StartNew(() => _flowsheetRepository.GetListFlowSheet(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo, ref totalFlowSheet));

                    Console.WriteLine("Stop FlowSheet Interactor: " + stopwatch.ElapsedMilliseconds);
                    var taskRaiinListMst = Task<List<RaiinListMstModel>>.Factory.StartNew(() => _flowsheetRaiinListMstRepository.GetRaiinListMsts(inputData.HpId));
                    Console.WriteLine("Stop RaiinList Interactor: " + stopwatch.ElapsedMilliseconds);
                    var taskRaiinList = Task<Dictionary<long, List<RaiinListInfModel>>>.Factory.StartNew(() => _flowsheetRaiinListInfRepository.GetRaiinListInf(inputData.HpId, inputData.PtId));
                    Console.WriteLine("Stop raiinListInfList Interactor: " + stopwatch.ElapsedMilliseconds);
                    var taskRaiinListInfNextOrder = Task<Dictionary<int, List<RaiinListInfModel>>>.Factory.StartNew(() => _flowsheetRaiinListNextOrderRepository.GetRaiinListInfForNextOrder(inputData.HpId, inputData.PtId));
                    Task.WaitAll(taskFlowsheetList, taskRaiinListMst, taskRaiinList, taskRaiinListInfNextOrder);
                    Console.WriteLine("Stop raiinListInfForNextOrderList Interactor: " + stopwatch.ElapsedMilliseconds);

                    var flowsheetList = taskFlowsheetList.Result;
                    var raiinListMst = taskRaiinListMst.Result;
                    var raiinListInfList = taskRaiinList.Result;
                    var raiinListInfForNextOrderList = taskRaiinListInfNextOrder.Result;

                    return new GetListFlowSheetOutputData(flowsheetList, raiinListMst, raiinListInfList, raiinListInfForNextOrderList, totalFlowSheet);
                }
            }
            finally
            {
                _flowsheetRepository.ReleaseResource();
                _flowsheetRaiinListMstRepository.ReleaseResource();
                _flowsheetRaiinListInfRepository.ReleaseResource();
                _flowsheetRaiinListNextOrderRepository.ReleaseResource();
            }
        }
    }
}