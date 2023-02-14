using Domain.Models.HistoryOrder;
using UseCase.Accounting.GetHistoryOrder;
using UseCase.MedicalExamination.GetHistory;

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
                var validate = Validate(inputData);
                if (validate != GetAccountingHistoryOrderStatus.Successed)
                {
                    return new GetAccountingHistoryOrderOutputData(0, new List<HistoryOrderModel>(), GetAccountingHistoryOrderStatus.NoData);
                }

                (int, List<HistoryOrderModel>) historyList = _historyOrderRepository.GetList(
                   inputData.HpId,
                   inputData.UserId,
                   inputData.PtId,
                   inputData.SinDate,
                   inputData.Offset,
                   inputData.Limit,
                   (int)inputData.FilterId,
                   inputData.DeleteConditon,
                   inputData.RaiinNo,
                   false
                   );
                return new GetAccountingHistoryOrderOutputData(historyList.Item1, historyList.Item2, GetAccountingHistoryOrderStatus.Successed);
            }
            catch (Exception)
            {
                return new GetAccountingHistoryOrderOutputData(0, new List<HistoryOrderModel>(), GetAccountingHistoryOrderStatus.Failed);
            }
            finally
            {
                _historyOrderRepository.ReleaseResource();
            }
        }

        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        private static GetAccountingHistoryOrderStatus Validate(GetAccountingHistoryOrderInputData inputData)
        {
            if (inputData.Offset < 0)
            {
                return GetAccountingHistoryOrderStatus.InvalidStartPage;
            }
            if (inputData.PtId <= 0)
            {
                return GetAccountingHistoryOrderStatus.InvalidPtId;
            }
            if (inputData.SinDate <= 0)
            {
                return GetAccountingHistoryOrderStatus.InvalidSinDate;
            }
            if (inputData.Limit <= 0)
            {
                return GetAccountingHistoryOrderStatus.InvalidPageSize;
            }

            if (!(inputData.DeleteConditon >= 0 && inputData.DeleteConditon <= 2))
            {
                return GetAccountingHistoryOrderStatus.InvalidDeleteCondition;
            }

            if (inputData.UserId <= 0)
            {
                return GetAccountingHistoryOrderStatus.InvalidUserId;
            }

            if (inputData.FilterId < 0)
            {
                return GetAccountingHistoryOrderStatus.InvalidFilterId;
            }

            if (inputData.RaiinNo <= 0)
            {
                return GetAccountingHistoryOrderStatus.InvalidRaiinNo;
            }

            return GetAccountingHistoryOrderStatus.Successed;
        }

    }
}
