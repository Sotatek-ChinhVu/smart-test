using Domain.Models.SwapHoken;
using Helper.Common;
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
            string sBuff = string.Empty;
            if (inputData.HokenIdBefore <= 0)
                return new SaveSwapHokenOutputData(SaveSwapHokenStatus.SourceInsuranceHasNotSelected);

            if (inputData.HokenIdAfter <= 0)
                return new SaveSwapHokenOutputData(SaveSwapHokenStatus.DesInsuranceHasNotSelected);

            if (inputData.StartDate > inputData.EndDate && inputData.StartDate > 0 && inputData.EndDate > 0)
                return new SaveSwapHokenOutputData(SaveSwapHokenStatus.Successful);

            if (inputData.StartDate > 0 && inputData.EndDate > 0)
                sBuff = CIUtil.SDateToShowSDate(inputData.StartDate) + " ～ " + CIUtil.SDateToShowSDate(inputData.EndDate);
            else if (inputData.StartDate > 0 && inputData.EndDate == 0)
                sBuff = CIUtil.SDateToShowSDate(inputData.StartDate) + " 以降 ";

            else if (inputData.StartDate == 0 && inputData.EndDate > 0)
                sBuff = CIUtil.SDateToShowSDate(inputData.EndDate) + " まで ";

            else if (inputData.StartDate == 0 && inputData.EndDate == 0)
                sBuff = "";

            if (string.IsNullOrEmpty(sBuff))
            {
                inputData.EndDate = 99999999;
                sBuff = "すべて";
            }
            else
            {
                long count = _swapHokenRepository.CountOdrInf(inputData.HpId,inputData.PtId, inputData.HokenPidBefore, inputData.StartDate, inputData.EndDate);
                if (count == 0)
                    return new SaveSwapHokenOutputData(SaveSwapHokenStatus.CantExecCauseNotValidDate);
            }

            List<long> ptIds = new List<long>() { inputData.PtId };
            var seikyuYms = _swapHokenRepository.GetListSeikyuYms(inputData.HpId,inputData.PtId,inputData.HokenPidBefore, inputData.StartDate, inputData.EndDate);
            var seiKyuPendingYms = _swapHokenRepository.GetSeikyuYmsInPendingSeikyu(inputData.HpId, inputData.PtId, seikyuYms, inputData.HokenIdBefore);

            bool swapHokenResult = _swapHokenRepository.SwapHokenParttern(inputData.HpId, inputData.PtId, inputData.HokenPidBefore, inputData.HokenPidAfter, inputData.StartDate, inputData.EndDate);

            if(!swapHokenResult)
                return new SaveSwapHokenOutputData(SaveSwapHokenStatus.Failed);

            if (seiKyuPendingYms.Count > 0)
            {
                if (!_swapHokenRepository.ExistRaiinInfUsedOldHokenId(inputData.HpId, inputData.PtId, seikyuYms, inputData.HokenPidBefore))
                {
                    _swapHokenRepository.UpdateReceSeikyu(inputData.HpId, inputData.PtId, seiKyuPendingYms, inputData.HokenIdBefore, inputData.HokenIdAfter);
                }
                seikyuYms.AddRange(seiKyuPendingYms);
            }

            return new SaveSwapHokenOutputData(SaveSwapHokenStatus.Successful);
        }
    }
}
