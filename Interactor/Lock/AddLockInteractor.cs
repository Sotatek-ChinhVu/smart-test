using Domain.Models.Lock;
using Domain.Models.User;
using UseCase.Lock.Add;

namespace Interactor.Lock
{
    public class AddLockInteractor : IAddLockInputPort
    {
        private readonly ILockRepository _lockRepository;
        private readonly IUserRepository _userRepository;
        public AddLockInteractor(ILockRepository lockRepository, IUserRepository userRepository)
        {
            _lockRepository = lockRepository;
            _userRepository = userRepository;
        }

        public AddLockOutputData Handle(AddLockInputData inputData)
        {
            try
            {
                string functionCode = inputData.FunctionCode;
                long ptId = inputData.PtId;
                long raiinNo = inputData.RaiinNo;
                int sinDate = inputData.SinDate;
                int hpId = inputData.HpId;
                int userId = inputData.UserId;

                bool result = _lockRepository.AddLock(hpId, functionCode, ptId, sinDate, raiinNo, userId, inputData.TabKey, inputData.LoginKey);
                if (result)
                {
                    var responseLockList = _lockRepository.GetResponseLockModel(hpId, ptId, sinDate, raiinNo);
                    return new AddLockOutputData(AddLockStatus.Successed, new LockModel(), responseLockList, inputData);
                }
                else
                {
                    string userName = _userRepository.GetByUserId(userId)?.Name ?? string.Empty;
                    var lockInfList = _lockRepository.GetLock(hpId, functionCode, ptId, sinDate, raiinNo, userId);
                    var functionName = _lockRepository.GetFunctionNameLock(functionCode);
                    return new AddLockOutputData(AddLockStatus.Existed, lockInfList.FirstOrDefault() ?? new LockModel(functionCode, userId, userName, functionName), new());
                }
            }
            finally
            {
                _lockRepository.ReleaseResource();
                _userRepository.ReleaseResource();
            }
        }
    }
}
