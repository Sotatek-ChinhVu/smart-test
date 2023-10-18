﻿using Infrastructure.Interfaces;
using Infrastructure.Logger;
using Interactor.CommonChecker.CommonMedicalCheck;
using UseCase.CommonChecker;

namespace Interactor.CommonChecker
{
    public class CommonCheckerInteractor : IGetOrderCheckerInputPort
    {
        private readonly ICommonMedicalCheck _commonMedicalCheck;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public CommonCheckerInteractor(ITenantProvider tenantProvider, ICommonMedicalCheck commonMedicalCheck)
        {
            _commonMedicalCheck = commonMedicalCheck;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public GetOrderCheckerOutputData Handle(GetOrderCheckerInputData inputData)
        {
            try
            {
                var checkedResult = _commonMedicalCheck.CheckListOrder(inputData.HpId, inputData.PtId, inputData.SinDay, inputData.CurrentListOdr, inputData.ListCheckingOrder, inputData.SpecialNoteItem, inputData.PtDiseaseModels, inputData.FamilyItems, inputData.IsDataOfDb, inputData.RealTimeCheckerCondition);

                if (checkedResult == null || checkedResult.Count == 0)
                {
                    return new GetOrderCheckerOutputData(new(), GetOrderCheckerStatus.Successed);
                }
                else
                {
                    var result = _commonMedicalCheck.GetErrorDetails(inputData.HpId, inputData.PtId, inputData.SinDay, checkedResult);
                    return new GetOrderCheckerOutputData(result ?? new(), GetOrderCheckerStatus.Error);
                }
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
        }
    }
}
