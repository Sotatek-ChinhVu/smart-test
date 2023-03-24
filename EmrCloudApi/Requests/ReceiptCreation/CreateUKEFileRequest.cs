using UseCase.ReceiptCreation.CreateUKEFile;

namespace EmrCloudApi.Requests.ReceiptCreation
{
    public class CreateUKEFileRequest
    {
        public int HpId { get; set; }

        public ModeTypeCreateUKE ModeType { get; set; }

        public int SeikyuYm { get; set; }

        public int SeikyuYmOutput { get; set; }

        public bool ChkHenreisai { get; set; }

        public bool ChkTogetsu { get; set; }

        public bool IncludeOutDrug { get; set; }

        public bool IncludeTester { get; set; }

        public int KaId { get; set; }

        public int DoctorId { get; set; }

        public int Sort { get; set; }

        public bool SkipWarningIncludeOutDrug { get; set; }

        public bool SkipWarningIncludeTester { get; set; }

        public bool SkipWarningKaId { get; set; }

        public bool SkipWarningDoctorId { get; set; }

        public bool ConfirmCreateUKEFile { get; set; }
    }
}
