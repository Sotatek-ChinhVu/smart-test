using Domain.Models.MstItem;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.MstItem.SaveRenkei;

namespace Interactor.MstItem;

public class SaveRenkeiInteractor : ISaveRenkeiInputPort
{
    private readonly IMstItemRepository _mstItemRepository;
    private readonly ILoggingHandler _loggingHandler;
    private readonly ITenantProvider _tenantProvider;

    public SaveRenkeiInteractor(ITenantProvider tenantProvider, IMstItemRepository mstItemRepository)
    {
        _mstItemRepository = mstItemRepository;
        _tenantProvider = tenantProvider;
        _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
    }

    public SaveRenkeiOutputData Handle(SaveRenkeiInputData inputData)
    {
        try
        {
            var validateResult = ValidateInput(inputData);
            if (validateResult != SaveRenkeiStatus.ValidateSuccess)
            {
                return new SaveRenkeiOutputData(validateResult);
            }
            if (_mstItemRepository.SaveRenkei(inputData.HpId, inputData.UserId, inputData.RenkeiTabList))
            {
                return new SaveRenkeiOutputData(SaveRenkeiStatus.Successed);
            }
            return new SaveRenkeiOutputData(SaveRenkeiStatus.Failed);
        }
        catch (Exception ex)
        {
            _loggingHandler.WriteLogExceptionAsync(ex);
            throw;
        }
        finally
        {
            _mstItemRepository.ReleaseResource();
            _loggingHandler.Dispose();
        }
    }

    private SaveRenkeiStatus ValidateInput(SaveRenkeiInputData inputData)
    {
        var renkeiMstModelList = _mstItemRepository.GetRenkeiMstModels(inputData.HpId);
        var renkeiTemplateMstModelList = _mstItemRepository.GetRenkeiTemplateMstModels(inputData.HpId);
        var eventMstModelList = _mstItemRepository.GetEventMstModelList();
        foreach (var tab in inputData.RenkeiTabList)
        {
            if (tab.renkeiSbt < 0 || tab.renkeiSbt > 1)
            {
                return SaveRenkeiStatus.InvalidRenkeiSbt;
            }
            else if (tab.renkeiSbt == 1 && tab.renkeiConfList.Any(item => item.RenkeiTimingModelList.Any()))
            {
                return SaveRenkeiStatus.InvalidRenkeiTimingModelList;
            }
            foreach (var renkeiConf in tab.renkeiConfList)
            {
                if (!renkeiMstModelList.Any(item => item.RenkeiId == renkeiConf.RenkeiId))
                {
                    return SaveRenkeiStatus.InvalidRenkeiId;
                }
                else if (!renkeiMstModelList.Where(item => item.RenkeiSbt == tab.renkeiSbt).Select(item => item.RenkeiId).Contains(renkeiConf.RenkeiId))
                {
                    return SaveRenkeiStatus.InvalidRenkeiId;
                }
                else if (renkeiConf.Param.Length > 1000)
                {
                    return SaveRenkeiStatus.InvalidParam;
                }
                else if (renkeiConf.PtNumLength > 10)
                {
                    return SaveRenkeiStatus.InvalidPtNumLength;
                }
                else if (!renkeiTemplateMstModelList.Any(item => item.TemplateId == renkeiConf.TemplateId))
                {
                    return SaveRenkeiStatus.InvalidTemplateId;
                }
                else if (renkeiConf.IsInvalid != 0 && renkeiConf.IsInvalid != 1)
                {
                    return SaveRenkeiStatus.InvalidIsInvalid;
                }
                else if (renkeiConf.Biko.Length > 300)
                {
                    return SaveRenkeiStatus.InvalidBiko;
                }

                // validate RenkeiPathConf
                foreach (var pathModel in renkeiConf.RenkeiPathConfModelList)
                {
                    if (pathModel.Path.Length > 300)
                    {
                        return SaveRenkeiStatus.InvalidPath;
                    }
                    else if (pathModel.Machine.Length > 60)
                    {
                        return SaveRenkeiStatus.InvalidMachine;
                    }
                    else if (pathModel.Biko.Length > 300)
                    {
                        return SaveRenkeiStatus.InvalidBiko;
                    }
                    else if (pathModel.WorkPath.Length > 300)
                    {
                        return SaveRenkeiStatus.InvalidWorkPath;
                    }
                    else if (pathModel.Param.Length > 1000)
                    {
                        return SaveRenkeiStatus.InvalidParam;
                    }
                    else if (pathModel.User.Length > 100)
                    {
                        return SaveRenkeiStatus.InvalidUser;
                    }
                    else if (pathModel.PassWord.Length > 100)
                    {
                        return SaveRenkeiStatus.InvalidPassWord;
                    }
                    else if (pathModel.IsInvalid != 0 && pathModel.IsInvalid != 1)
                    {
                        return SaveRenkeiStatus.InvalidIsInvalid;
                    }
                }

                // validate renkeiTiming
                foreach (var timmingModel in renkeiConf.RenkeiTimingModelList)
                {
                    if (timmingModel.IsInvalid != 0 && timmingModel.IsInvalid != 1)
                    {
                        return SaveRenkeiStatus.InvalidIsInvalid;
                    }
                    else if (!eventMstModelList.Any(item => item.EventCd == timmingModel.EventCd))
                    {
                        return SaveRenkeiStatus.InvalidEventCd;
                    }
                }
            }
        }
        return SaveRenkeiStatus.ValidateSuccess;
    }
}
