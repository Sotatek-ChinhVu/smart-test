using Domain.Models.SwapHoken;
using UseCase.SwapHoken.Save;

namespace Interactor.SwapHoken
{
    public class SaveSwapHokenInteractor : ISaveSwapHokenInputPort
    {
        private readonly ISwapHokenRepository _swapHokenRepository;

        public SaveSwapHokenInteractor(ISwapHokenRepository repository)
        {
            _swapHokenRepository = repository;
        }

        public SaveSwapHokenOutputData Handle(SaveSwapHokenInputData inputData)
        {
            if (inputData.HokenIdBefore <= 0)
                return new SaveSwapHokenOutputData(SaveSwapHokenStatus.SourceInsuranceHasNotSelected);

            if (inputData.HokenIdAfter <= 0)
                return new SaveSwapHokenOutputData(SaveSwapHokenStatus.DesInsuranceHasNotSelected);

            if (inputData.StartDate > inputData.EndDate && inputData.StartDate > 0 && inputData.EndDate > 0)
                return new SaveSwapHokenOutputData(SaveSwapHokenStatus.StartDateGreaterThanEndDate);

            long count = _swapHokenRepository.CountOdrInf(inputData.HpId, inputData.PtId, inputData.HokenPidBefore, inputData.StartDate, inputData.EndDate);
            if (count == 0)
                return new SaveSwapHokenOutputData(SaveSwapHokenStatus.CantExecCauseNotValidDate);

            var seikyuYms = _swapHokenRepository.GetListSeikyuYms(inputData.HpId,inputData.PtId,inputData.HokenPidBefore, inputData.StartDate, inputData.EndDate);
            var seiKyuPendingYms = _swapHokenRepository.GetSeikyuYmsInPendingSeikyu(inputData.HpId, inputData.PtId, seikyuYms, inputData.HokenIdBefore);

            bool swapHokenResult = _swapHokenRepository.SwapHokenParttern(inputData.HpId, inputData.PtId, inputData.HokenPidBefore, inputData.HokenPidAfter, inputData.StartDate, inputData.EndDate, inputData.UserId);

            if(!swapHokenResult)
                return new SaveSwapHokenOutputData(SaveSwapHokenStatus.Failed);

            if (seiKyuPendingYms.Count > 0)
            {
                if (!_swapHokenRepository.ExistRaiinInfUsedOldHokenId(inputData.HpId, inputData.PtId, seikyuYms, inputData.HokenPidBefore))
                    _swapHokenRepository.UpdateReceSeikyu(inputData.HpId, inputData.PtId, seiKyuPendingYms, inputData.HokenIdBefore, inputData.HokenIdAfter, inputData.UserId);
            }

            return new SaveSwapHokenOutputData(SaveSwapHokenStatus.Successful);
        }
    }
}
