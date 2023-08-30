using System.Xml.Serialization;

namespace UseCase.Online.QualificationConfirmation;

[Serializable]
[XmlRoot(ElementName = "SpecificDiseasesCertificateInfo")]
public class SpecificDiseasesCertificateInfo
{
    [XmlElement(ElementName = "SpecificDiseasesDiseaseCategory")]
    public string SpecificDiseasesDiseaseCategory { get; set; } = string.Empty;

    [XmlElement(ElementName = "SpecificDiseasesCertificateDate")]
    public string SpecificDiseasesCertificateDate { get; set; } = string.Empty;

    [XmlElement(ElementName = "SpecificDiseasesValidStartDate")]
    public string SpecificDiseasesValidStartDate { get; set; } = string.Empty;

    [XmlElement(ElementName = "SpecificDiseasesValidEndDate")]
    public string SpecificDiseasesValidEndDate { get; set; } = string.Empty;

    [XmlElement(ElementName = "SpecificDiseasesSelfPay")]
    public string SpecificDiseasesSelfPay { get; set; } = string.Empty;
}