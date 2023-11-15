using System.Xml.Serialization;

namespace UseCase.XmlDTD.QCBIXmlMsgResponse
{
    [Serializable]
    [XmlRoot(ElementName = "XmlMsg")]
    public class QCBIXmlMsgResponse
    {
        [XmlElement(ElementName = "MessageHeader")]
        public MessageHeader MessageHeader { get; set; } = new();

        [XmlElement(ElementName = "MessageBody")]
        public MessageBody MessageBody { get; set; } = new();
    }

    [Serializable]
    [XmlRoot(ElementName = "MessageHeader")]
    public class MessageHeader
    {
        [XmlElement(ElementName = "ProcessExecutionTime")]
        public string ProcessExecutionTime { get; set; } = string.Empty;

        [XmlElement(ElementName = "MedicalInstitutionCode")]
        public string MedicalInstitutionCode { get; set; } = string.Empty;

        [XmlElement(ElementName = "ArbitraryFileIdentifier")]
        public string ArbitraryFileIdentifier { get; set; } = string.Empty;

        [XmlElement(ElementName = "CharacterCodeIdentifier")]
        public string CharacterCodeIdentifier { get; set; } = string.Empty;
    }

    [Serializable]
    [XmlRoot(ElementName = "MessageBody")]
    public class MessageBody
    {
        [XmlElement(ElementName = "ReceptionNumber")]
        public string ReceptionNumber { get; set; } = string.Empty;

        [XmlElement(ElementName = "ReceptionDateTime")]
        public string ReceptionDateTime { get; set; } = string.Empty;
    }
}
