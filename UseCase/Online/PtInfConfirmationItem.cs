namespace UseCase.Online;

public class PtInfConfirmationItem
{
    public PtInfConfirmationItem(string attributeName, string currentValue, string xmlValue)
    {
        AttributeName = attributeName;
        CurrentValue = currentValue;
        XmlValue = xmlValue;
    }

    public string AttributeName { get; private set; }

    public string CurrentValue { get; private set; }

    public string XmlValue { get; private set; }
}
