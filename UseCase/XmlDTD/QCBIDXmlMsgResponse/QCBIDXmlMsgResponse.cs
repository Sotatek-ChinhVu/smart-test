using Domain.Models.Online.QualificationConfirmation;
using System.Xml.Serialization;

namespace UseCase.XmlDTD.QCBIDXmlMsgResponse
{
    /// <summary>
    /// 資格確認一括照会結果
    /// OQSmuquc02res_xxxxxxxxxxxx.xml
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "XmlMsg")]
    public class QCBIDXmlMsgResponse
    {
        [XmlElement(ElementName = "MessageHeader")]
        public MessageHeader? MessageHeader { get; set; } = null;
        [XmlElement(ElementName = "MessageBody")]
        public MessageBody? MessageBody { get; set; } = null;
    }

    [Serializable]
    [XmlRoot(ElementName = "MessageHeader")]
    public class MessageHeader
    {
        [XmlElement(ElementName = "ProcessExecutionTime")]
        public string ProcessExecutionTime { get; set; } = string.Empty;

        [XmlElement(ElementName = "QualificationConfirmationDate")]
        public string QualificationConfirmationDate { get; set; } = string.Empty;

        [XmlElement(ElementName = "MedicalInstitutionCode")]
        public string MedicalInstitutionCode { get; set; } = string.Empty;

        [XmlElement(ElementName = "ArbitraryFileIdentifier")]
        public string ArbitraryFileIdentifier { get; set; } = string.Empty;

        [XmlElement(ElementName = "ReceptionNumber")]
        public string ReceptionNumber { get; set; } = string.Empty;

        [XmlElement(ElementName = "SegmentOfResult")]
        public string SegmentOfResult { get; set; } = string.Empty;

        [XmlElement(ElementName = "ErrorCode")]
        public string ErrorCode { get; set; } = string.Empty;

        [XmlElement(ElementName = "ErrorMessage")]
        public string ErrorMessage { get; set; } = string.Empty;

        [XmlElement(ElementName = "NumberOfProcessingResult")]
        public string NumberOfProcessingResult { get; set; } = string.Empty;

        [XmlElement(ElementName = "NumberOfNormalProcessing")]
        public string NumberOfNormalProcessing { get; set; } = string.Empty;

        [XmlElement(ElementName = "NumberOfError")]
        public string NumberOfError { get; set; } = string.Empty;

        [XmlElement(ElementName = "CharacterCodeIdentifier")]
        public string CharacterCodeIdentifier { get; set; } = string.Empty;
    }

    [Serializable]
    [XmlRoot(ElementName = "MessageBody")]
    public class MessageBody
    {
        [XmlElement(ElementName = "BulkConfirmUnit")]
        public QCBIDXmlMsgResponseInfo[] QCBIDXmlMsgResponseInfo { get; set; } = new QCBIDXmlMsgResponseInfo[0];
    }

    [Serializable]
    [XmlRoot(ElementName = "BulkConfirmUnit")]
    public class QCBIDXmlMsgResponseInfo
    {
        [XmlElement(ElementName = "QualificationConfirmSearchInfo")]
        public QualificationConfirmSearchInfo QualificationConfirmSearchInfo { get; set; } = new();

        [XmlElement(ElementName = "ProcessingResultStatus")]
        public string ProcessingResultStatus { get; set; } = string.Empty;

        [XmlElement(ElementName = "ProcessingResultCode")]
        public string ProcessingResultCode { get; set; } = string.Empty;

        [XmlElement(ElementName = "ProcessingResultMessage")]
        public string ProcessingResultMessage { get; set; } = string.Empty;

        [XmlElement(ElementName = "QualificationValidity")]
        public string QualificationValidity { get; set; } = string.Empty;

        [XmlElement(ElementName = "ResultOfQualificationConfirmation")]
        public ResultOfQualificationConfirmation ResultOfQualificationConfirmation { get; set; } = new();

        [XmlElement(ElementName = "ReferenceNumber")]
        public string ReferenceNumber { get; set; } = string.Empty;
    }

    [Serializable]
    [XmlRoot(ElementName = "ResultOfQualificationConfirmation")]
    public class ResultOfQualificationConfirmation
    {
        [XmlElement(ElementName = "InsuredCardClassification")]
        public string InsuredCardClassification { get; set; } = string.Empty;

        [XmlElement(ElementName = "InsurerNumber")]
        public string InsurerNumber { get; set; } = string.Empty;

        [XmlElement(ElementName = "InsuredCardSymbol")]
        public string InsuredCardSymbol { get; set; } = string.Empty;

        [XmlElement(ElementName = "InsuredIdentificationNumber")]
        public string InsuredIdentificationNumber { get; set; } = string.Empty;

        [XmlElement(ElementName = "InsuredBranchNumber")]
        public string InsuredBranchNumber { get; set; } = string.Empty;

        [XmlElement(ElementName = "PersonalFamilyClassification")]
        public string PersonalFamilyClassification { get; set; } = string.Empty;

        [XmlElement(ElementName = "InsuredName")]
        public string InsuredName { get; set; } = string.Empty;

        [XmlElement(ElementName = "Name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement(ElementName = "NameOfOther")]
        public string NameOfOther { get; set; } = string.Empty;

        [XmlElement(ElementName = "NameKana")]
        public string NameKana { get; set; } = string.Empty;

        [XmlElement(ElementName = "NameOfOtherKana")]
        public string NameOfOtherKana { get; set; } = string.Empty;

        [XmlElement(ElementName = "Sex1")]
        public string Sex1 { get; set; } = string.Empty;

        [XmlElement(ElementName = "Sex2")]
        public string Sex2 { get; set; } = string.Empty;

        [XmlElement(ElementName = "Birthdate")]
        public string Birthdate { get; set; } = string.Empty;

        [XmlElement(ElementName = "Address")]
        public string Address { get; set; } = string.Empty;

        [XmlElement(ElementName = "PostNumber")]
        public string PostNumber { get; set; } = string.Empty;

        [XmlElement(ElementName = "InsuredCertificateIssuanceDate")]
        public string InsuredCertificateIssuanceDate { get; set; } = string.Empty;

        [XmlElement(ElementName = "InsuredCardValidDate")]
        public string InsuredCardValidDate { get; set; } = string.Empty;

        [XmlElement(ElementName = "InsuredCardExpirationDate")]
        public string InsuredCardExpirationDate { get; set; } = string.Empty;

        [XmlElement(ElementName = "InsuredPartialContributionRatio")]
        public string InsuredPartialContributionRatio { get; set; } = string.Empty;

        [XmlElement(ElementName = "PreschoolClassification")]
        public string PreschoolClassification { get; set; } = string.Empty;

        [XmlElement(ElementName = "ReasonOfLoss")]
        public string ReasonOfLoss { get; set; } = string.Empty;

        [XmlElement(ElementName = "InsurerName")]
        public string InsurerName { get; set; } = string.Empty;

        [XmlElement(ElementName = "ElderlyRecipientCertificateInfo")]
        public ElderlyRecipientCertificateInfo ElderlyRecipientCertificateInfo { get; set; } = new();

        [XmlElement(ElementName = "LimitApplicationCertificateChanged")]
        public string LimitApplicationCertificateChanged { get; set; } = string.Empty;
    }
}
