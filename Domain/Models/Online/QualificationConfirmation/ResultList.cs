using System.Xml.Serialization;

namespace Domain.Models.Online.QualificationConfirmation;

[Serializable]
[XmlRoot(ElementName = "ResultList")]
public class ResultList
{
    [XmlElement(ElementName = "ResultOfQualificationConfirmation")]
    public ResultOfQualificationConfirmation[] ResultOfQualificationConfirmation { get; set; }
}
