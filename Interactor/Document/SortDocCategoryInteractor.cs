using Domain.Models.Document;
using Domain.Models.HpMst;
using Domain.Models.User;
using UseCase.Document.SortDocCategory;

namespace Interactor.Document;

public class SortDocCategoryInteractor : ISortDocCategoryInputPort
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IHpInfRepository _hpInfRepository;
    private readonly IUserRepository _userRepository;

    public SortDocCategoryInteractor(IDocumentRepository documentRepository, IHpInfRepository hpInfRepository, IUserRepository userRepository)
    {
        _documentRepository = documentRepository;
        _hpInfRepository = hpInfRepository;
        _userRepository = userRepository;
    }

    public SortDocCategoryOutputData Handle(SortDocCategoryInputData inputData)
    {
        try
        {
            var validateResult = ValidateInputData(inputData);
            if (validateResult != SortDocCategoryStatus.ValidateSuccess)
            {
                return new SortDocCategoryOutputData(validateResult);
            }
            if (_documentRepository.SortDocCategory(inputData.HpId, inputData.UserId, inputData.MoveInCd, inputData.MoveOutCd))
            {
                return new SortDocCategoryOutputData(SortDocCategoryStatus.Successed);
            }
            return new SortDocCategoryOutputData(SortDocCategoryStatus.Failed);
        }
        catch (Exception)
        {
            return new SortDocCategoryOutputData(SortDocCategoryStatus.Failed);
        }
    }

    private SortDocCategoryStatus ValidateInputData(SortDocCategoryInputData inputData)
    {
        if (!_hpInfRepository.CheckHpId(inputData.HpId))
        {
            return SortDocCategoryStatus.InvalidHpId;
        }
        else if (!_userRepository.CheckExistedUserId(inputData.UserId))
        {
            return SortDocCategoryStatus.InvalidUserId;
        }
        else if (!_documentRepository.CheckExistDocCategory(inputData.HpId, inputData.MoveInCd))
        {
            return SortDocCategoryStatus.InvalidMoveInCd;
        }
        else if (!_documentRepository.CheckExistDocCategory(inputData.HpId, inputData.MoveOutCd))
        {
            return SortDocCategoryStatus.InvalidMoveOutCd;
        }
        return SortDocCategoryStatus.ValidateSuccess;
    }
}
