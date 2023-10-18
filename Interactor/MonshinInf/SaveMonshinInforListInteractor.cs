﻿using Domain.Models.MonshinInf;
using Domain.Models.Reception;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.MonshinInfor.Save;
using static Helper.Constants.MonshinInfConst;

namespace Interactor.MonshinInf
{
    public class SaveMonshinInforListInteractor : ISaveMonshinInputPort
    {
        private readonly IMonshinInforRepository _monshinInforRepository;
        private readonly IReceptionRepository _receptionRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public SaveMonshinInforListInteractor(ITenantProvider tenantProvider, IMonshinInforRepository monshinInforRepository, IReceptionRepository receptionRepository)
        {
            _monshinInforRepository = monshinInforRepository;
            _receptionRepository = receptionRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public SaveMonshinOutputData Handle(SaveMonshinInputData inputData)
        {
            try
            {
                if (inputData.MonshinInfors.Any())
                {
                    foreach (var item in inputData.MonshinInfors)
                    {
                        var validationStatus = item.Validation();
                        if (validationStatus != ValidationStatus.Valid)
                            return new SaveMonshinOutputData(ConvertStatus(validationStatus));

                        if (!_receptionRepository.CheckExistReception(item.HpId, item.PtId, item.SinDate, item.RaiinNo))
                            return new SaveMonshinOutputData(SaveMonshinStatus.InputDataDoesNotExists);
                    }
                }
                else
                    return new SaveMonshinOutputData(SaveMonshinStatus.InputDataNull);

                bool success = _monshinInforRepository.SaveList(inputData.MonshinInfors, inputData.UserId);
                var status = success ? SaveMonshinStatus.Success : SaveMonshinStatus.Failed;
                return new SaveMonshinOutputData(status);
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _monshinInforRepository.ReleaseResource();
                _receptionRepository.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }

        private SaveMonshinStatus ConvertStatus(ValidationStatus status)
        {
            if (status == ValidationStatus.InvalidPtId)
                return SaveMonshinStatus.InvalidPtId;

            if (status == ValidationStatus.InvalidHpId)
                return SaveMonshinStatus.InvalidHpId;

            if (status == ValidationStatus.InvalidSinDate)
                return SaveMonshinStatus.InvalidSinDate;

            if (status == ValidationStatus.InValidRaiinNo)
                return SaveMonshinStatus.InValidRaiinNo;

            return SaveMonshinStatus.Success;
        }
    }
}
