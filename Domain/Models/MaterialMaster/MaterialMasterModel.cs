using Helper.Constants;

namespace Domain.Models.MaterialMaster
{
    public class MaterialMasterModel
    {
        public MaterialMasterModel(long materialCd, string materialName, ModelStatus materialModelStatus)
        {
            MaterialCd = materialCd;
            MaterialName = materialName;
            MaterialModelStatus = materialModelStatus;
        }

        public long MaterialCd { get; private set; }

        public string MaterialName { get; private set; }

        public ModelStatus MaterialModelStatus { get; private set; }
    }
}