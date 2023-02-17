using Domain.Models.SwapHoken;
using Helper.Common;
using UseCase.SwapHoken.Validate;

namespace Interactor.SwapHoken
{
    public class ValidateSwapHokenInteractor : IValidateSwapHokenInputPort
    {
        private readonly ISwapHokenRepository _swapHokenRepository;

        public ValidateSwapHokenInteractor(ISwapHokenRepository repository)
        {
            _swapHokenRepository = repository;
        }

        public ValidateSwapHokenOutputData Handle(ValidateSwapHokenInputData inputData)
        {
            try
            {
                if (inputData.HpId < 0)
                    return new ValidateSwapHokenOutputData(ValidateSwapHokenStatus.InvalidHpId, string.Empty);

                if (inputData.PtId < 0)
                    return new ValidateSwapHokenOutputData(ValidateSwapHokenStatus.InvalidPtId , string.Empty);

                if (inputData.StartDate < 0)
                    return new ValidateSwapHokenOutputData(ValidateSwapHokenStatus.InvalidStartDate, string.Empty);

                if (inputData.EndDate < 0)
                    return new ValidateSwapHokenOutputData(ValidateSwapHokenStatus.InvalidEndDate, string.Empty);

                if(inputData.HokenPid < 0)
                    return new ValidateSwapHokenOutputData(ValidateSwapHokenStatus.InvalidHokenPid, string.Empty);

                string msg = string.Empty;
                if (inputData.StartDate != 0 && inputData.EndDate != 0)
                {
                    long count = _swapHokenRepository.CountOdrInf(inputData.HpId, inputData.PtId, inputData.HokenPid , inputData.StartDate, inputData.EndDate);
                    if (count == 0)
                    {
                        msg = string.Format("変換元の保険は{0}に一度も使用されていないため、" + Environment.NewLine + "選択できません。", CIUtil.SDateToShowSDate(inputData.StartDate) + " ～ " + CIUtil.SDateToShowSDate(inputData.EndDate));
                    }
                }
                else
                {
                    msg = _swapHokenRepository.IsPtHokenPatternUsed(inputData.HpId, inputData.PtId, inputData.HokenPid) ?
                        ""
                        : "変換元の保険は一度も使用されていないため、" + Environment.NewLine + "選択できません。";
                }

                if(string.IsNullOrEmpty(msg)) 
                    return new ValidateSwapHokenOutputData(ValidateSwapHokenStatus.Successful , string.Empty);
                else
                    return new ValidateSwapHokenOutputData(ValidateSwapHokenStatus.Error, msg);
            }
            finally
            {
                _swapHokenRepository.ReleaseResource();
            }
        }
    }
}
