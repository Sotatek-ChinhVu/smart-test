namespace EmrCloudApi.Responses.FlowSheet
{
    public class GetFlowSheetTooltipResponse
    {
        public GetFlowSheetTooltipResponse(List<GetFlowSheetTooltipItem> getFlowSheetTooltipItems)
        {
            GetFlowSheetTooltipItems = getFlowSheetTooltipItems;
        }

        public List<GetFlowSheetTooltipItem> GetFlowSheetTooltipItems { get; private set; }

    }

    public class GetFlowSheetTooltipItem
    {
        public GetFlowSheetTooltipItem(int date, string tooltip)
        {
            Date = date;
            Tooltip = tooltip;
        }

        public int Date { get; private set; }

        public string Tooltip { get; private set; }
    }
}
