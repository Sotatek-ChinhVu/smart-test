using Domain.Models.MstItem;
using Domain.Models.OrdInf;

namespace Domain.Models.OrdInfs
{
    public interface IMedicalExaminationRepository
    {
        OrdInfModel CheckValidationCommonToSave(int ordId);
    }
}
