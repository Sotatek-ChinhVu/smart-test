using Helper.Constants;
using System.Text.Json.Serialization;

namespace Domain.Models.VisitingListSetting
{
    public class VisitingListSettingModel
    {
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

        public VisitingListSettingModel(List<ReceptionTimeColorConfig> receptionTimeColorConfigs, List<ReceptionStatusColorConfig> receptionStatusColorConfigs)
        {
            ReceptionTimeColorConfigs = receptionTimeColorConfigs;
            ReceptionStatusColorConfigs = receptionStatusColorConfigs;
        }

    }

    public class ReceptionTimeColorConfig
    {
        public ReceptionTimeColorConfig(int duration)
        {
            Duration = duration;
        }

        [JsonConstructor]
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

        [JsonConstructor]
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
