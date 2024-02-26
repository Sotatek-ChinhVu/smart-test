namespace EmrCloudApi.Requests.SwapHoken
{
    public class CalculationSwapHokenRequest
    {
        public CalculationSwapHokenRequest(List<int> seikyuYms, long ptId, bool isReCalculation, bool isReceCalculation, bool isReceCheckError)
        {
            SeikyuYms = seikyuYms;
            PtId = ptId;
            IsReCalculation = isReCalculation;
            IsReceCalculation = isReceCalculation;
            IsReceCheckError = isReceCheckError;
        }

        public List<int> SeikyuYms { get; private set; }

        public long PtId { get; private set; }

        #region caculate module client
        public bool IsReCalculation { get; private set; }

        public bool IsReceCalculation { get; private set; }

        public bool IsReceCheckError { get; private set; }
        #endregion
    }
}
