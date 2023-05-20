using UseCase.Receipt.CreateUKEFile;

namespace EmrCloudApi.Requests.Receipt
{
    public class ValidateCreateUKEFileRequest
    {
        public int SeikyuYm { get; set; }

        public ModeTypeCreateUKE ModeType { get; set; }
    }
}
