using Domain.Models.MainMenu;
using UseCase.MainMenu;
using UseCase.MainMenu.SaveStatisticMenu;

namespace Interactor.MainMenu;

public class SaveStatisticMenuInteractor : ISaveStatisticMenuInputPort
{
    private readonly IStatisticRepository _statisticRepository;

    public SaveStatisticMenuInteractor(IStatisticRepository statisticRepository)
    {
        _statisticRepository = statisticRepository;
    }

    public SaveStatisticMenuOutputData Handle(SaveStatisticMenuInputData inputData)
    {
        try
        {
            var validateResult = ValidateInput(inputData);
            if (validateResult != SaveStatisticMenuStatus.ValidateSuccess)
            {
                return new SaveStatisticMenuOutputData(validateResult);
            }
            if (_statisticRepository.SaveStatisticMenu(inputData.HpId, inputData.UserId, ConvertToModelList(inputData.GrpId, inputData.StaticMenuList)))
            {
                return new SaveStatisticMenuOutputData(SaveStatisticMenuStatus.Successed);
            }
            return new SaveStatisticMenuOutputData(SaveStatisticMenuStatus.Failed);
        }
        finally
        {
            _statisticRepository.ReleaseResource();
        }
    }

    private SaveStatisticMenuStatus ValidateInput(SaveStatisticMenuInputData input)
    {
        var staMenuDBList = _statisticRepository.GetStatisticMenu(input.HpId, input.GrpId);
        var staGrpDBList = _statisticRepository.GetStaGrp(input.HpId, input.GrpId);
        if (input.GrpId != 0 && !staMenuDBList.Exists(item => item.GrpId == input.GrpId))
        {
            return SaveStatisticMenuStatus.InvalidGrpId;
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
                                                              menu.IsDeleted
                                          )).ToList();
        return result;
    }
}
