namespace UseCase.Accounting.GetAccountingHeader
{
    public class HeaderDto
    {
        public HeaderDto(long raiinNo, string raiinBinding, string patternName)
        {
            RaiinNo = raiinNo;
            RaiinBinding = raiinBinding;
            PatternName = patternName;
        }

        public long RaiinNo { get; private set; }
        public string RaiinBinding { get; private set; }
        public string PatternName { get; private set; }
    }
}
