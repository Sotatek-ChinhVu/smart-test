namespace OnlineService.Response
{
    public class GetXmlResponse
    {
        public List<XmlFileInfo> FileList { get; set; } = new List<XmlFileInfo>();
    }

    public class XmlFileInfo
    {
        public string Filename { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
    }
}
