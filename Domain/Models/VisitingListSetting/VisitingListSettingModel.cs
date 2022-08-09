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

        public List<ReceptionTimeColorConfig> ReceptionTimeColorConfigs { get; private set; }

        public List<ReceptionStatusColorConfig> ReceptionStatusColorConfigs { get; private set; }

        public List<ReceptionSettingsComment> ReceptionComments { get; private set; } = new()
        {
            new(0, "TODO: Reception comments.")
        };

        public List<ReceptionSettingsComment> FreeComments { get; private set; } = new()
        {
            new(0, "TODO: Free comments.")
        };

        public VisitingListSettingModel(string fontName, int fontSize, int autoRefresh, int mouseWheel, int kanFocus, int selectTodoSetting, List<ReceptionTimeColorConfig> receptionTimeColorConfigs, List<ReceptionStatusColorConfig> receptionStatusColorConfigs)
        {
            FontName = fontName;
            FontSize = fontSize;
            AutoRefresh = autoRefresh;
            MouseWheel = mouseWheel;
            KanFocus = kanFocus;
            SelectTodoSetting = selectTodoSetting;
            ReceptionTimeColorConfigs = receptionTimeColorConfigs;
            ReceptionStatusColorConfigs = receptionStatusColorConfigs;
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
