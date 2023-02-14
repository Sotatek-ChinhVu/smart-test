using Domain.Models.HistoryOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.MedicalExamination.GetHistory;

namespace Interactor.MedicalExamination
{
    public class GetAccountingHistoryOrderInteractor : IGetMedicalExaminationHistoryInputPort
    {
        private readonly IHistoryOrderRepository _historyOrderRepository;

        public GetAccountingHistoryOrderInteractor(IHistoryOrderRepository historyOrderRepository)
        {
            _historyOrderRepository = historyOrderRepository;
        }

        public GetMedicalExaminationHistoryOutputData Handle(GetMedicalExaminationHistoryInputData inputData)
        {
            try
            {
                var validate = Validate(inputData);
                if (validate != GetMedicalExaminationHistoryStatus.Successed)
                {
                    return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), validate, 0);
                }

                (int, List<HistoryOrderModel>) historyList = _historyOrderRepository.GetList(
                   inputData.HpId,
                   inputData.UserId,
                   inputData.PtId,
                   inputData.SinDate,
                   inputData.Offset,
                   inputData.Limit,
                   (int)inputData.FilterId,
                   inputData.DeleteConditon);

            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        private static GetMedicalExaminationHistoryStatus Validate(GetMedicalExaminationHistoryInputData inputData)
        {
            if (inputData.HpId <= 0)
            {
                return GetMedicalExaminationHistoryStatus.InvalidHpId;
            }
            if (inputData.Offset < 0)
            {
                return GetMedicalExaminationHistoryStatus.InvalidStartPage;
            }
            if (inputData.PtId <= 0)
            {
                return GetMedicalExaminationHistoryStatus.InvalidPtId;
            }
            if (inputData.SinDate <= 0)
            {
                return GetMedicalExaminationHistoryStatus.InvalidSinDate;
            }
            if (inputData.Limit <= 0)
            {
                return GetMedicalExaminationHistoryStatus.InvalidPageSize;
            }

            if (!(inputData.DeleteConditon >= 0 && inputData.DeleteConditon <= 2))
            {
                return GetMedicalExaminationHistoryStatus.InvalidDeleteCondition;
            }

            if (inputData.UserId <= 0)
            {
                return GetMedicalExaminationHistoryStatus.InvalidUserId;
            }

            if (inputData.FilterId < 0)
            {
                return GetMedicalExaminationHistoryStatus.InvalidFilterId;
            }

            return GetMedicalExaminationHistoryStatus.Successed;
        }
    }
}
