using System.Xml.Serialization;

namespace Domain.Models.Online
{
    [Serializable]
    [XmlRoot(ElementName = "SpecificDiseasesCertificateInfo")]
    public class SpecificDiseasesCertificateInfo
    {
        [XmlElement(ElementName = "SpecificDiseasesDiseaseCategory")]
        public string SpecificDiseasesDiseaseCategory { get; set; }

        [XmlElement(ElementName = "SpecificDiseasesCertificateDate")]
        public string SpecificDiseasesCertificateDate { get; set; }

        [XmlElement(ElementName = "SpecificDiseasesValidStartDate")]
        public string SpecificDiseasesValidStartDate { get; set; }

        [XmlElement(ElementName = "SpecificDiseasesValidEndDate")]
        public string SpecificDiseasesValidEndDate { get; set; }

        [XmlElement(ElementName = "SpecificDiseasesSelfPay")]
        public string SpecificDiseasesSelfPay { get; set; }
    }
}
