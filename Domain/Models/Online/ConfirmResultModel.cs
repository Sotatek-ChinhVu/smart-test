using Domain.Models.Insurance;

namespace Domain.Models.Online
{
    public class ConfirmResultModel
    {
        public ConfirmResultModel(long ptId, int sinDate, string birthday, string sex1, string sex2, string address, string insuredName, string postNumber, string nameKana, string name, string insurerNumber, string insuredCardSymbol, string insuredIdentificationNumber, string insuredBranchNumber, string personalFamilyClassification, string insuredCertificateIssuanceDate, string insuredCardValidDate, string insuredCardExpirationDate, string limitApplicationCertificateClassificationFlag, string processingResultMessage, string confirmationResult, int confirmationStatus, long referenceNo, ConfirmDateModel ptHokenCheckModel)
        {
            PtId = ptId;
            SinDate = sinDate;
            Birthday = birthday;
            Sex1 = sex1;
            Sex2 = sex2;
            Address = address;
            InsuredName = insuredName;
            PostNumber = postNumber;
            NameKana = nameKana;
            Name = name;
            InsurerNumber = insurerNumber;
            InsuredCardSymbol = insuredCardSymbol;
            InsuredIdentificationNumber = insuredIdentificationNumber;
            InsuredBranchNumber = insuredBranchNumber;
            PersonalFamilyClassification = personalFamilyClassification;
            InsuredCertificateIssuanceDate = insuredCertificateIssuanceDate;
            InsuredCardValidDate = insuredCardValidDate;
            InsuredCardExpirationDate = insuredCardExpirationDate;
            LimitApplicationCertificateClassificationFlag = limitApplicationCertificateClassificationFlag;
            ProcessingResultMessage = processingResultMessage;
            ConfirmationResult = confirmationResult;
            ConfirmationStatus = confirmationStatus;
            ReferenceNo = referenceNo;
            PtHokenCheckModel = ptHokenCheckModel;
        }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public string Birthday { get; private set; }

        public string Sex1 { get; private set; }

        public string Sex2 { get; private set; }

        public string Address { get; private set; }

        public string InsuredName { get; private set; }

        public string PostNumber { get; private set; }

        public string NameKana { get; private set; }

        public string Name { get; private set; }

        public string InsurerNumber { get; private set; }

        public string InsuredCardSymbol { get; private set; }

        public string InsuredIdentificationNumber { get; private set; }

        public string InsuredBranchNumber { get; private set; }

        public string PersonalFamilyClassification { get; private set; }

        public string InsuredCertificateIssuanceDate { get; private set; }

        public string InsuredCardValidDate { get; private set; }

        public string InsuredCardExpirationDate { get; private set; }

        public string LimitApplicationCertificateClassificationFlag { get; private set; }

        public string ProcessingResultMessage { get; private set; }

        public string ConfirmationResult { get; private set; }

        public int ConfirmationStatus { get; private set; }

        public long ReferenceNo { get; private set; }

        public ConfirmDateModel PtHokenCheckModel { get; private set; }

        public int HokenId
        {
            get => PtHokenCheckModel?.HokenId ?? 0;
        }

        public int HokenGrp
        {
            get => PtHokenCheckModel?.HokenGrp ?? 0;
        }

        public int CheckDate
        {
            get => PtHokenCheckModel?.ConfirmDate ?? 0;
        }

        public ConfirmResultModel ChangeConfirmationStatus(int confirmationStatus)
        {
            ConfirmationStatus = confirmationStatus;
            return this;
        }

        public ConfirmResultModel ChangeConfirmationResult(string confirmationResult)
        {
            ConfirmationResult = confirmationResult;
            return this;
        }
    }
}
