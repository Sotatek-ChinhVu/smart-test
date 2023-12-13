using Entity.Tenant;
using Helper.Constants;

namespace Reporting.DailyStatic.Model;

public class StatisticModelBase
{
    public StaMenu StaMenu { get; set; } = new();

    public List<StaConf> ListStaConf { get; set; } = new();

    protected void SettingConfig(int configId, string value)
    {
        var conf = ListStaConf.Find(x => x.ConfId == configId);

        if (conf == null)
        {
            conf = new StaConf();
            conf.MenuId = StaMenu.MenuId;
            conf.ConfId = configId;

            ListStaConf.Add(conf);
            if (ModelStatus == ModelStatus.None)
            {
                ModelStatus = ModelStatus.Modified;
            }
        }
        else
        {

            if (conf.Val != value && ModelStatus == ModelStatus.None)
            {
                ModelStatus = ModelStatus.Modified;
            }
        }

        conf.Val = value;
    }

    protected string GetValueConf(int config)
    {
        var conf = ListStaConf.Find(x => x.ConfId == config);

        if (conf == null)
        {
            return string.Empty;
        }

        return conf.Val ?? string.Empty;
    }

    public ModelStatus ModelStatus { get; set; }
}
