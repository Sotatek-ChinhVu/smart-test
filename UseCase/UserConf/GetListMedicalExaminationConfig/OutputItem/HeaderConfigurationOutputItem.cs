namespace UseCase.UserConf.GetListMedicalExaminationConfig.OutputItem;

public class HeaderConfigurationOutputItem
{
    public HeaderConfigurationOutputItem(Dictionary<int, string> colorCode, List<int> header1, List<int> header2)
    {
        ColorCode = colorCode;
        Header1 = header1;
        Header2 = header2;
    }

    public HeaderConfigurationOutputItem()
    {
        ColorCode = new();
        Header1 = new();
        Header2 = new();
    }

    public Dictionary<int, string> ColorCode { get; private set; }

    public List<int> Header1 { get; private set; }

    public List<int> Header2 { get; private set; }
}
