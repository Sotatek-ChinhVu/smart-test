using Interactor.CommonChecker.CommonMedicalCheck;
using UseCase.CommonChecker;

namespace Interactor.CommonChecker
{
    public class CommonCheckerInteractor : IGetOrderCheckerInputPort
    {
        private readonly ICommonMedicalCheck _commonMedicalCheck;

        public CommonCheckerInteractor(ICommonMedicalCheck commonMedicalCheck)
        {
            _commonMedicalCheck = commonMedicalCheck;
        }

        public GetOrderCheckerOutputData Handle(GetOrderCheckerInputData inputData)
        {
            var checkedResult = _commonMedicalCheck.CheckListOrder(inputData.HpId, inputData.PtId, inputData.SinDay, inputData.CurrentListOdr, inputData.ListCheckingOrder);

            var result = _commonMedicalCheck.GetErrorDetails(inputData.HpId, inputData.PtId, inputData.SinDay, checkedResult);

            if (checkedResult == null || checkedResult.Count == 0)
            {
                return new GetOrderCheckerOutputData(new(), GetOrderCheckerStatus.Successed);
            }
            else
            {
                return new GetOrderCheckerOutputData(result ?? new(), GetOrderCheckerStatus.Error);
            }
        }
    }
}
