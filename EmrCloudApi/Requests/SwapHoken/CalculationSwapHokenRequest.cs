namespace EmrCloudApi.Requests.SwapHoken
{
    public class CalculationSwapHokenRequest
    {
        public List<int> SeikyuYms { get; private set; }

        public int PtId { get; private set; }

        #region caculate module client
        public bool IsReCalculation { get; private set; }

        public bool IsReceCalculation { get; private set; }

        public bool IsReceCheckError { get; private set; }
        #endregion
    }
}
