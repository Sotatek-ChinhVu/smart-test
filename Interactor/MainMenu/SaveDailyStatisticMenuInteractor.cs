using Domain.Models.MainMenu;
using UseCase.MainMenu;
using UseCase.MainMenu.SaveDailyStatisticMenu;

namespace Interactor.MainMenu;

public class SaveDailyStatisticMenuInteractor : ISaveDailyStatisticMenuInputPort
{
    private readonly IStatisticRepository _statisticRepository;

    public SaveDailyStatisticMenuInteractor(IStatisticRepository statisticRepository)
    {
        _statisticRepository = statisticRepository;
    }

    public SaveDailyStatisticMenuOutputData Handle(SaveDailyStatisticMenuInputData inputData)
    {
        try
        {
            var validateResult = ValidateInput(inputData);
            if (validateResult != SaveDailyStatisticMenuStatus.ValidateSuccess)
            {
                return new SaveDailyStatisticMenuOutputData(validateResult);
            }
            if (_statisticRepository.SaveStatisticMenu(inputData.HpId, inputData.UserId, ConvertToModelList(inputData.GrpId, inputData.StaticMenuList)))
            {
                return new SaveDailyStatisticMenuOutputData(SaveDailyStatisticMenuStatus.Successed);
            }
            return new SaveDailyStatisticMenuOutputData(SaveDailyStatisticMenuStatus.Failed);
        }
        finally
        {
            _statisticRepository.ReleaseResource();
        }
    }

    private SaveDailyStatisticMenuStatus ValidateInput(SaveDailyStatisticMenuInputData input)
    {
        var staMenuDBList = _statisticRepository.GetStatisticMenu(input.HpId, input.GrpId);
        var staGrpDBList = _statisticRepository.GetStaGrp(input.HpId, input.GrpId);
        if (input.GrpId != 0 && !staMenuDBList.Exists(item => item.GrpId == input.GrpId))
        {
            return SaveDailyStatisticMenuStatus.InvalidGrpId;
        }
        foreach (var menu in input.StaticMenuList)
        {
            if (menu.MenuId != 0 && !staMenuDBList.Exists(item => item.MenuId == menu.MenuId))
            {
                return SaveDailyStatisticMenuStatus.InvalidMenuId;
            }
            else if (!staGrpDBList.Exists(item => item.ReportId == menu.ReportId))
            {
                return SaveDailyStatisticMenuStatus.InvalidReportId;
            }
            else if (menu.MenuName.Equals(string.Empty))
            {
                return SaveDailyStatisticMenuStatus.InvalidMenuName;
            }
        }
        return SaveDailyStatisticMenuStatus.ValidateSuccess;
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
                                                              menu.IsDeleted
                                          )).ToList();
        return result;
    }
}
