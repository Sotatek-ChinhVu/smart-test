using Interactor.Family.ValidateFamilyList;
using UseCase.Family;
using UseCase.Family.ValidateFamilyList;

namespace Interactor.Family;

public class ValidateFamilyListInteractor : IValidateFamilyListInputPort
{
    private readonly IValidateFamilyList _validateFamilyList;

    public ValidateFamilyListInteractor(IValidateFamilyList validateFamilyList)
    {
        _validateFamilyList = validateFamilyList;
    }

    public ValidateFamilyListOutputData Handle(ValidateFamilyListInputData inputData)
    {
        try
        {
            var validateResult = _validateFamilyList.ValidateData(inputData.HpId, inputData.PtId, inputData.ListFamily);
            if (validateResult != ValidateFamilyListStatus.ValidateSuccess)
            {
                return new ValidateFamilyListOutputData(validateResult);
            }
            return new ValidateFamilyListOutputData(ValidateFamilyListStatus.Successed);
        }
        finally
        {
            _validateFamilyList.ReleaseResource();
        }
    }
}
