using Domain.Models.Document;
using Domain.Models.HpMst;
using Domain.Models.User;
using UseCase.Document.DeleteDocCategory;

namespace Interactor.Document;

public class DeleteDocCategoryInteractor : IDeleteDocCategoryInputPort
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IHpInfRepository _hpInfRepository;
    private readonly IUserRepository _userRepository;

    public DeleteDocCategoryInteractor(IDocumentRepository documentRepository, IHpInfRepository hpInfRepository, IUserRepository userRepository)
    {
        _documentRepository = documentRepository;
        _hpInfRepository = hpInfRepository;
        _userRepository = userRepository;
    }

    public DeleteDocCategoryOutputData Handle(DeleteDocCategoryInputData inputData)
    {
        try
        {
            var validateResult = ValidateInputData(inputData);
            if (validateResult != DeleteDocCategoryStatus.ValidateSuccess)
            {
                return new DeleteDocCategoryOutputData(validateResult);
            }
            if (_documentRepository.DeleteDocCategory(inputData.HpId, inputData.UserId, inputData.CategoryCd))
            {
                return new DeleteDocCategoryOutputData(DeleteDocCategoryStatus.Successed);
            }
            return new DeleteDocCategoryOutputData(DeleteDocCategoryStatus.Failed);
        }
        catch (Exception)
        {
            return new DeleteDocCategoryOutputData(DeleteDocCategoryStatus.Failed);
        }
    }

    private DeleteDocCategoryStatus ValidateInputData(DeleteDocCategoryInputData inputData)
    {
        if (!_hpInfRepository.CheckHpId(inputData.HpId))
        {
            return DeleteDocCategoryStatus.InvalidHpId;
        }
        else if (!_userRepository.CheckExistedUserId(inputData.UserId))
        {
            return DeleteDocCategoryStatus.InvalidUserId;
        }
        else if (!_documentRepository.CheckExistDocCategory(inputData.HpId, inputData.CategoryCd))
        {
            return DeleteDocCategoryStatus.InvalidDocCategoryCd;
        }
        return DeleteDocCategoryStatus.ValidateSuccess;
    }
}
