using UseCase.Core.Sync.Core;
using UseCase.Receipt.CreateUKEFile;

namespace UseCase.Receipt.ValidateCreateUKEFile
{
    public class ValidateCreateUKEFileInputData : IInputData<ValidateCreateUKEFileOutputData>
    {
        public ValidateCreateUKEFileInputData(int hpId, int seikyuYm, ModeTypeCreateUKE modeType)
        {
            HpId = hpId;
            SeikyuYm = seikyuYm;
            ModeType = modeType;
        }

        public int HpId { get; private set; }

        public int SeikyuYm { get; private set; }

        public ModeTypeCreateUKE ModeType { get; private set; }
    }
}
