using UseCase.Core.Sync.Core;

namespace UseCase.SwapHoken.Calculation
{
    public class CalculationSwapHokenInputData : IInputData<CalculationSwapHokenOutputData>
    {
        public CalculationSwapHokenInputData(int hpId, List<int> seikyuYms, int ptId, bool isReCalculation, bool isReceCalculation, bool isReceCheckError)
        {
            HpId = hpId;
            SeikyuYms = seikyuYms;
            PtId = ptId;
            IsReCalculation = isReCalculation;
            IsReceCalculation = isReceCalculation;
            IsReceCheckError = isReceCheckError;
        }

        public int HpId { get; private set; }

        public List<int> SeikyuYms { get; private set; }

        public int PtId { get; private set; }

        #region caculate module client
        public bool IsReCalculation { get; private set; }

        public bool IsReceCalculation { get; private set; }

        public bool IsReceCheckError { get; private set; }
        #endregion
    }
}
