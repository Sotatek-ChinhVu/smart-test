using Domain.Models.Document;
using Domain.Models.HpMst;
using Domain.Models.User;
using UseCase.Document.SaveListDocCategory;

namespace Interactor.Document;

public class SaveListDocCategoryInteractor : ISaveListDocCategoryInputPort
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IHpInfRepository _hpInfRepository;
    private readonly IUserRepository _userRepository;

    public SaveListDocCategoryInteractor(IDocumentRepository documentRepository, IHpInfRepository hpInfRepository, IUserRepository userRepository)
    {
        _documentRepository = documentRepository;
        _hpInfRepository = hpInfRepository;
        _userRepository = userRepository;
    }
    public SaveListDocCategoryOutputData Handle(SaveListDocCategoryInputData inputData)
    {
        try
        {
            var validateResult = ValidateInputItem(inputData);
            if (validateResult != SaveListDocCategoryStatus.ValidateSuccess)
            {
                return new SaveListDocCategoryOutputData(validateResult);
            }
            var listDocCategoryModel = inputData.ListDocCategoryItems
                                                .Select(input => new DocCategoryModel(
                                                        input.CategoryCd,
                                                        input.CategoryName,
                                                        input.SortNo,
                                                        input.IsDelete
                                                )).ToList();
            _documentRepository.SaveListDocCategory(inputData.HpId, inputData.UserId, listDocCategoryModel);
            return new SaveListDocCategoryOutputData(SaveListDocCategoryStatus.Successed);
        }
        catch (Exception)
        {
            return new SaveListDocCategoryOutputData(SaveListDocCategoryStatus.Failed);
        }
    }

    private SaveListDocCategoryStatus ValidateInputItem(SaveListDocCategoryInputData inputData)
    {
        if (!_hpInfRepository.CheckHpId(inputData.HpId))
        {
            return SaveListDocCategoryStatus.InvalidHpId;
        }
        else if (!_userRepository.CheckExistedUserId(inputData.UserId))
        {
            return SaveListDocCategoryStatus.InvalidUserId;
        }
        foreach (var item in inputData.ListDocCategoryItems)
        {
            if (item.CategoryCd != 0 && !_documentRepository.CheckExistDocCategory(inputData.HpId, item.CategoryCd))
            {
                return SaveListDocCategoryStatus.InvalidCategoryCd;
            }
            if (item.CategoryName.Length == 0 || _documentRepository.CheckDuplicateCategoryName(inputData.HpId, item.CategoryCd, item.CategoryName))
            {
                return SaveListDocCategoryStatus.InvalidCategoryName;
            }
            if (item.SortNo <= 0)
            {
                return SaveListDocCategoryStatus.InvalidSortNo;
            }
        }
        return SaveListDocCategoryStatus.ValidateSuccess;
    }
}
