namespace AWSSDK.Dto
{
    public class RDSInformation
    {
        public string RDSIdentifier { get; set; } = string.Empty;
        public string InstanceType { get; set; } = string.Empty;
        public int SizeStorage { get; set; }
        public string DBEngine { get; set; } = string.Empty;
    }
}
