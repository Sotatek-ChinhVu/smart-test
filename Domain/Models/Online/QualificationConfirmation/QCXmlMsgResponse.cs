using System.Xml.Serialization;

namespace Domain.Models.Online.QualificationConfirmation;

/// <summary>
/// 資格確認結果
/// OQSsiquc01res_xxxxxxxxxxxx.xml
/// </summary>
[Serializable]
[XmlRoot(ElementName = "XmlMsg")]
public class QCXmlMsgResponse
{
    [XmlElement(ElementName = "MessageHeader")]
    public MessageHeader MessageHeader { get; set; }
    [XmlElement(ElementName = "MessageBody")]
    public MessageBody MessageBody { get; set; }
}
