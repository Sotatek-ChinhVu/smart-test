using Domain.Models.SystemConf;
using Domain.Models.UserConf;
using Helper.Common;
using Helper.Constants;

namespace Domain.Models.Reception;

public class ReceptionSettings
{
    public ReceptionSettings(List<UserConfModel> userConfigs, List<SystemConfModel> systemConfigs)
    {
        ApplyUserConfigs(userConfigs);
        ApplySystemConfigs(systemConfigs);
    }

    public string FontName { get; private set; } = "Yu Gothic UI";
    public int FontSize { get; private set; } = 13;
    public int AutoRefresh { get; private set; } = 60;
    public int MouseWheel { get; private set; } = 3;
    public int KanFocus { get; private set; } = 0;
    public int SelectTodoSetting { get; private set; } = 0;

    public List<ReceptionTimeColorConfig> ReceptionTimeColorConfigs { get; private set; } = new()
    {
        new(15), new(30), new(999)
    };

    public List<ReceptionStatusColorConfig> ReceptionStatusColorConfigs { get; private set; } = new();

    public List<ReceptionSettingsComment> ReceptionComments { get; private set; } = new()
    {
        new(0, "TODO: Reception comments.")
    };

    public List<ReceptionSettingsComment> FreeComments { get; private set; } = new()
    {
        new(0, "TODO: Free comments.")
    };

    private void ApplyUserConfigs(List<UserConfModel> configs)
    {
        foreach (var config in configs)
        {
            ApplyUserConfig(config);
        }
    }

    private void ApplySystemConfigs(List<SystemConfModel> configs)
    {
        var receptionTimeColorConfigs = new List<ReceptionTimeColorConfig>();
        var receptionStatusColorConfigs = new List<ReceptionStatusColorConfig>();
        foreach (var config in configs)
        {
            if (config.GrpCd == SystemConfGroupCodes.ReceptionTimeColor)
            {
                receptionTimeColorConfigs.Add(new ReceptionTimeColorConfig(config.GrpEdaNo, config.Param));
            }
            else if(config.GrpCd == SystemConfGroupCodes.ReceptionStatusColor)
            {
                receptionStatusColorConfigs.Add(new ReceptionStatusColorConfig(config.GrpEdaNo, config.Param));
            }
        }

        if (receptionTimeColorConfigs.Any())
        {
            // Override the default settings
            ReceptionTimeColorConfigs = receptionTimeColorConfigs;
        }

        ReceptionStatusColorConfigs = StandardizeReceptionStatusColorConfigs(receptionStatusColorConfigs);
    }

    /// <summary>
    /// Some configs may not match with the status code or missing so we have to standardize the configs.
    /// </summary>
    /// <param name="configs"></param>
    /// <returns></returns>
    private List<ReceptionStatusColorConfig> StandardizeReceptionStatusColorConfigs(List<ReceptionStatusColorConfig> configs)
    {
        var receptionStatuses = RaiinState.ReceptionStatusToText.Select(pair => pair.Key).ToList();
        var matchingStatusVsConfigQuery =
            from status in receptionStatuses
            join config in configs on status equals config.Status into colorConfigs
            from colorConfig in colorConfigs.DefaultIfEmpty() // Left join with statuses
            orderby status
            select colorConfig ?? new ReceptionStatusColorConfig(status); // Set the default value for the missing config

        return matchingStatusVsConfigQuery.ToList();
    }

    private void ApplyUserConfig(UserConfModel config)
    {
        switch (config.GrpCd)
        {
            case UserConfCommon.GroupCodes.Font:
                FontName = config.Param;
                FontSize = config.Val;
                break;
            case UserConfCommon.GroupCodes.AutoRefresh:
                AutoRefresh = config.Val;
                break;
            case UserConfCommon.GroupCodes.MouseWheel:
                MouseWheel = config.Val;
                break;
            case UserConfCommon.GroupCodes.KanFocus:
                KanFocus = config.Val;
                break;
            case UserConfCommon.GroupCodes.SelectTodoSetting:
                SelectTodoSetting = config.Val;
                break;
            default:
                break;
        }
    }
}

public class ReceptionTimeColorConfig
{
    public ReceptionTimeColorConfig(int duration)
    {
        Duration = duration;
    }

    public ReceptionTimeColorConfig(int duration, string color)
    {
        Duration = duration;
        Color = color;
    }

    public int Duration { get; private set; }
    public string Color { get; private set; } = string.Empty;
}

public class ReceptionStatusColorConfig
{
    public ReceptionStatusColorConfig(int status)
    {
        Status = status;
    }

    public ReceptionStatusColorConfig(int status, string color)
    {
        Status = status;
        Color = color;
    }

    public int Status { get; private set; }
    public string StatusText => RaiinState.ReceptionStatusToText[Status];
    public string Color { get; private set; } = string.Empty;
}

public class ReceptionSettingsComment
{
    public ReceptionSettingsComment(int index, string text)
    {
        Index = index;
        Text = text;
    }

    public int Index { get; private set; }
    public string Text { get; private set; }
}
