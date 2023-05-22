﻿using Domain.Models.User;
using Interactor.CalculateService;
using UseCase.Accounting.Recaculate;
using UseCase.MedicalExamination.Calculate;

namespace Interactor.MedicalExamination
{
    public class CalculateInteractor : ICalculateInputPort
    {
        private readonly ICalculateService _calculateRepository;
        private readonly IUserRepository _userRepository;

        public CalculateInteractor(ICalculateService calculateRepository, IUserRepository userRepository)
        {
            _calculateRepository = calculateRepository;
            _userRepository = userRepository;
        }

        public CalculateOutputData Handle(CalculateInputData inputData)
        {
            var checkLockMedical = _userRepository.CheckLockMedicalExamination(inputData.HpId, inputData.PtId, inputData.RaiinNo, inputData.SinDate, inputData.UserId);
            if (checkLockMedical)
            {
                return new CalculateOutputData(inputData.PtId, inputData.SinDate, CalculateStatus.MedicalScreenLocked);
            }
            var check = false;
            if (inputData.FromRcCheck)
            {
                var checkRunCalculateOne = _calculateRepository.RunCalculateOne(new CalculateOneRequest(
                        inputData.HpId,
                        inputData.PtId,
                        inputData.SinDate,
                        inputData.SeikyuUp,
                        inputData.Prefix
                    ));

                var checkReceFutanCalculateMain = _calculateRepository.ReceFutanCalculateMain(new ReceCalculateRequest(new List<long> { inputData.PtId }, inputData.SinDate / 100));
                check = checkRunCalculateOne && checkReceFutanCalculateMain;
            }
            else
            {
                check = _calculateRepository.RunCalculate(new RecaculationInputDto(
                        inputData.HpId,
                        inputData.PtId,
                        inputData.SinDate,
                        inputData.SeikyuUp,
                        inputData.Prefix
                    ));
            }

            if (check)
            {
                return new CalculateOutputData(inputData.PtId, inputData.SinDate, CalculateStatus.Successed);
            }

            return new CalculateOutputData(inputData.PtId, inputData.SinDate, CalculateStatus.Failed);
        }
    }
}
