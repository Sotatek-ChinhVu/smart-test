using System.Xml.Serialization;

namespace UseCase.XmlDTD.OnlineConfirmHistoryData
{
    [Serializable]
    [XmlRoot(ElementName = "MessageHeader")]
    public class ConfirmResultMessageHeader
    {
        [XmlElement(ElementName = "ProcessExecutionTime")]
        public string ProcessExecutionTime { get; set; }

        [XmlElement(ElementName = "QualificationConfirmationDate")]
        public string QualificationConfirmationDate { get; set; }

        [XmlElement(ElementName = "MedicalInstitutionCode")]
        public string MedicalInstitutionCode { get; set; }

        [XmlElement(ElementName = "ArbitraryFileIdentifier")]
        public string ArbitraryFileIdentifier { get; set; }

        [XmlElement(ElementName = "ReceptionNumber")]
        public string ReceptionNumber { get; set; }

        [XmlElement(ElementName = "SegmentOfResult")]
        public string SegmentOfResult { get; set; }

        [XmlElement(ElementName = "ErrorCode")]
        public string ErrorCode { get; set; }

        [XmlElement(ElementName = "ErrorMessage")]
        public string ErrorMessage { get; set; }

        [XmlElement(ElementName = "NumberOfProcessingResult")]
        public string NumberOfProcessingResult { get; set; }

        [XmlElement(ElementName = "NumberOfNormalProcessing")]
        public string NumberOfNormalProcessing { get; set; }

        [XmlElement(ElementName = "NumberOfError")]
        public string NumberOfError { get; set; }

        [XmlElement(ElementName = "CharacterCodeIdentifier")]
        public string CharacterCodeIdentifier { get; set; }
    }
}
