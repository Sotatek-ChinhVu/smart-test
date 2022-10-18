namespace DevExpress.Models.Karte2;

public class RichTextKarteOrder
{
    public RichTextKarteOrder(string sindateDisplay, string richTextKarte, string richTextOrder)
    {
        SindateDisplay = sindateDisplay;
        RichTextKarte = richTextKarte;
        RichTextOrder = richTextOrder;
    }

    public string SindateDisplay { get; private set; }
    public string RichTextKarte { get; private set; }
    public string RichTextOrder { get; private set; }
}
