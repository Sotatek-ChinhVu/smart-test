﻿using Domain.Models.Document;
using Domain.Models.HpInf;
using Domain.Models.User;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.Document.SaveListDocCategory;

namespace Interactor.Document;

public class SaveListDocCategoryInteractor : ISaveListDocCategoryInputPort
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IHpInfRepository _hpInfRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILoggingHandler _loggingHandler;
    private readonly ITenantProvider _tenantProvider;

    public SaveListDocCategoryInteractor(ITenantProvider tenantProvider, IDocumentRepository documentRepository, IHpInfRepository hpInfRepository, IUserRepository userRepository)
    {
        _documentRepository = documentRepository;
        _hpInfRepository = hpInfRepository;
        _userRepository = userRepository;
        _tenantProvider = tenantProvider;
        _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
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
        catch (Exception ex)
        {
            _loggingHandler.WriteLogExceptionAsync(ex);
            throw;
        }
        finally
        {
            _documentRepository.ReleaseResource();
            _hpInfRepository.ReleaseResource();
            _userRepository.ReleaseResource();
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
