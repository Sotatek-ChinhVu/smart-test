using Helper.Common;
using Helper.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.VisitingListSetting
{
    public class VisitingListSettingModel
    {
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

        public VisitingListSettingModel(List<ConfigModel> userConfigList, List<ConfigModel> systemConfig)
        {
            ApplyUserConfig(userConfigList);
            ApplySystemConfigs(systemConfig);
        }

        private void ApplyUserConfig(List<ConfigModel> userConfigList)
        {
            foreach (var config in userConfigList)
            {
                switch (config.GrpCd)
                {
                    case UserConfCommon.GroupCodes.Font:
                        FontName = config.Param;
                        FontSize = (int)config.Value;
                        break;
                    case UserConfCommon.GroupCodes.AutoRefresh:
                        AutoRefresh = (int)config.Value;
                        break;
                    case UserConfCommon.GroupCodes.MouseWheel:
                        MouseWheel = (int)config.Value;
                        break;
                    case UserConfCommon.GroupCodes.KanFocus:
                        KanFocus = (int)config.Value;
                        break;
                    case UserConfCommon.GroupCodes.SelectTodoSetting:
                        SelectTodoSetting = (int)config.Value;
                        break;
                    default:
                        break;
                }
            }
        }

        private void ApplySystemConfigs(List<ConfigModel> configs)
        {
            ReceptionTimeColorConfigs = configs
                .Where(c => c.GrpCd == SystemConfGroupCodes.ReceptionTimeColor)
                .Select(c => new ReceptionTimeColorConfig(c.EdaNo, c.Param))
                .ToList();

            var receptionStatusColorConfigs = configs
                .Where(c => c.GrpCd == SystemConfGroupCodes.ReceptionStatusColor)
                .Select(c => new ReceptionStatusColorConfig(c.EdaNo, c.Param))
                .ToList();

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
    }

    public class ConfigModel
    {
        public int GrpCd { get; private set; }

        public string Param { get; private set; }

        public double Value { get; private set; }

        public int EdaNo { get; private set; }

        public ConfigModel(int grpCd, string param, double value, int edaNo)
        {
            GrpCd = grpCd;
            Param = param;
            Value = value;
            EdaNo = edaNo;
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
}
