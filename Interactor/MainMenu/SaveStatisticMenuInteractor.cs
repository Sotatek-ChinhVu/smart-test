using Domain.Models.MainMenu;
using Domain.Models.User;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using Reporting.DailyStatic.Enum;
using UseCase.MainMenu;
using UseCase.MainMenu.SaveStatisticMenu;
using static Helper.Constants.UserConst;

namespace Interactor.MainMenu;

public class SaveStatisticMenuInteractor : ISaveStatisticMenuInputPort
{
    private readonly IStatisticRepository _statisticRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITenantProvider _tenantProvider;
    private readonly ILoggingHandler _loggingHandler;

    public SaveStatisticMenuInteractor(ITenantProvider tenantProvider, IStatisticRepository statisticRepository, IUserRepository userRepository)
    {
        _statisticRepository = statisticRepository;
        _userRepository = userRepository;
        _tenantProvider = tenantProvider;
        _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
    }

    public SaveStatisticMenuOutputData Handle(SaveStatisticMenuInputData inputData)
    {
        try
        {
            var validateResult = ValidateInput(inputData);
            if (validateResult != SaveStatisticMenuStatus.ValidateSuccess)
            {
                return new SaveStatisticMenuOutputData(0, validateResult);
            }
            var resultSave = _statisticRepository.SaveStatisticMenu(inputData.HpId, inputData.UserId, ConvertToModelList(inputData.GrpId, inputData.StaticMenuList));
            if (resultSave.success)
            {
                return new SaveStatisticMenuOutputData(resultSave.menuIdTemp, SaveStatisticMenuStatus.Successed);
            }
            return new SaveStatisticMenuOutputData(0, SaveStatisticMenuStatus.Failed);
        }
        catch (Exception ex)
        {
            _loggingHandler.WriteLogExceptionAsync(ex);
            throw;
        }
        finally
        {
            _statisticRepository.ReleaseResource();
            _userRepository.ReleaseResource();
            _loggingHandler.Dispose();
        }
    }

    private SaveStatisticMenuStatus ValidateInput(SaveStatisticMenuInputData input)
    {
        var staMenuDBList = _statisticRepository.GetStatisticMenu(input.HpId, input.GrpId);
        var staGrpDBList = _statisticRepository.GetStaGrp(input.HpId, input.GrpId);
        if (input.GrpId != 0 && staMenuDBList.Any() && !staMenuDBList.Exists(item => item.GrpId == input.GrpId))
        {
            return SaveStatisticMenuStatus.InvalidGrpId;
        }
        else if (staGrpDBList.Exists(item => GetPermissionSta(input.HpId, input.UserId, item.ReportId) != PermissionType.Unlimited))
        {
            return SaveStatisticMenuStatus.NoPermission;
        }
        foreach (var menu in input.StaticMenuList)
        {
            if (menu.MenuId != 0 && staMenuDBList.Any() && !staMenuDBList.Exists(item => item.MenuId == menu.MenuId))
            {
                return SaveStatisticMenuStatus.InvalidMenuId;
            }
            else if (staGrpDBList.Any() && !staGrpDBList.Exists(item => item.ReportId == menu.ReportId))
            {
                return SaveStatisticMenuStatus.InvalidReportId;
            }
            else if (menu.MenuName.Equals(string.Empty))
            {
                return SaveStatisticMenuStatus.InvalidMenuName;
            }
        }
        return SaveStatisticMenuStatus.ValidateSuccess;
    }

    private List<StatisticMenuModel> ConvertToModelList(int grpId, List<StatisticMenuItem> statisticMenuItemList)
    {
        var result = statisticMenuItemList.Select(menu => new StatisticMenuModel(
                                                              menu.MenuId,
                                                              grpId,
                                                              menu.ReportId,
                                                              menu.SortNo,
                                                              menu.MenuName,
                                                              menu.IsPrint,
                                                              menu.StaConfigList
                                                                  .Select(conf => new StaConfModel(
                                                                                      menu.MenuId,
                                                                                      conf.ConfId,
                                                                                      conf.Val
                                                                  )).ToList(),
                                                              menu.IsDeleted,
                                                              menu.IsSaveTemp
                                          )).ToList();
        return result;
    }

    private PermissionType GetPermissionSta(int hpId, int userId, int reportId)
    {
        switch ((StatisticReportType)reportId)
        {
            case StatisticReportType.Sta1001:
                return GetPermissionByScreenCode(hpId, userId, FunctionCode.Sta1001);
            case StatisticReportType.Sta1002:
                return GetPermissionByScreenCode(hpId, userId, FunctionCode.Sta1002);
            case StatisticReportType.Sta1010:
                return GetPermissionByScreenCode(hpId, userId, FunctionCode.Sta1010);
            case StatisticReportType.Sta2001:
                return GetPermissionByScreenCode(hpId, userId, FunctionCode.Sta2001);
            case StatisticReportType.Sta2002:
                return GetPermissionByScreenCode(hpId, userId, FunctionCode.Sta2002);
            case StatisticReportType.Sta2003:
                return GetPermissionByScreenCode(hpId, userId, FunctionCode.Sta2003);
            case StatisticReportType.Sta2010:
                return GetPermissionByScreenCode(hpId, userId, FunctionCode.Sta2010);
            case StatisticReportType.Sta2011:
                return GetPermissionByScreenCode(hpId, userId, FunctionCode.Sta2011);
            case StatisticReportType.Sta2020:
                return GetPermissionByScreenCode(hpId, userId, FunctionCode.Sta2020);
            case StatisticReportType.Sta2021:
                return GetPermissionByScreenCode(hpId, userId, FunctionCode.Sta2021);
            case StatisticReportType.Sta3001:
                return GetPermissionByScreenCode(hpId, userId, FunctionCode.Sta3001);
            case StatisticReportType.Sta3010:
                return GetPermissionByScreenCode(hpId, userId, FunctionCode.Sta3010);
            case StatisticReportType.Sta3020:
                return GetPermissionByScreenCode(hpId, userId, FunctionCode.Sta3020);
            case StatisticReportType.Sta3030:
                return GetPermissionByScreenCode(hpId, userId, FunctionCode.Sta3030);
            case StatisticReportType.Sta3040:
                return GetPermissionByScreenCode(hpId, userId, FunctionCode.Sta3040);
            case StatisticReportType.Sta3041:
                return GetPermissionByScreenCode(hpId, userId, FunctionCode.Sta3041);
            case StatisticReportType.Sta3050:
                return GetPermissionByScreenCode(hpId, userId, FunctionCode.Sta3050);
            case StatisticReportType.Sta3060:
                return GetPermissionByScreenCode(hpId, userId, FunctionCode.Sta3060);
            case StatisticReportType.Sta3061:
                return GetPermissionByScreenCode(hpId, userId, FunctionCode.Sta3061);
            case StatisticReportType.Sta3070:
                return GetPermissionByScreenCode(hpId, userId, FunctionCode.Sta3070);
            case StatisticReportType.Sta3071:
                return GetPermissionByScreenCode(hpId, userId, FunctionCode.Sta3071);
            case StatisticReportType.Sta3080:
                return GetPermissionByScreenCode(hpId, userId, FunctionCode.Sta3080);
            default:
                return PermissionType.Unlimited;
        }
    }

    private PermissionType GetPermissionByScreenCode(int hpId, int userId, string screenCode)
    {
        return _userRepository.GetPermissionByScreenCode(hpId, userId, screenCode);
    }
}
