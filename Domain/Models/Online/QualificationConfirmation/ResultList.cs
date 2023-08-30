using System.Xml.Serialization;

namespace UseCase.Online.QualificationConfirmation;

[Serializable]
[XmlRoot(ElementName = "ResultList")]
public class ResultList
{
    [XmlElement(ElementName = "ResultOfQualificationConfirmation")]
    public ResultOfQualificationConfirmation[] ResultOfQualificationConfirmation { get; set; }
}
